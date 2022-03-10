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
* Enumeration of mosaic restriction types.
*/

namespace Symbol.Builders {

    [Serializable]
    public enum MosaicRestrictionTypeDto {
        /* Uninitialized value indicating no restriction.. */
        NONE = 0,
        /* Allow if equal.. */
        EQ = 1,
        /* Allow if not equal.. */
        NE = 2,
        /* Allow if less than.. */
        LT = 3,
        /* Allow if less than or equal.. */
        LE = 4,
        /* Allow if greater than.. */
        GT = 5,
        /* Allow if greater than or equal.. */
        GE = 6,
    }
    
    public static class MosaicRestrictionTypeDtoExtensions
    {
        /* Enum value. */
        private static byte value(this MosaicRestrictionTypeDto self) {
            return (byte)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static MosaicRestrictionTypeDto RawValueOf(this MosaicRestrictionTypeDto self, byte value) {
            return (MosaicRestrictionTypeDto)Enum.ToObject(typeof(MosaicRestrictionTypeDto), value);
            throw new Exception(value + " was not a backing value for MosaicRestrictionTypeDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this MosaicRestrictionTypeDto type)
        {
            return 1;
        }

        /*
        * Creates an instance of MosaicRestrictionTypeDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicRestrictionTypeDto.
        */
        public static MosaicRestrictionTypeDto LoadFromBinary(this MosaicRestrictionTypeDto self, BinaryReader stream) {
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
        public static byte[] Serialize(this MosaicRestrictionTypeDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
