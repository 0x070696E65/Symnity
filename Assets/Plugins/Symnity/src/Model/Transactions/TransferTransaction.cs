using System;
using System.Collections.Generic;
using System.Numerics;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using UnityEngine;

namespace Symnity.Model.Transactions
{
    /**
     * Transfer transactions contain data about transfers of mosaics and message to another account.
     */
    public class TransferTransaction : Transaction
    {
        /**
         * The address of the recipient address.
         */
        // とりあえずネームスペースは無視
        public UnresolvedAddress RecipientAddress;

        /**
         * The array of Mosaic objects.
         */
        public List<Mosaic> Mosaics;

        /**
         * The transaction message of 2048 characters.
         */
        // messageもとりあえず空で
        public Message Message;
        
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
        public static TransferTransaction Create(
            Deadline deadline,
            UnresolvedAddress recipientAddress,
            List<Mosaic> mosaics,
            Message message,
            NetworkType networkType,
            long maxFee = 0,
            string signature = null,
            PublicAccount signer = null
        )
        {
            return new TransferTransaction(
                networkType,
                TransactionVersion.TRANSFER,
                deadline,
                maxFee,
                recipientAddress,
                mosaics,
                message,
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
        public TransferTransaction(
            NetworkType networkType, byte version, Deadline deadline, long maxFee,
            UnresolvedAddress recipientAddress, List<Mosaic> mosaics, Message message, string signature = null,
            PublicAccount signer = null, TransactionInfo transactionInfo = null
            )
            : base(TransactionType.TRANSFER, networkType, version, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            RecipientAddress = recipientAddress;
            Mosaics = mosaics;
            Message = message;
        }

        public byte[] ConcatTypedArrays(byte[] arr1, byte[] arr2)
        {
            var newArray = new byte[arr1.Length + arr2.Length];
            arr1.CopyTo(newArray, 0);
            arr2.CopyTo(newArray, arr1.Length);
            return newArray;
        }
        
        
        /**
         * Return message buffer
         * @internal
         * @returns {Uint8Array}
         */
        public byte[] GetMessageBuffer() {
            if (Message == null || Message.Payload == null) {
                return new byte[0];
            }
            var messgeHex = Message.Type == MessageType.PersistentHarvestingDelegationMessage
                ? Message.Payload
                : ConvertUtils.Utf8ToHex(Message.Payload);
            var payloadBuffer = ConvertUtils.GetBytes(messgeHex);
            var typeBuffer = new []{(byte)Message.Type};
            return Message.Type == MessageType.PersistentHarvestingDelegationMessage || Message.Payload == null
                ? payloadBuffer
                : ConcatTypedArrays(typeBuffer, payloadBuffer);
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
        public new TransferTransactionBuilder CreateBuilder()
        {
            var listUnresolvedMosaicBuilder = new List<UnresolvedMosaicBuilder>(){};
            Mosaics.ForEach(mosaic =>
            {
                listUnresolvedMosaicBuilder.Add(new UnresolvedMosaicBuilder(new UnresolvedMosaicIdDto(mosaic.GetId().GetIdAsLong()),
                    new AmountDto(mosaic.Amount)));
            });
            return new TransferTransactionBuilder(
                GetSignatureAsBuilder(),
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.TRANSFER),
                new AmountDto(MaxFee),
                new TimestampDto(Deadline.AdjustedValue),
                new UnresolvedAddressDto(RecipientAddress.EncodeUnresolvedAddress()),
                listUnresolvedMosaicBuilder,
                GetMessageBuffer()
            );
        }
        
        /**
         * @internal
         * @returns {EmbeddedTransactionBuilder}
         */
        public override EmbeddedTransactionBuilder ToEmbeddedTransaction() {
            var listUnresolvedMosaicBuilder = new List<UnresolvedMosaicBuilder>(){};
            Mosaics.ForEach(mosaic =>
            {
                listUnresolvedMosaicBuilder.Add(new UnresolvedMosaicBuilder(new UnresolvedMosaicIdDto(mosaic.GetId().GetIdAsLong()),
                    new AmountDto(mosaic.Amount)));
            });
            return new EmbeddedTransferTransactionBuilder(
                GetSignerAsBuilder(),
                VersionToDTO(),
                (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte) NetworkType),
                (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short) TransactionType.TRANSFER),
                new UnresolvedAddressDto(RecipientAddress.EncodeUnresolvedAddress()),
                listUnresolvedMosaicBuilder,
                GetMessageBuffer()
            );
        }
    }
}