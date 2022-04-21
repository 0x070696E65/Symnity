using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Symnity.Core.Format;
using Symnity.Http;
using Symnity.Http.Model;
using Symnity.Infrastructure.SearchCriteria;
using Symnity.Model.Accounts;
using Symnity.Model.BlockChain;
using Symnity.Model.Lock;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using UnityEngine;

namespace Symnity.Infrastructure
{
    public class TransactionRepository
    {
        private string Node;
        private Deadline deadline;
        private long maxFee;

        public TransactionRepository(string node)
        {
            Node = node;
        }

        public async UniTask<Model.Transactions.Transaction> GetTransaction(string transactionId, TransactionGroup transactionGroup)
        {
            var info = transactionGroup switch
            {
                TransactionGroup.Confirmed => await ApiTransaction.GetConfirmedTransaction(Node, transactionId),
                TransactionGroup.Unconfirmed => await ApiTransaction.GetUnconfirmedTransaction(Node, transactionId),
                TransactionGroup.Partial => await ApiTransaction.GetPartialTransaction(Node, transactionId),
                _ => throw new ArgumentOutOfRangeException(nameof(transactionGroup), transactionGroup, null)
            };
            var ea = await HttpUtilities.GetEpochAdjustment(Node);
            return GetTransactionInformation(info, ea);
        }

        public async UniTask<Page<Symnity.Model.Transactions.Transaction>> Search(TransactionSearchCriteria searchCriteria)
        {
            var result = await ApiTransaction.SearchConfirmedTransactions(Node, searchCriteria);
            var ea = await HttpUtilities.GetEpochAdjustment(Node);
            var list = result.data.Select(tx => GetTransactionInformation(tx, ea)).ToList();
            return new Page<Symnity.Model.Transactions.Transaction>(
                list,
                result.pagination.pageNumber,
                result.pagination.pageSize
                );
        }

        public async UniTask<string> Announce(SignedTransaction signedTransaction)
        {
            try
            {
                var result = await HttpUtilities.Announce(Node, signedTransaction.Payload);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("announce error: " + e.Message);
            }
        }
        
