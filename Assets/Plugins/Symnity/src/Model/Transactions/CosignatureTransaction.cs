using System;
using Symnity.Core.Crypto;
using Symnity.Core.Format;
using Symnity.Model.Accounts;

namespace Symnity.Model.Transactions
{
    public class CosignatureTransaction
    {
        /**
         * Transaction to cosign.
         */
        public readonly AggregateTransaction TransactionToCosign;
            
        /*
         * @param transactionToCosign Aggregate transaction
         */
        public CosignatureTransaction(AggregateTransaction transactionToCosign)
        {
            TransactionToCosign = transactionToCosign;
        }

        /**
         * Create a cosignature transaction
         * @param transactionToCosign - Transaction to cosign.
         * @returns {CosignatureTransaction}
         */
        public static CosignatureTransaction Create(AggregateTransaction transactionToCosign) {
            return new CosignatureTransaction(transactionToCosign);
        }

        /**
         * Co-sign transaction with transaction payload (off chain)
         * Creating a new CosignatureSignedTransaction
         * @param account - The signing account
         * @param payload - off transaction payload (aggregated transaction is unannounced)
         * @param generationHash - Network generation hash
         * @returns {CosignatureSignedTransaction}
         */
        public static CosignatureSignedTransaction SignTransactionPayload(Account account, string payload, string generationHash) {
            var transactionHash = Transaction.CreateTransactionHash(payload, ConvertUtils.GetBytes(generationHash));
            return SignTransactionHash(account, transactionHash);
        }

        /**
         * Co-sign transaction with transaction hash (off chain)
         * Creating a new CosignatureSignedTransaction
         * @param account - The signing account
         * @param transactionHash - The hash of the aggregate transaction to be cosigned
         * @returns {CosignatureSignedTransaction}
         */
        public static CosignatureSignedTransaction SignTransactionHash(Account account, string transactionHash)
        {
            var hashBytes = ConvertUtils.GetBytes(transactionHash);
            var keyPairEncoded = KeyPair.CreateKeyPairFromPrivateKeyString(account.GetPrivateKey());
            var signature = KeyPair.Sign(keyPairEncoded, hashBytes);
        return new CosignatureSignedTransaction(transactionHash, ConvertUtils.ToHex(signature), account.GetPublicKey());
        }

        /**
         * Serialize and sign transaction creating a new SignedTransaction
         * @param account
         * @param transactionHash Transaction hash (optional)
         * @returns {CosignatureSignedTransaction}
         */
        public CosignatureSignedTransaction SignWith(Account account, string transactionHash = null) {
            var hash = transactionHash != null ? transactionHash : TransactionToCosign.TransactionInfo?.Hash;
            if (hash == null) {
                throw new Exception("Transaction to cosign should be announced first");
            }
            return SignTransactionHash(account, hash);
        }
    }
}