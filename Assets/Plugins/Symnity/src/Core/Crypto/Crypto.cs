using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Symnity.Core.Format;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Debug = UnityEngine.Debug;

namespace Symnity.Core.Crypto
{
    [Serializable]
    public class Crypto
    {
        /**
         * Generate random bytes by length
         * @param {byte} length - The length of the random bytes
         *
         * @return {byte[]}
         */
        public static byte[] RandomBytes(byte length)
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var randomBytes = new byte[length];
            rngCsp.GetBytes(randomBytes);
            return randomBytes;
        }

        public static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            var plainBytes = ConvertUtils.GetBytes(plainText);
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters =
                new AeadParameters(new KeyParameter(key), 128, iv);

            cipher.Init(true, parameters);

            var encryptedBytes = new byte[cipher.GetOutputSize(plainBytes.Length)];
            var retLen = cipher.ProcessBytes
                (plainBytes, 0, plainBytes.Length, encryptedBytes, 0);
            cipher.DoFinal(encryptedBytes, retLen);
            using (var combinedStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(combinedStream))
                {
                    binaryWriter.Write(iv);
                    binaryWriter.Write(encryptedBytes);
                }
                
                var combinedStreamArray = combinedStream.ToArray();
                var result = new byte[combinedStreamArray.Length];
                Array.Copy(combinedStreamArray, combinedStreamArray.Length - 16, result, 0,  16);
                Array.Copy(combinedStreamArray, 0, result, 16,  combinedStreamArray.Length - 16);
                return result;
            }
        }
        
        public static byte[] Decrypt(byte[] encryptedMessage, byte[] key)
        {
            var newArray = new byte[encryptedMessage.Length];
            Array.Copy(encryptedMessage, 16, newArray, 0,  12);
            Array.Copy(encryptedMessage, 28, newArray, 12,  encryptedMessage.Length - 28);
            Array.Copy(encryptedMessage, 0, newArray, encryptedMessage.Length - 16,  16);
            using var cipherStream = new MemoryStream(newArray);
            using var cipherReader = new BinaryReader(cipherStream);

            //Grab Nonce
            var iv = cipherReader.ReadBytes(12);
             
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), 128, iv);
            cipher.Init(false, parameters);

            //Decrypt Cipher Text
            var cipherText = cipherReader.ReadBytes(encryptedMessage.Length - iv.Length);
            var plainText = new byte[cipher.GetOutputSize(cipherText.Length)];  

            try
            {
                var len = cipher.ProcessBytes(cipherText, 0, cipherText.Length, plainText, 0);
                cipher.DoFinal(plainText, len);
            }
            catch (InvalidCipherTextException)
            {
                //Return null if it doesn't authenticate
                return null;
            }

            return plainText;
        }

        /***
         * Encode a message, separated from encode() to help testing
         *
         * @param {string} senderPriv - A sender private key
         * @param {string} recipientPub - A recipient public key
         * @param {string} msg - A text message
         * @param {Uint8Array} iv - An initialization vector
         * @param {Uint8Array} salt - A salt
         * @return {string} - The encoded message
         */
        private static string _Encode(KeyPair senderKeyPair, string recipientPub, string msg, byte[] iv)
        {
            if (senderKeyPair == null || recipientPub == null || msg == null || iv == null)
            {
                throw new Exception("Missing argument !");
            }

            var encKey =
                Utilities.CatapultCrypto.DeriveSharedKey(senderKeyPair.privateKey, ConvertUtils.GetBytes(recipientPub));
            var cipher = Encrypt(msg, encKey, iv);
            return ConvertUtils.ToHex(cipher);
        }

        /**
         * Encode a message using AES-GCM algorithm
         *
         * @param {string} senderPriv - A sender private key
         * @param {string} recipientPub - A recipient public key
         * @param {string} msg - A text message
         * @param {boolean} isHexString - Is payload string a hexadecimal string (default = false)
         * @return {string} - The encoded message
         */
        public static string Encode(KeyPair senderKeyPair, string recipientPub, string msg, bool isHexString = false)
        {
            // Errors
            if (senderKeyPair == null || recipientPub == null || msg == null)
            {
                throw new Exception("Missing argument !");
            }
            // Processing
            var iv = RandomBytes(12);
            var encoded = _Encode(senderKeyPair, recipientPub, isHexString ? msg : ConvertUtils.Utf8ToHex(msg), iv);
            // Result
            return encoded;
        }
        
        /**
         * Decode an encrypted message payload
         *
         * @param {string} recipientPrivate - A recipient private key
         * @param {string} senderPublic - A sender public key
         * @param {Uint8Array} payload - An encrypted message payload in bytes
         * @param {Uint8Array} tagAndIv - 16-bytes AES auth tag and 12-byte AES initialization vector
         * @return {string} - The decoded payload as hex
         */
        public static string _Decode(KeyPair privateKeyPair, string senderPublic, byte[] payload) {
            // Error
            if (privateKeyPair == null || senderPublic == null || payload == null) {
                throw new Exception("Missing argument !");
            }
            // Processing
            var encKey = Utilities.CatapultCrypto.DeriveSharedKey(privateKeyPair.privateKey, ConvertUtils.GetBytes(senderPublic));
            var cipher = Decrypt(payload, encKey);
            // Result
            return ConvertUtils.ToHex(cipher);
        }

        /**
         * Decode an encrypted (AES-GCM algorithm) message payload
         *
         * @param {string} recipientPrivate - A recipient private key
         * @param {string} senderPublic - A sender public key
         * @param {string} payload - An encrypted message payload
         * @return {string} - The decoded payload as hex
         */
        public static string Decode(KeyPair privateKeyPair, string senderPublic, string payload) {
            // Error
            if (privateKeyPair == null || senderPublic == null  || payload == null ) {
                throw new Exception("Missing argument !");
            }
            try
            {
                var encKey = Utilities.CatapultCrypto.DeriveSharedKey(privateKeyPair.privateKey, ConvertUtils.GetBytes(senderPublic));
                var cipher = Decrypt(ConvertUtils.GetBytes(payload), encKey);
                // Result
                return ConvertUtils.ToHex(cipher);
            } catch {
                // To return empty string rather than error throwing if authentication failed
                return "";
            }
        }
    }
}