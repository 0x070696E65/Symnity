using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Org.BouncyCastle.Math.EC.Rfc8032;
using Org.BouncyCastle.Utilities;
using Symnity.Core.Format;

namespace Symnity.Core.Crypto
{
    [Serializable]
    public class KeyPair
    {
        public byte[] privateKey;
        public byte[] publicKey;

        private KeyPair(byte[] privateKey, byte[] publicKey)
        {
            this.privateKey = privateKey;
            this.publicKey = publicKey;
        }
        /**
         * Creates a key pair from a private key string.
         * @param {string} privateKeyString A hex encoded private key string.
         * @returns {module:crypto/keyPair~KeyPair} The key pair.
         */
        
        
        public static KeyPair CreateKeyPairFromPrivateKeyString(string privateKeyString)
        {
            var privateKey = ConvertUtils.GetBytes(privateKeyString);
            if (CryptoUtilities.Key_Size != privateKey.Length) {
                throw new Exception("private key has unexpected size: " + privateKey.Length);
            }
            /*
            Console.WriteLine("aaa");
            var privateKeyRebuild = new Ed25519PrivateKeyParameters(privateKey, 0);
            var data = new byte[Ed25519PrivateKeyParameters.KeySize];
            Array.Copy((Array) privateKey, 0, (Array) data, 0, Ed25519PrivateKeyParameters.KeySize);
            var numArray = new byte[Ed25519.PublicKeySize];
            Console.WriteLine("bbb");
            var digest =  (IDigest) new Sha512Digest();
            var numArray1 = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, Ed25519.SecretKeySize);
            digest.DoFinal(numArray1, 0);
            var numArray2 = new byte[32];
            
            Ed25519.GeneratePublicKey(data, 0, numArray, 0);
            Console.WriteLine("bcbc");
            //var publicKeyRebuild = new Ed25519PublicKeyParameters(numArray, 0);
            //var publicKeyRebuild = privateKeyRebuild.GeneratePublicKey();
            var pubData = new byte[Ed25519PublicKeyParameters.KeySize];
            Array.Copy((Array) numArray, 0, (Array) pubData, 0, Ed25519PublicKeyParameters.KeySize);
            
            Console.WriteLine("ccc");
            var publicKey = Arrays.Clone(pubData);
            //var publicKey = publicKeyRebuild.GetEncoded();
            Console.WriteLine("ddd");
            return new KeyPair(privateKey, publicKey);*/
            
            var privateKeyRebuild = new Ed25519PrivateKeyParameters(privateKey, 0);
            var publicKeyRebuild = privateKeyRebuild.GeneratePublicKey();
            var publicKey = publicKeyRebuild.GetEncoded();
            return new KeyPair(privateKey, publicKey);
        }
        
        /**
         * Signs a data buffer with a key pair.
         * @param {module:crypto/keyPair~KeyPair} keyPair The key pair to use for signing.
         * @param {Uint8Array} data The data to sign.
         * @returns {Uint8Array} The signature.
         */
        public static byte[] Sign(KeyPair keyPair, byte[] data) {
            // create the signature
            var privateKey = new Ed25519PrivateKeyParameters(keyPair.privateKey, 0);
            var signer = new Ed25519Signer();
            signer.Init(true, privateKey);
            signer.BlockUpdate(data, 0, data.Length);
            var signature = signer.GenerateSignature();
            return signature;
        }
    }
}