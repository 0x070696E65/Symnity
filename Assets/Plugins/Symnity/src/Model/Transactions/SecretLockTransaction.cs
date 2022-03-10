using System;
using System.Collections.Generic;
using System.Numerics;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.BlockChain;
using Symnity.Model.Lock;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using UnityEngine;

namespace Symnity.Model.Transactions
{
    /**
     * Transfer transactions contain data about transfers of mosaics and message to another account.
     */
    public class SecretLockTransaction : Transaction
    {
        /**
         * The locked mosaic.
         */
        public Mosaic Mosaic;

        /**
         * The duration for the funds to be released or returned.
         */
        public BlockDuration Duration;

        /**
         * The hash algorithm, secret is generated with.
         */
        public LockHashAlgorithm HashAlgorithm;

        /**
         * The proof hashed.
         */
        public string Secret;

        /**
         * The unresolved recipientAddress of the funds.
         */
        public UnresolvedAddress RecipientAddress;
        
        /**
         * Create a transfer transaction object.
         *
         * - This method can also be used to create PersistentDelegationRequestTransaction
         * with `PersistentHarvestingDelegationMessage` provided.
         * @param deadline - The deadline to include the transaction.
         * @param recipientAddress - The recipient address of the transaction.
         * @param mosaics - The array of mosaics.
         * @param message - The transaction message.
         * @param networkType - The network type.
         * @param maxFee - (Optional) Max fee defined by the sender
         * @param signature - (Optional) Transaction signature
         * @param signer - (Optional) Signer public account
         * @returns {TransferTransaction}
         */
        public static SecretLockTransaction Create(
            Deadline deadline,
            Mosaic mosaic,
            BlockDuration duration,
            LockHashAlgorithm hashAlgorithm,
            string secret,
            UnresolvedAddress recipientAddress,
            NetworkType networkType,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new SecretLockTransaction(
                networkType,
                TransactionVersion.SECRET_LOCK,
                deadline,
                maxFee,
                mosaic,
                duration,
                hashAlgorithm,
                secret,
                recipientAddress,
                signature,
                signer
            );
        }

        /**
         * @param networkType
         * @param version
         * @param deadline
         * @param maxFee
         * @param recipientAddress
         * @param mosaics
         * @param message
         * @param signature
         * @param signer
         * @param transactionInfo
         */
        public SecretLockTransaction(
            NetworkType networkType, byte version, Deadline deadline, long maxFee, Mosaic mosaic, BlockDuration duration, LockHashAlgorithm hashAlgorithm, string secret, UnresolvedAddress recipientAddress, string signature = null,
            PublicAccount signer = null, TransactionInfo transactionInfo = null
            )
            : base(TransactionType.SECRET_LOCK, networkType, version, deadline, maxFee, signature, signer, transactionInfo)
        {
            if (!hashAlgorithm.LockHashAlgorithmLengthValidator(secret)) {
                throw new Exception("HashAlgorithm and Secret have incompatible length or not hexadecimal string");
            }
            RecipientAddress = recipientAddress;
            Mosaic = mosaic;
            HashAlgorithm = hashAlgorithm;
            Secret = secret;
            Duration = duration;
        }
        
        /**
         * @description Get secret bytes
         * @returns {Uint8Array}
         * @memberof SecretLockTransaction
         */
        public byte[] GetSecretByte()
        {
            return ConvertUtils.GetBytes(64 > Secret.Length ? Secret.PadRight(64, '0') : Secret);
        }

        /**
         * @override Transaction.size()
         * @description get the byte size of a transaction using the builder
         * @returns {number}
         * @memberof TransferTransaction
         */ 
        public override int GetSize() {
            return _payloadSize ?? CreateBuilder().GetSize();
        }
        
        /**
         * @description Serialize a transaction object
         * @returns {string}
         * @memberof Transaction
         */
        public override string Serialize()
        {
            return ConvertUtils.ToHex(GenerateBytes());
        }
        
        /*
        * @internal
        * @returns {byte[]}
        */
        public override byte[] GenerateBytes()
        {
            return CreateBuilder().Serialize();
        }
        
        /**
         * @internal
         * @returns {TransactionBuilder}
         */
        public new SecretLockTransactionBuilder CreateBuilder()
        {
            return new SecretLockTransactionBuilder(
                GetSignatureAsBuilder(),
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.SECRET_LOCK),
                new AmountDto(MaxFee),
                new TimestampDto(Deadline.AdjustedValue),
                new UnresolvedAddressDto(RecipientAddress.EncodeUnresolvedAddress()),
                new Hash256Dto(GetSecretByte()),
                new UnresolvedMosaicBuilder(new UnresolvedMosaicIdDto(Mosaic.Id.GetIdAsLong()), new AmountDto(Mosaic.Amount)),
                new BlockDurationDto(Duration.GetDuration()),
                (LockHashAlgorithmDto)Enum.ToObject(typeof(LockHashAlgorithmDto), (short) HashAlgorithm)
            );
        }
        
        /**
         * @internal
         * @returns {EmbeddedTransactionBuilder}
         */
        public override EmbeddedTransactionBuilder ToEmbeddedTransaction() {
            return new EmbeddedSecretLockTransactionBuilder(
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.SECRET_LOCK),
                new UnresolvedAddressDto(RecipientAddress.EncodeUnresolvedAddress()),
                new Hash256Dto(GetSecretByte()),
                new UnresolvedMosaicBuilder(new UnresolvedMosaicIdDto(Mosaic.Id.GetIdAsLong()), new AmountDto(Mosaic.Amount)),
                new BlockDurationDto(Duration.GetDuration()),
                (LockHashAlgorithmDto)Enum.ToObject(typeof(LockHashAlgorithmDto), (short) HashAlgorithm)
            );
        }
    }
}