using System;
using System.IO;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Transactions;
namespace Symnity.Infrastructure.Transaction
{
    public class Serialization
    {
        /*public static Transaction CreateTransactionFromPayload(string payload, bool isEmbedded = false)
        {
            var ms = new MemoryStream(ConvertUtils.GetBytes(payload));
            var br = new BinaryReader(ms);
            EntityTypeDto type;
            byte version;
            if (isEmbedded)
            {
                var embeddedTransactionBuilder = EmbeddedTransactionBuilder.LoadFromBinary(br);
                type = EntityTypeDto.RESERVED.RawValueOf((short) embeddedTransactionBuilder.GetType());
                version = embeddedTransactionBuilder.GetVersion();
            }
            else
            {
                var transactionBuilder = TransactionBuilder.LoadFromBinary(br);
                type = EntityTypeDto.RESERVED.RawValueOf((short) transactionBuilder.GetType());
                version = transactionBuilder.GetVersion();
            }

            if ((int) type == (int) TransactionType.AGGREGATE_COMPLETE ||
                (int) type == (int) TransactionType.AGGREGATE_BONDED)
            {
                return AggregateTransaction.CreateFromPayload(payload);
            } else {
                throw new Exception($"Transaction type {type} not implemented yet for version {version}.");
            }
            //var transactionBuilder = isEmbedded ? EmbeddedTransactionBuilder.LoadFromBinary(br) : TransactionBuilder.LoadFromBinary(br);
        }*/
    }
}