        private Model.Transactions.Transaction GetTransactionInformation(ApiTransaction.Datum info, int ea)
        {
            var type = info.transaction.type;
            var networkType = (NetworkType) Enum.ToObject(typeof(NetworkType), info.transaction.network);
            var version = (byte) info.transaction.version;
            
            deadline = Deadline.Create((int) long.Parse(info.transaction.deadline) / 1000 + ea);
            maxFee = long.Parse(info.transaction.maxFee);
            var signature = info.transaction.signature;
            var signer = PublicAccount.CreateFromPublicKey(info.transaction.signerPublicKey, networkType);
            var transactionInfo = new TransactionInfo(
                uint.Parse(info.meta.height),
                info.meta.index,
                info.id,
                info.meta.hash,
                info.meta.merkleComponentHash,
                (byte) info.meta.feeMultiplier,
                BigInteger.Parse(info.meta.timestamp)
            );
            switch (type)
            {
                case 16724:
                    return new TransferTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        Address.CreateFromEncoded(info.transaction.recipientAddress),
                        info.transaction.mosaics.Select(mosaic => new Mosaic(new MosaicId(mosaic.id), mosaic.amount)).ToList(),
                        MessageFactory.CreateMessageFromHex(info.transaction.message),
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16717:
                    return new MosaicDefinitionTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        new MosaicNonce(info.transaction.nonce),
                        new MosaicId(info.transaction.id),
                        new MosaicFlags(info.transaction.flags),
                        info.transaction.divisibility,
                        new BlockDuration(long.Parse(info.transaction.duration)),
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16973:
                    return new MosaicSupplyChangeTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        new MosaicId(info.transaction.mosaicId),
                        (MosaicSupplyChangeAction) Enum.ToObject(typeof(MosaicSupplyChangeAction),
                            info.transaction.network),
                        long.Parse(info.transaction.delta),
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16705:
                    var innerTransactionsComplete =
                        info.transaction.transactions.Select(innerTransactionDatum => GetInnerTransaction(innerTransactionDatum, ea)).ToList();
                    return new AggregateTransaction(
                        networkType,
                        TransactionType.AGGREGATE_COMPLETE,
                        version,
                        deadline,
                        maxFee,
                        innerTransactionsComplete,
                        info.transaction.cosignatures.Select(cosig => new AggregateTransactionCosignature(
                            cosig.signature,
                            PublicAccount.CreateFromPublicKey(cosig.signerPublicKey, networkType),
                            long.Parse(cosig.version))).ToList(),
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16961:
                    var innerTransactionsBonded =
                        info.transaction.transactions.Select(innerTransactionDatum => GetInnerTransaction(innerTransactionDatum, ea)).ToList();
                        return new AggregateTransaction(
                            networkType,
                            TransactionType.AGGREGATE_BONDED,
                            version,
                            deadline,
                            maxFee,
                            innerTransactionsBonded,
                            info.transaction.cosignatures.Select(cosig => new AggregateTransactionCosignature(
                                cosig.signature,
                                PublicAccount.CreateFromPublicKey(cosig.signerPublicKey, networkType),
                                long.Parse(cosig.version))).ToList(),
                            signature,
                            signer,
                            transactionInfo
                        );
                case 16722:
                    return new SecretLockTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        new Mosaic(new MosaicId(info.transaction.mosaicId), long.Parse(info.transaction.amount)),
                        new BlockDuration(long.Parse(info.transaction.duration)),
                        (LockHashAlgorithm) Enum.ToObject(typeof(LockHashAlgorithm), info.transaction.hashAlgorithm),
                        info.transaction.secret,
                        Address.CreateFromEncoded(info.transaction.recipientAddress)
                    );
                case 16978:
                    return new SecretProofTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        (LockHashAlgorithm) Enum.ToObject(typeof(LockHashAlgorithm), info.transaction.hashAlgorithm),
                        info.transaction.secret,
                        Address.CreateFromEncoded(info.transaction.recipientAddress),
                        info.transaction.proof,
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16708:
                    return new AccountMetadataTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        Address.CreateFromEncoded(info.transaction.targetAddress),
                        BigInteger.Parse(info.transaction.scopedMetadataKey, NumberStyles.AllowHexSpecifier),
                        (short) info.transaction.valueSizeDelta,
                        ConvertUtils.HexToChar(info.transaction.value),
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16964:
                    return new MosaicMetadataTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        Address.CreateFromEncoded(info.transaction.targetAddress),
                        BigInteger.Parse(info.transaction.scopedMetadataKey, NumberStyles.AllowHexSpecifier),
                        new MosaicId(info.transaction.targetMosaicId),
                        (short) info.transaction.valueSizeDelta,
                        ConvertUtils.HexToChar(info.transaction.value),
                        signature,
                        signer,
                        transactionInfo
                    );
                case 16725:
                    return new MultisigAccountModificationTransaction(
                        networkType,
                        version,
                        deadline,
                        maxFee,
                        (sbyte) info.transaction.minApprovalDelta,
                        (sbyte) info.transaction.minRemovalDelta,
                        new List<UnresolvedAddress>(info.transaction.addressAdditions.Select(addressAddition => Address.CreateFromEncoded(
                            addressAddition)).ToList()),
                        new List<UnresolvedAddress>(info.transaction.addressDeletions.Select(addressDeletion => Address.CreateFromEncoded(
                            addressDeletion)).ToList()),
                        signature,
                        signer,
                        transactionInfo
                    );
                default:
                    throw new Exception("This transaction type does not exist in Symnity. Please make a request to the developer.");
            }
        }
        
