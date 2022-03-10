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
* Enumeration of account key type flags
*/

namespace Symbol.Builders {

    [Serializable]
    public enum AccountKeyTypeFlagsDto {
        /* unset key. */
        UNSET = 0,
        /* linked account public key \note this can be either a remote or main account public key depending on context. */
        LINKED = 1,
        /* node public key on which remote is allowed to harvest. */
        NODE = 2,
        /* VRF public key. */
        VRF = 4,
    }
    
    public static class AccountKeyTypeFlagsDtoExtensions
    {
        /* Enum value. */
        private static byte value(this AccountKeyTypeFlagsDto self) {
            return (byte)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static AccountKeyTypeFlagsDto RawValueOf(this AccountKeyTypeFlagsDto self, byte value) {
            return (AccountKeyTypeFlagsDto)Enum.ToObject(typeof(AccountKeyTypeFlagsDto), value);
            throw new Exception(value + " was not a backing value for AccountKeyTypeFlagsDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this AccountKeyTypeFlagsDto type)
        {
            return 1;
        }

        /*
        * Creates an instance of AccountKeyTypeFlagsDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountKeyTypeFlagsDto.
        */
        public static AccountKeyTypeFlagsDto LoadFromBinary(this AccountKeyTypeFlagsDto self, BinaryReader stream) {
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
        public static byte[] Serialize(this AccountKeyTypeFlagsDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
