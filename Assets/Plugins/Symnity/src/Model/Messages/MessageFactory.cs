using Symnity.Core.Format;

namespace Symnity.Model.Messages
{
    public class MessageFactory
    {
        /**
         * It creates a message from the byte array payload
         * @param payload the payload as byte array
         */
        /*public static Message CreateMessageFromBuffer(byte[] payload = null) {
            return CreateMessageFromHex(payload != null ? ConvertUtils.ToHex(payload) : null);
        }*/
        
        /**
         * It creates a message from the hex payload
         * @param payload the payload as hex
         */
        /*public static Message CreateMessageFromHex(string payload)  {
            if (payload == null) {
                return new RawMessage("");
            }
            var upperCasePayload = payload.ToUpper();
            if (
                upperCasePayload.Length == PersistentHarvestingDelegationMessage.HEX_PAYLOAD_SIZE &&
                upperCasePayload.startsWith(MessageMarker.PersistentDelegationUnlock)
            ) {
                return PersistentHarvestingDelegationMessage.createFromPayload(upperCasePayload);
            }
            var messageType = ConvertUtils.GetBytes(upperCasePayload)[0];
            switch (messageType) {
                case (int)MessageType.PlainMessage:
                    return PlainMessage.createFromPayload(upperCasePayload.substring(2));
                case (int)MessageType.EncryptedMessage:
                    return EncryptedMessage.createFromPayload(upperCasePayload.substring(2));
            }
            return new RawMessage(upperCasePayload);
        }*/
        /**
         * It creates a message from the hex payload
         * @param payload the payload as hex
         */
        //public Message EmptyMessage = MessageFactory.CreateMessageFromBuffer();
    }
}