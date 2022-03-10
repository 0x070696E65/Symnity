using System;
using System.Numerics;
using Symnity.Core.Crypto;

namespace Symnity.Core.Format
{
    public class IdGenerator
    {
        private static long ID_GENERATOR_FLAG = -0x8000000000000000L;
        /**
         * Generates a mosaic id given a nonce and a address.
         * @param {object} nonce The mosaic nonce.
         * @param {object} ownerAddress The address.
         * @returns {module:coders/uint64~uint64} The mosaic id.
         */
        public static BigInteger GenerateMosaicId(int nonce, byte[] publicKey) {
            var resultArray = new byte[8];
            Array.Copy(CryptoUtilities.CreateSha2356Hashes(BitConverter.GetBytes(nonce), publicKey), resultArray, 8);
            var result = BitConverter.ToInt64(resultArray, 0);
            // Unset the high bit for mosaic id
            return new BigInteger(result & ~ID_GENERATOR_FLAG);
        }
    }
}