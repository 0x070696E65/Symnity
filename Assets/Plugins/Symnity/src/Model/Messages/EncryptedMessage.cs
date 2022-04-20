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
         * It creates a encrypted message from the payload hex wihtout the 01 prefix.
         *
         * The 01 prefix will be attached to the final payload.
         *
         * @internal
         * @param payload
         */
        public static EncryptedMessage CreateFromPayload(string payload) {
            return new EncryptedMessage(DecodeHex(payload));
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