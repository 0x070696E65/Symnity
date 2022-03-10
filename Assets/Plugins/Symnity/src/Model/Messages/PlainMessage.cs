using System;

namespace Symnity.Model.Messages
{
    [Serializable]
    public class PlainMessage : Message
    {
        /**
         * Create plain message object.
         * @returns PlainMessage
         */
        public static PlainMessage Create(string  message) {
            return new PlainMessage(message);
        }
        
        /**
         *
         * It creates the Plain message from a payload hex without the 00 prefix.
         *
         * The 00 prefix will be attached to the final payload.
         *
         * @internal
         */
        public static PlainMessage CreateFromPayload(string payload) {
            return new PlainMessage(DecodeHex(payload));
        }
        
        /**
         * @internal
         * @param payload
         */
        public PlainMessage(string payload) 
        : base(MessageType.PlainMessage, payload)
        {
        }
    }
}