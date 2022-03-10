using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Digests;
using Symnity.Model.Network;

namespace Symnity.Core.Format
{
    [Serializable]
    public class RawAddress
    {
        private static Dictionary<string, Dictionary<string, byte>> _constants = new Dictionary<string, Dictionary<string, byte>>
        {
            {
                "sizes",
                new Dictionary<string, byte>{
                    {"ripemd160", 20},
                    {"addressDecoded", 24}, 
                    {"addressEncoded", 39}, 
                    {"key", 32}, 
                    {"checksum", 3},
                }
            }                
        };
        
        /**
         * Converts an encoded address string to a decoded address.
         * @param {string} encoded The encoded address string.
         * @returns {Uint8Array} The decoded address corresponding to the input.
         */
        public static byte[] StringToAddress(string encoded){
            if (_constants["sizes"]["addressEncoded"] != encoded.Length) {
                throw new Exception(encoded + " does not represent a valid encoded address");
            }

            var bytes = Base32.FromBase32String(encoded + "A");
            Array.Resize(ref bytes, _constants["sizes"]["addressDecoded"]);
            return bytes;
        }
    
        /*
         * Converts a decoded address to an encoded address string.
         * @param {Uint8Array} decoded The decoded address.
         * @returns {string} The encoded address string corresponding to the input.
         */
        public static string AddressToString(byte[] decoded)
        {
            if (_constants["sizes"]["addressDecoded"] != decoded.Length)
            {
                throw new Exception(ConvertUtils.ToHex(decoded) + " does not represent a valid decoded address");
            }

            var padded  = new byte[_constants["sizes"]["addressDecoded"] + 1];
            Array.Copy(decoded, padded, decoded.Length);
            /*foreach (var VARIABLE in padded)
            {
                Console.WriteLine(VARIABLE);
            }*/
            return Base32.ToBase32String(padded).Substring(0, _constants["sizes"]["addressEncoded"]);
        }
        
        /**
         * Converts a public key to a decoded address for a specific network.
         * @param {Uint8Array} publicKey The public key.
         * @param {NetworkType} networkType The network identifier.
         * @returns {Uint8Array} The decoded address corresponding to the inputs.
         */
        public static byte[] PublicKeyToAddress(byte[] publicKey, NetworkType networkType)
        {
            // step 1: sha3 hash of the public key
            var sha3256Digest = new Sha3Digest(256);
            var sha3256Hash = new byte[sha3256Digest.GetDigestSize()];
            sha3256Digest.BlockUpdate(publicKey, 0, publicKey.Length);
            sha3256Digest.DoFinal(sha3256Hash, 0);

            // step 2: ripemd160 hash of (1)
            var digest = new RipeMD160Digest();
            var ripemdHash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(sha3256Hash, 0, sha3256Hash.Length);
            digest.DoFinal(ripemdHash, 0);
            
            // step 3: add network identifier byte in front of (2)
            var decodedAddress = new byte[_constants["sizes"]["addressDecoded"]];
            decodedAddress[0] = (byte)networkType;
            
            RawArray.Copy(decodedAddress, ripemdHash, _constants["sizes"]["ripemd160"], 1);
            
            // step 4: concatenate (3) and the checksum of (3)
            var hash = new byte[_constants["sizes"]["ripemd160"] + 1];
            Array.Copy(decodedAddress, hash, _constants["sizes"]["ripemd160"] + 1);
            
            var resultHash = new byte[sha3256Digest.GetDigestSize()];
            sha3256Digest.BlockUpdate(hash, 0, hash.Length);
            sha3256Digest.DoFinal(resultHash, 0);
            
            RawArray.Copy(
                decodedAddress,
                resultHash,
                _constants["sizes"]["checksum"],
                _constants["sizes"]["ripemd160"] + 1
            );
            
            return decodedAddress;
        }
    }
}