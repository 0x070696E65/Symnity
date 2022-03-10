using System;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;

namespace Symnity.Model.Transactions
{
    public class MosaicSupplyRevocationTransaction : Transaction
    {
        /**
         * Address from which tokens should be revoked.
         */
        public readonly UnresolvedAddress SourceAddress;

        /**
         * Revoked mosaic and amount.
         */
        public readonly Mosaic Mosaic;

        /**
         * Create a mosaic supply revocation transaction object
         * @param deadline - The deadline to include the transaction.
         * @param sourceAddress - Address from which tokens should be revoked.
         * @param mosaic - Revoked mosaic and amount.
         * @param networkType - The network type.
         * @param maxFee - (Optional) Max fee defined by the sender
         * @param signature - (Optional) Transaction signature
         * @param signer - (Optional) Signer public account
         * @returns {MosaicSupplyRevocationTransaction}
         */
        public static MosaicSupplyRevocationTransaction Create(
            Deadline deadline,
            UnresolvedAddress sourceAddress,
            Mosaic mosaic,
            NetworkType networkType,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new MosaicSupplyRevocationTransaction(
                networkType,
                TransactionVersion.MOSAIC_SUPPLY_REVOCATION,
                deadline,
                maxFee,
                sourceAddress,
                mosaic,
                signature,
                signer
            );
        }

        /**
         * @param networkType
         * @param version
         * @param deadline
         * @param maxFee
         * @param sourceAddress
         * @param mosaic
         * @param signature
         * @param signer
         * @param transactionInfo
         */
        public MosaicSupplyRevocationTransaction(
            NetworkType networkType,
            byte version,
            Deadline deadline,
            long maxFee,
            UnresolvedAddress sourceAddress,
            Mosaic mosaic,
            string signature = null,
            PublicAccount signer = null,
            TransactionInfo transactionInfo = null
        )
            : base(TransactionType.MOSAIC_SUPPLY_REVOCATION, networkType, version, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            SourceAddress = sourceAddress;
            Mosaic = mosaic;
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
        public new MosaicSupplyRevocationTransactionBuilder CreateBuilder()
        {
            var builder = new UnresolvedMosaicBuilder(
                new UnresolvedMosaicIdDto(Mosaic.Id.GetIdAsLong()),
                new AmountDto(Mosaic.Amount)
            );

            return new MosaicSupplyRevocationTransactionBuilder(
                GetSignatureAsBuilder(),
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto) Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto) Enum.ToObject(typeof(TransactionTypeDto),
                    (short) TransactionType.MOSAIC_SUPPLY_REVOCATION),
                new AmountDto(MaxFee),
                new TimestampDto(Deadline.AdjustedValue),
                new UnresolvedAddressDto(SourceAddress.EncodeUnresolvedAddress()),
                builder
            );
        }

        /**
         * @internal
         * @returns {EmbeddedTransactionBuilder}
         */
        public override EmbeddedTransactionBuilder ToEmbeddedTransaction() {
            var builder = new UnresolvedMosaicBuilder(
                new UnresolvedMosaicIdDto(Mosaic.Id.GetIdAsLong()),
                new AmountDto(Mosaic.Amount)
            );
            return new EmbeddedMosaicSupplyRevocationTransactionBuilder(
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto) Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto) Enum.ToObject(typeof(TransactionTypeDto),(short) TransactionType.MOSAIC_SUPPLY_REVOCATION),
                new UnresolvedAddressDto(SourceAddress.EncodeUnresolvedAddress()),
                builder
            );
        }
    }
}