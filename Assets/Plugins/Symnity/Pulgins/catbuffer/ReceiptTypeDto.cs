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
* Enumeration of receipt types.
*/

namespace Symbol.Builders {

    [Serializable]
    public enum ReceiptTypeDto {
        /* Reserved.. */
        RESERVED = 0,
        /* Mosaic rental fee receipt.. */
        MOSAIC_RENTAL_FEE = 4685,
        /* Namespace rental fee receipt.. */
        NAMESPACE_RENTAL_FEE = 4942,
        /* Harvest fee receipt.. */
        HARVEST_FEE = 8515,
        /* Hash lock completed receipt.. */
        LOCK_HASH_COMPLETED = 8776,
        /* Hash lock expired receipt.. */
        LOCK_HASH_EXPIRED = 9032,
        /* Secret lock completed receipt.. */
        LOCK_SECRET_COMPLETED = 8786,
        /* Secret lock expired receipt.. */
        LOCK_SECRET_EXPIRED = 9042,
        /* Hash lock created receipt.. */
        LOCK_HASH_CREATED = 12616,
        /* Secret lock created receipt.. */
        LOCK_SECRET_CREATED = 12626,
        /* Mosaic expired receipt.. */
        MOSAIC_EXPIRED = 16717,
        /* Namespace expired receipt.. */
        NAMESPACE_EXPIRED = 16718,
        /* Namespace deleted receipt.. */
        NAMESPACE_DELETED = 16974,
        /* Inflation receipt.. */
        INFLATION = 20803,
        /* Transaction group receipt.. */
        TRANSACTION_GROUP = 57667,
        /* Address alias resolution receipt.. */
        ADDRESS_ALIAS_RESOLUTION = 61763,
        /* Mosaic alias resolution receipt.. */
        MOSAIC_ALIAS_RESOLUTION = 62019,
    }
    
    public static class ReceiptTypeDtoExtensions
    {
        /* Enum value. */
        private static short value(this ReceiptTypeDto self) {
            return (short)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static ReceiptTypeDto RawValueOf(this ReceiptTypeDto self, short value) {
            return (ReceiptTypeDto)Enum.ToObject(typeof(ReceiptTypeDto), value);
            throw new Exception(value + " was not a backing value for ReceiptTypeDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this ReceiptTypeDto type)
        {
            return 2;
        }

        /*
        * Creates an instance of ReceiptTypeDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of ReceiptTypeDto.
        */
        public static ReceiptTypeDto LoadFromBinary(this ReceiptTypeDto self, BinaryReader stream) {
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
        public static byte[] Serialize(this ReceiptTypeDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
