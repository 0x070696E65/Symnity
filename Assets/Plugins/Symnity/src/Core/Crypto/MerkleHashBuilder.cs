using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Digests;

namespace Symnity.Core.Crypto
{
    [Serializable]
    public class MerkleHashBuilder
    {
        /**
         * The list of hashes used to calculate root hash.
         *
         * @var {Uint8Array}
         */
        protected List<List<byte>> Hashes = new List<List<byte>>{};

        /**
         * Length of produced merkle hash in bytes.
         *
         * @var {number}
         */
        public readonly int Length;

        /**
         * Constructor
         * @param length Hash size
         */
        public MerkleHashBuilder(
         /*
          * Length of produced merkle hash in bytes.
          *
          * @var {number}
          */
         int length
        )
        { 
            Length = length;
        }
        
        /**
         * Hash inner transactions
         *
         * @internal
         * @param hashes Inner transaction hashes
         * @return {List<byte>}
         */
        protected List<byte> Hash(List<List<byte>> hashes) {
         var hasher = new Sha3Digest(256);
         
         hashes.ForEach((hashVal) => {
             hasher.BlockUpdate(hashVal.ToArray(), 0, hashVal.Count);
         });

         var hash = new byte[Length];
         hasher.DoFinal(hash, 0);
         return new List<byte>(hash);
        }
        
        /**
        * Get root hash of Merkle Tree
        *
        * @internal
        * @param {Uint8Array[]} hashes Inner transaction hashes
        * @return {Uint8Array}
        */
        protected List<byte> CalculateRootHash(List<List<byte>> hashes) { 
            if (hashes.Count == 0) { 
                return new List<byte>(Length);
            }

            var numRemainingHashes = hashes.Count;
            while (numRemainingHashes > 1) {
                for (var i = 0; i < numRemainingHashes; i += 2) {
                    if (i + 1 < numRemainingHashes)
                    {
                        var a = new List<List<byte>> {Hashes[i], Hashes[i + 1]};
                        var b = a[0].ToArray();
                        var c = a[1].ToArray();
                        hashes.Insert( i / 2, Hash(new List<List<byte>> {Hashes[i], Hashes[i + 1]}));
                        continue;
                    }

                    // if there is an odd number of hashes, duplicate the last one
                    hashes.Insert(i / 2, Hash(new List<List<byte>> {Hashes[i], Hashes[i]}));
                    ++numRemainingHashes;
                }
                numRemainingHashes /= 2; 
            } 
            return hashes[0];
        }
        
        /**
         * Get root hash of Merkle tree
         *
         * @return {Uint8Array}
         */
        public List<byte> GetRootHash() {
         return CalculateRootHash(Hashes);
        }

        /**
         * Update hashes array (add hash)
         *
         * @param hash Inner transaction hash buffer
         * @return {MerkleHashBuilder}
         */
        public void Update(List<byte> hash) {
         Hashes.Add(hash);
        }
    }
}