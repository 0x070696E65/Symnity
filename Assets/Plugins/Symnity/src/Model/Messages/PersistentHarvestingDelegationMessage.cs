using System;
using Symnity.Core.Format;

namespace Symnity.Model.Messages
{
    /*
    [Serializable]
    public class PersistentHarvestingDelegationMessage : Message
    {
        public const short HexPayloadSize = 264;

        PersistentHarvestingDelegationMessage (string payload) 
        : base(MessageType.PersistentHarvestingDelegationMessage, payload.ToUpper());{
            if (!ConvertUtils.isHexString(payload)) {
                throw Error('Payload format is not valid hexadecimal string');
            }
            if (payload.length != PersistentHarvestingDelegationMessage.HEX_PAYLOAD_SIZE) {
                throw Error(
                    `Invalid persistent harvesting delegate payload size! Expected ${PersistentHarvestingDelegationMessage.HEX_PAYLOAD_SIZE} but got ${payload.length}`,
                );
            }
            if (payload.toUpperCase().indexOf(MessageMarker.PersistentDelegationUnlock) != 0) {
                throw Error(
                    `Invalid persistent harvesting delegate payload! It does not start with ${MessageMarker.PersistentDelegationUnlock}`,
                );
            }
        }
    }
    */
}