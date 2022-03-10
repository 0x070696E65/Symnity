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
* Enumeration of account restriction flags.
*/

namespace Symbol.Builders {

    [Serializable]
    public enum AccountRestrictionFlagsDto {
        /* Restriction type is an address.. */
        ADDRESS = 1,
        /* Restriction type is a mosaic identifier.. */
        MOSAIC_ID = 2,
        /* Restriction type is a transaction type.. */
        TRANSACTION_TYPE = 4,
        /* Restriction is interpreted as outgoing.. */
        OUTGOING = 16384,
        /* Restriction is interpreted as blocking (instead of allowing) operation.. */
        BLOCK = 32768,
    }
    
    public static class AccountRestrictionFlagsDtoExtensions
    {
        /* Enum value. */
        private static short value(this AccountRestrictionFlagsDto self) {
            return (short)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static AccountRestrictionFlagsDto RawValueOf(this AccountRestrictionFlagsDto self, short value) {
            return (AccountRestrictionFlagsDto)Enum.ToObject(typeof(AccountRestrictionFlagsDto), value);
            throw new Exception(value + " was not a backing value for AccountRestrictionFlagsDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this AccountRestrictionFlagsDto type)
        {
            return 2;
        }

        /*
        * Creates an instance of AccountRestrictionFlagsDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountRestrictionFlagsDto.
        */
        public static AccountRestrictionFlagsDto LoadFromBinary(this AccountRestrictionFlagsDto self, BinaryReader stream) {
            try {
                short streamValue = stream.ReadInt16();
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
        public static byte[] Serialize(this AccountRestrictionFlagsDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
