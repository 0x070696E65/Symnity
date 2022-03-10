using System;
using Symnity.Core.Format;

namespace Symnity.Model.Messages
{
    /**
     * The a raw message that doesn't assume any prefix.
     */
    [Serializable]
    public class RawMessage : Message
    {
        /**
         * Create plain message object.
         * @returns PlainMessage
         */
        public static RawMessage Create(byte[] payload) {
            return new RawMessage(ConvertUtils.ToHex(payload));
        }
        /**
         * @internal
         * @param payload
         */
        internal RawMessage(string payload)
            : base(MessageType.RawMessage, payload)
        {
        }
    }
}