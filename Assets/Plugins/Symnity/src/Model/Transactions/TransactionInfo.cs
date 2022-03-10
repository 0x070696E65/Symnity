using System;
using System.Numerics;

namespace Symnity.Model.Transactions
{
    /**
 * Transaction information model included in all transactions
 */
    [Serializable]
    public class TransactionInfo
    {
        /**
         * The block height in which the transaction was included.
         */
        public uint Height;

        /**
         * The index representing either transaction index/position within block or within an aggregate transaction.
         */
        public int Index;

        /*
        * The transaction db id.
        */
        public string Id;

        /*
        * The transaction hash.
        */
        public string Hash;

        /**
        * The transaction merkle hash.
        */
        public string MerkleComponentHash;

        public BigInteger Timestamp;

        public byte FeeMultiplier;

        public Transaction Transaction;

        /**
        * @param height
        * @param index
        * @param id
        * @param hash
        * @param merkleComponentHash
        */
        public TransactionInfo(uint height, int index, string id, string hash, string merkleComponentHash, Transaction transaction, byte feeMultiplier, BigInteger timestamp)
        {
            Height = height;
            Index = index;
            Id = id;
            Hash = hash;
            MerkleComponentHash = merkleComponentHash;
            Timestamp = timestamp;
            FeeMultiplier = feeMultiplier;
            Transaction = transaction;
        }
    }
}