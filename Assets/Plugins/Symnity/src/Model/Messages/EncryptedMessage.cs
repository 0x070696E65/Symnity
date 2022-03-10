using Symnity.Core.Crypto;
using Symnity.Model.Accounts;

namespace Symnity.Model.Messages
{
    public class EncryptedMessage : Message
    {
        public PublicAccount RecipientPublicAccount;

        public EncryptedMessage(string payload, PublicAccount recipientPublicAccount = null)
            : base(MessageType.EncryptedMessage, payload)
        {
            RecipientPublicAccount = recipientPublicAccount;
        }

        /**
         *
         * @param message - Plain message to be encrypted
         * @param recipientPublicAccount - Recipient public account
         * @param privateKey - Sender private key
         * @return {EncryptedMessage}
         */
        public static EncryptedMessage Create(string message, PublicAccount recipientPublicAccount, KeyPair senderKeyPair)
        {
            return new EncryptedMessage(
                Crypto.Encode(senderKeyPair, recipientPublicAccount.PublicKey, message),
                recipientPublicAccount
            );
        }
        
        /**
         *
         * @param encryptMessage - Encrypted message to be decrypted
         * @param privateKey - Recipient private key
         * @param recipientPublicAccount - Sender public account
         * @return {PlainMessage}
         */
        public static PlainMessage Decrypt(EncryptedMessage encryptMessage, KeyPair privateKeyPair, PublicAccount recipientPublicAccount) {
            return new PlainMessage(DecodeHex(Crypto.Decode(privateKeyPair, recipientPublicAccount.PublicKey, encryptMessage.Payload)));
        }
    }
}