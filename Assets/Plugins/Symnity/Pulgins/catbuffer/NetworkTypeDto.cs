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
* Enumeration of network types.
*/

namespace Symbol.Builders {

    [Serializable]
    public enum NetworkTypeDto {
        /* Public network.. */
        MAINNET = 104,
        /* Public test network.. */
        TESTNET = 152,
    }
    
    public static class NetworkTypeDtoExtensions
    {
        /* Enum value. */
        private static byte value(this NetworkTypeDto self) {
            return (byte)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static NetworkTypeDto RawValueOf(this NetworkTypeDto self, byte value) {
            return (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), value);
            throw new Exception(value + " was not a backing value for NetworkTypeDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this NetworkTypeDto type)
        {
            return 1;
        }

        /*
        * Creates an instance of NetworkTypeDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NetworkTypeDto.
        */
        public static NetworkTypeDto LoadFromBinary(this NetworkTypeDto self, BinaryReader stream) {
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
        public static byte[] Serialize(this NetworkTypeDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
