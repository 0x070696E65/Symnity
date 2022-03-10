using System;
using Org.BouncyCastle.Crypto.Digests;

namespace Symnity.Core.Crypto
{
    [Serializable]
    public static class CryptoUtilities
    {
        public static byte Key_Size = 32;
        
        public static byte[] CreateSha2356Hash(byte[] hashBytes)
        {
            var sha3256Digest = new Sha3Digest(256);
            var sha3256Hash = new byte[sha3256Digest.GetDigestSize()];
            sha3256Digest.BlockUpdate(hashBytes, 0, hashBytes.Length);
            sha3256Digest.DoFinal(sha3256Hash, 0);
            return sha3256Hash;
        }
        
        public static byte[] CreateSha2356Hashes(params byte[][] hashBytes)
        {
            var sha3256Digest = new Sha3Digest(256);
            var sha3256Hash = new byte[sha3256Digest.GetDigestSize()];
            foreach(var i in hashBytes){
                sha3256Digest.BlockUpdate(i, 0, i.Length);
            }            
            sha3256Digest.DoFinal(sha3256Hash, 0);
            return sha3256Hash;
        }
    }
}