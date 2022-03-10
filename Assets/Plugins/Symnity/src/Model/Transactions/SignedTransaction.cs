using System;
using Symnity.Model.Network;

namespace Symnity.Model.Transactions
{
    /**
     * SignedTransaction object is used to transfer the transaction data and the signature to the server
     * in order to initiate and broadcast a transaction.
     */
    [Serializable]
    public class SignedTransaction
    {
        /**
         * Transaction serialized data
         */
        public readonly string Payload;

        /**
         * Transaction hash
         */
        public readonly string Hash;

        /**
         * Transaction signerPublicKey
         */
        public readonly string SignerPublicKey;

        /**
         * Transaction type
         */
        public readonly TransactionType Type;

        /**
         * Signer network type
         */
        public readonly NetworkType NetworkType;

        /**
        * @param payload
        * @param hash
        * @param type
        * @param networkType
        */
        public SignedTransaction(string payload, string hash, string signerPublicKey, TransactionType type, NetworkType networkType)
        { 
            Payload = payload;
            Hash = hash;
            SignerPublicKey = signerPublicKey;
            Type = type;
            NetworkType = networkType;
            if (hash.Length != 64)
            {
                throw new Exception("hash must be 64 characters long");
            }
        }
    }
}