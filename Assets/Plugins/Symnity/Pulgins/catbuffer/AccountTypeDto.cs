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
* Enumeration of account types
*/

namespace Symbol.Builders {

    [Serializable]
    public enum AccountTypeDto {
        /* account is not linked to another account. */
        UNLINKED = 0,
        /* account is a balance-holding account that is linked to a remote harvester account. */
        MAIN = 1,
        /* account is a remote harvester account that is linked to a balance-holding account. */
        REMOTE = 2,
        /* account is a remote harvester eligible account that is unlinked \note this allows an account that has previously been used as remote to be reused as a remote. */
        REMOTE_UNLINKED = 3,
    }
    
    public static class AccountTypeDtoExtensions
    {
        /* Enum value. */
        private static byte value(this AccountTypeDto self) {
            return (byte)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static AccountTypeDto RawValueOf(this AccountTypeDto self, byte value) {
            return (AccountTypeDto)Enum.ToObject(typeof(AccountTypeDto), value);
            throw new Exception(value + " was not a backing value for AccountTypeDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this AccountTypeDto type)
        {
            return 1;
        }

        /*
        * Creates an instance of AccountTypeDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountTypeDto.
        */
        public static AccountTypeDto LoadFromBinary(this AccountTypeDto self, BinaryReader stream) {
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
        public static byte[] Serialize(this AccountTypeDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
