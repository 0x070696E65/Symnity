using Symnity.Core.Format;

namespace Symnity.Model.Messages
{
    public class MessageFactory
    {
        /**
         * It creates a message from the byte array payload
         * @param payload the payload as byte array
         */
        public static Message CreateMessageFromBuffer(byte[] payload = null) {
            return CreateMessageFromHex(payload != null ? ConvertUtils.ToHex(payload) : null);
        }
        
        /**
         * It creates a message from the hex payload
         * @param payload the payload as hex
         */
        public static Message CreateMessageFromHex(string payload)  {
            if (payload == null) {
                return new RawMessage("");
            }
            var upperCasePayload = payload.ToUpper();
            /*
            if (
                upperCasePayload.Length == PersistentHarvestingDelegationMessage.HEX_PAYLOAD_SIZE &&
                upperCasePayload.startsWith(MessageMarker.PersistentDelegationUnlock)
            ) {
                return PersistentHarvestingDelegationMessage.createFromPayload(upperCasePayload);
            }*/
            var messageType = ConvertUtils.GetBytes(upperCasePayload)[0];
            return messageType switch
            {
                (int) MessageType.PlainMessage => PlainMessage.CreateFromPayload(upperCasePayload[2..]),
                (int) MessageType.EncryptedMessage => EncryptedMessage.CreateFromPayload(upperCasePayload[2..]),
                _ => new RawMessage(upperCasePayload)
            };
        }

        /**
         * It creates a message from the hex payload
         * @param payload the payload as hex
         */
        public static Message EmptyMessage()
        {
            return CreateMessageFromBuffer();   
        }
    }
}