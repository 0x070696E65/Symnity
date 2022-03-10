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
* Enumeration of lock hash algorithms.
*/

namespace Symbol.Builders {

    [Serializable]
    public enum LockHashAlgorithmDto {
        /* Input is hashed using [SHA-3 256](https://en.wikipedia.org/wiki/SHA-3).. */
        SHA3_256 = 0,
        /* Input is hashed twice: first with [SHA-256](https://en.wikipedia.org/wiki/SHA-2) and then with [RIPEMD-160](https://en.wikipedia.org/wiki/RIPEMD) (bitcoin's OP_HASH160).. */
        HASH_160 = 1,
        /* Input is hashed twice with [SHA-256](https://en.wikipedia.org/wiki/SHA-2) (bitcoin's OP_HASH256).. */
        HASH_256 = 2,
    }
    
    public static class LockHashAlgorithmDtoExtensions
    {
        /* Enum value. */
        private static byte value(this LockHashAlgorithmDto self) {
            return (byte)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static LockHashAlgorithmDto RawValueOf(this LockHashAlgorithmDto self, byte value) {
            return (LockHashAlgorithmDto)Enum.ToObject(typeof(LockHashAlgorithmDto), value);
            throw new Exception(value + " was not a backing value for LockHashAlgorithmDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this LockHashAlgorithmDto type)
        {
            return 1;
        }

        /*
        * Creates an instance of LockHashAlgorithmDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of LockHashAlgorithmDto.
        */
        public static LockHashAlgorithmDto LoadFromBinary(this LockHashAlgorithmDto self, BinaryReader stream) {
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
        public static byte[] Serialize(this LockHashAlgorithmDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
