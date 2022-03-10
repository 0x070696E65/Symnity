using System;
using Symbol.Builders;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using System.Numerics;
using Symnity.Core.Format;

namespace Symnity.Model.Transactions
{
    public class MosaicSupplyChangeTransaction : Transaction
    {
        /**
         * The unresolved mosaic id.
         */
        public UnresolvedMosaicId MosaicId;

        /**
         * The supply type.
         */
        public MosaicSupplyChangeAction Action;

        /**
         * The supply change in units for the mosaic.
         */
        public long Delta;

        /**
         * Create a mosaic supply change transaction object
         * @param deadline - The deadline to include the transaction.
         * @param mosaicId - The unresolved mosaic id.
         * @param action - The supply change action (increase | decrease).
         * @param delta - The supply change in units for the mosaic.
         * @param networkType - The network type.
         * @param maxFee - (Optional) Max fee defined by the sender
         * @param signature - (Optional) Transaction signature
         * @param signer - (Optional) Signer public account
         * @returns {MosaicSupplyChangeTransaction}
         */
        public static MosaicSupplyChangeTransaction Create(
            Deadline deadline,
            UnresolvedMosaicId mosaicId,
            MosaicSupplyChangeAction action,
            long delta,
            NetworkType networkType,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new MosaicSupplyChangeTransaction(
                networkType,
                TransactionVersion.MOSAIC_SUPPLY_CHANGE,
                deadline,
                maxFee,
                mosaicId,
                action,
                delta,
                signature,
                signer
            );
        }

        /**
         * @param networkType
         * @param version
         * @param deadline
         * @param maxFee
         * @param mosaicId
         * @param action
         * @param delta
         * @param signature
         * @param signer
         * @param transactionInfo
         */
        public MosaicSupplyChangeTransaction(
            NetworkType networkType,
            byte version,
            Deadline deadline,
            long maxFee,
            UnresolvedMosaicId mosaicId,
            MosaicSupplyChangeAction action,
            long delta,
            string signature,
            PublicAccount signer,
            TransactionInfo transactionInfo = null
        )
            : base(TransactionType.MOSAIC_SUPPLY_CHANGE, networkType, version, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            MosaicId = mosaicId;
            Action = action;
            Delta = delta;
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
        public new MosaicSupplyChangeTransactionBuilder CreateBuilder() {
            return new MosaicSupplyChangeTransactionBuilder(
                GetSignatureAsBuilder(),
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.MOSAIC_SUPPLY_CHANGE),
                new AmountDto(MaxFee),
                new TimestampDto(Deadline.AdjustedValue),
                new UnresolvedMosaicIdDto(MosaicId.GetIdAsLong()),
                new AmountDto(Delta),
                (MosaicSupplyChangeActionDto)Enum.ToObject(typeof(MosaicSupplyChangeActionDto), Action)
            );
        }

        /**
         * @internal
         * @returns {EmbeddedTransactionBuilder}
         */
        public override EmbeddedTransactionBuilder ToEmbeddedTransaction() {
            return new EmbeddedMosaicSupplyChangeTransactionBuilder(
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.MOSAIC_SUPPLY_CHANGE),
                new UnresolvedMosaicIdDto(MosaicId.GetIdAsLong()),
                new AmountDto(Delta),
                (MosaicSupplyChangeActionDto)Enum.ToObject(typeof(MosaicSupplyChangeActionDto), Action)
            );
        }
    }
}