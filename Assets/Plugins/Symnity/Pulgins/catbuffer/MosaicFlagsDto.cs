/**
*** Copyright (c) 2016-2019, Jaguar0625, gimre, BloodyRookie, Tech Bureau, Corp.
*** Copyright (c) 2020-present, Jaguar0625, gimre, BloodyRookie.
*** All rights reserved.
***
*** This file is part of Catapult.
***
*** Catapult is free software: you can redistribute it and/or modify
*** it under the terms of the GNU Lesser General Public License as published by
*** the Free Software Foundation, either version 3 of the License, or
*** (at your option) any later version.
***
*** Catapult is distributed in the hope that it will be useful,
*** but WITHOUT ANY WARRANTY; without even the implied warranty of
*** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*** GNU Lesser General Public License for more details.
***
*** You should have received a copy of the GNU Lesser General Public License
*** along with Catapult. If not, see <http://www.gnu.org/licenses/>.
**/

using System;
using System.IO;

/*
* Enumeration of mosaic property flags.
*/

namespace Symbol.Builders {

    [Serializable]
    public enum MosaicFlagsDto {
        /* No flags present.. */
        NONE = 0,
        /* Mosaic supports supply changes through a MosaicSupplyChangeTransaction even when mosaic creator only owns a partial supply.
If the mosaic creator owns the totality of the supply, it can be changed even if this flag is not set.. */
        SUPPLY_MUTABLE = 1,
        /* Mosaic supports TransferTransaction between arbitrary accounts. When not set, this mosaic can only be transferred to or from the mosaic creator.. */
        TRANSFERABLE = 2,
        /* Mosaic supports custom restrictions configured by the mosaic creator.
See MosaicAddressRestrictionTransaction and MosaicGlobalRestrictionTransaction.. */
        RESTRICTABLE = 4,
        /* Mosaic supports revocation of tokens by the mosaic creator.. */
        REVOKABLE = 8,
    }
    
    public static class MosaicFlagsDtoExtensions
    {
        /* Enum value. */
        private static byte value(this MosaicFlagsDto self) {
            return (byte)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static MosaicFlagsDto RawValueOf(this MosaicFlagsDto self, byte value) {
            return (MosaicFlagsDto)Enum.ToObject(typeof(MosaicFlagsDto), value);
            throw new Exception(value + " was not a backing value for MosaicFlagsDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this MosaicFlagsDto type)
        {
            return 1;
        }

        /*
        * Creates an instance of MosaicFlagsDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicFlagsDto.
        */
        public static MosaicFlagsDto LoadFromBinary(this MosaicFlagsDto self, BinaryReader stream) {
            try {
                byte streamValue = stream.ReadByte();
                return RawValueOf(self, streamValue);
            } catch(Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public static byte[] Serialize(this MosaicFlagsDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
