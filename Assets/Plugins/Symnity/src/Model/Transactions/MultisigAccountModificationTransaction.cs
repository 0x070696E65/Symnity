using System;
using System.Collections.Generic;
using System.Linq;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Network;

namespace Symnity.Model.Transactions
{
    public class MultisigAccountModificationTransaction : Transaction
    {
        /**
         * The number of signatures needed to approve a transaction.
         * If we are modifying and existing multi-signature account this indicates the relative change of the minimum cosignatories.
         */
        public readonly byte MinApprovalDelta;

        /**
         * The number of signatures needed to remove a cosignatory.
         * If we are modifying and existing multi-signature account this indicates the relative change of the minimum cosignatories.
         */
        public readonly byte MinRemovalDelta;

        /**
         * The Cosignatory address additions.
         */
        public readonly List<UnresolvedAddress> AddressAdditions;

        /**
         * The Cosignatory address deletion.
         */
        public readonly List<UnresolvedAddress> AddressDeletions;

        /**
         * @param networkType
         * @param version
         * @param deadline
         * @param maxFee
         * @param minApprovalDelta
         * @param minRemovalDelta
         * @param addressAdditions
         * @param addressDeletions
         * @param signature
         * @param signer
         * @param transactionInfo
         */
        public MultisigAccountModificationTransaction(
            NetworkType networkType,
            byte version,
            Deadline deadline,
            long maxFee,
            byte minApprovalDelta,
            byte minRemovalDelta,
            List<UnresolvedAddress> addressAdditions,
            List<UnresolvedAddress> addressDeletions,
            string signature = null,
            PublicAccount signer = null,
            TransactionInfo transactionInfo = null
        ) : base(TransactionType.MULTISIG_ACCOUNT_MODIFICATION, networkType, version, deadline, maxFee, signature,
            signer, transactionInfo)
        {
            MinApprovalDelta = minApprovalDelta;
            MinRemovalDelta = minRemovalDelta;
            AddressAdditions = addressAdditions;
            AddressDeletions = addressDeletions;
        }

        /**
         * Create a modify multisig account transaction object
         * @param deadline - The deadline to include the transaction.
         * @param minApprovalDelta - The min approval relative change.
         * @param minRemovalDelta - The min removal relative change.
         * @param addressAdditions - Cosignatory address additions.
         * @param addressDeletions - Cosignatory address deletions.
         * @param networkType - The network type.
         * @param maxFee - (Optional) Max fee defined by the sender
         * @param signature - (Optional) Transaction signature
         * @param signer - (Optional) Signer public account
         * @returns {MultisigAccountModificationTransaction}
         */
        public static MultisigAccountModificationTransaction Create(
            Deadline deadline,
            byte minApprovalDelta,
            byte minRemovalDelta,
            List<UnresolvedAddress> addressAdditions,
            List<UnresolvedAddress> addressDeletions,
            NetworkType networkType,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new MultisigAccountModificationTransaction(
                networkType,
                TransactionVersion.MULTISIG_ACCOUNT_MODIFICATION,
                deadline,
                maxFee,
                minApprovalDelta,
                minRemovalDelta,
                addressAdditions,
                addressDeletions,
                signature,
                signer
            );
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
        * @internal
        * @returns {byte[]}
        */
        public override byte[] GenerateBytes()
        {
            return CreateBuilder().Serialize();
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
        
        /**
         * @internal
         * @returns {TransactionBuilder}
         */
        public new MultisigAccountModificationTransactionBuilder CreateBuilder()
        {
            var addressAdditionsDto = new List<UnresolvedAddressDto> { };
            AddressAdditions.ForEach(address =>
            {
                addressAdditionsDto.Add(new UnresolvedAddressDto(address.EncodeUnresolvedAddress()));
            });
            var addressDeletionsDto = new List<UnresolvedAddressDto> { };
            AddressDeletions.ForEach(address =>
            {
                addressAdditionsDto.Add(new UnresolvedAddressDto(address.EncodeUnresolvedAddress()));
            });
            return new MultisigAccountModificationTransactionBuilder(
                GetSignatureAsBuilder(),
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.MULTISIG_ACCOUNT_MODIFICATION),
                new AmountDto(MaxFee),
                new TimestampDto(Deadline.AdjustedValue),
                MinRemovalDelta,
                MinApprovalDelta,
                addressAdditionsDto,
                addressDeletionsDto
            );
        }
        
        /**
         * @internal
         * @returns {EmbeddedTransactionBuilder}
         */
        public override EmbeddedTransactionBuilder ToEmbeddedTransaction() {
            var addressAdditionsDto = new List<UnresolvedAddressDto> { };
            AddressAdditions.ForEach(address =>
            {
                addressAdditionsDto.Add(new UnresolvedAddressDto(address.EncodeUnresolvedAddress()));
            });
            var addressDeletionsDto = new List<UnresolvedAddressDto> { };
            AddressDeletions.ForEach(address =>
            {
                addressAdditionsDto.Add(new UnresolvedAddressDto(address.EncodeUnresolvedAddress()));
            });
            return new EmbeddedMultisigAccountModificationTransactionBuilder(
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.MULTISIG_ACCOUNT_MODIFICATION),
                MinRemovalDelta,
                MinApprovalDelta,
                addressAdditionsDto,
                addressDeletionsDto
            );
        }
    }
}