        private Model.Transactions.Transaction GetInnerTransaction(ApiTransaction.InnerTransactionDatum info, int ea)
        {
            var type = info.transaction.type;
            var networkType = (NetworkType) Enum.ToObject(typeof(NetworkType), info.transaction.network);
            var version = (byte) info.transaction.version;
            var signature = info.transaction.signature;
            var signer = PublicAccount.CreateFromPublicKey(info.transaction.signerPublicKey, networkType);
            var transactionInfo = new TransactionInfo(
                uint.Parse(info.meta.height),
                info.meta.index,
                info.id,
                info.meta.hash,
                info.meta.merkleComponentHash,
                (byte) info.meta.feeMultiplier,
                BigInteger.Parse(info.meta.timestamp)
            );
            return type switch
            {
                16724 => new TransferTransaction(networkType, version, deadline, maxFee,
                    Address.CreateFromEncoded(info.transaction.recipientAddress),
                    info.transaction.mosaics.Select(mosaic => new Mosaic(new MosaicId(mosaic.id), mosaic.amount))
                        .ToList(), MessageFactory.CreateMessageFromHex(info.transaction.message), signature, signer,
                    transactionInfo),
                16717 => new MosaicDefinitionTransaction(networkType, version, deadline, maxFee,
                    new MosaicNonce(info.transaction.nonce), new MosaicId(info.transaction.id),
                    new MosaicFlags(info.transaction.flags), info.transaction.divisibility,
                    new BlockDuration(long.Parse(info.transaction.duration)), signature, signer, transactionInfo),
                16973 => new MosaicSupplyChangeTransaction(networkType, version, deadline, maxFee,
                    new MosaicId(info.transaction.mosaicId),
                    (MosaicSupplyChangeAction) Enum.ToObject(typeof(MosaicSupplyChangeAction),
                        info.transaction.network), long.Parse(info.transaction.delta), signature, signer,
                    transactionInfo),
                16722 => new SecretLockTransaction(networkType, version, deadline, maxFee,
                    new Mosaic(new MosaicId(info.transaction.mosaicId), long.Parse(info.transaction.amount)),
                    new BlockDuration(long.Parse(info.transaction.duration)),
                    (LockHashAlgorithm) Enum.ToObject(typeof(LockHashAlgorithm), info.transaction.hashAlgorithm),
                    info.transaction.secret, Address.CreateFromEncoded(info.transaction.recipientAddress)),
                16978 => new SecretProofTransaction(networkType, version, deadline, maxFee,
                    (LockHashAlgorithm) Enum.ToObject(typeof(LockHashAlgorithm), info.transaction.hashAlgorithm),
                    info.transaction.secret, Address.CreateFromEncoded(info.transaction.recipientAddress),
                    info.transaction.proof, signature, signer, transactionInfo),
                16708 => new AccountMetadataTransaction(networkType, version, deadline, maxFee,
                    Address.CreateFromEncoded(info.transaction.targetAddress),
                    BigInteger.Parse(info.transaction.scopedMetadataKey, NumberStyles.AllowHexSpecifier),
                    (short) info.transaction.valueSizeDelta, ConvertUtils.HexToChar(info.transaction.value), signature,
                    signer, transactionInfo),
                16964 => new MosaicMetadataTransaction(networkType, version, deadline, maxFee,
                    Address.CreateFromEncoded(info.transaction.targetAddress),
                    BigInteger.Parse(info.transaction.scopedMetadataKey, NumberStyles.AllowHexSpecifier),
                    new MosaicId(info.transaction.targetMosaicId), (short) info.transaction.valueSizeDelta,
                    ConvertUtils.HexToChar(info.transaction.value), signature, signer, transactionInfo),
                16725 => new MultisigAccountModificationTransaction(networkType, version, deadline, maxFee,
                    (sbyte) info.transaction.minApprovalDelta, (sbyte) info.transaction.minRemovalDelta,
                    new List<UnresolvedAddress>(info.transaction.addressAdditions
                        .Select(addressAddition => Address.CreateFromEncoded(addressAddition))
                        .ToList()),
                    new List<UnresolvedAddress>(info.transaction.addressDeletions
                        .Select(addressDeletion => Address.CreateFromEncoded(addressDeletion))
                        .ToList()), signature, signer, transactionInfo),
                _ => throw new Exception(
                    "This transaction type does not exist in Symnity. Please make a request to the developer.")
            };
        }
    }
}