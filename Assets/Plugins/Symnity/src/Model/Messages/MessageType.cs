using System;

namespace Symnity.Model.Messages
{
    /**
     * The Message type. Supported supply types are:
     * -1: RawMessage (no type appended)
     * 0: PlainMessage
     * 1: EncryptedMessage.
     * 254: Persistent harvesting delegation.
     */
    [Serializable]
    public enum MessageType
    {
        RawMessage = -1,
        PlainMessage = 0x00,
        EncryptedMessage = 0x01,
        PersistentHarvestingDelegationMessage = 0xfe,
    }
}