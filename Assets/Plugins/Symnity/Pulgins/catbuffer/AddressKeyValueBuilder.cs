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
using System.Collections;
using System.Collections.Generic;

namespace Symbol.Builders {
    /*
    * Layout for mosaic address restriction key-value pair
    */
    [Serializable]
    public class AddressKeyValueBuilder: ISerializer {

        /* Key for value. */
        public MosaicRestrictionKeyDto key;
        /* Value associated by key. */
        public long value;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AddressKeyValueBuilder(BinaryReader stream)
        {
            try {
                key = MosaicRestrictionKeyDto.LoadFromBinary(stream);
                value = stream.ReadInt64();
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AddressKeyValueBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AddressKeyValueBuilder.
        */
        public static AddressKeyValueBuilder LoadFromBinary(BinaryReader stream) {
            return new AddressKeyValueBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param key Key for value.
        * @param value Value associated by key.
        */
        internal AddressKeyValueBuilder(MosaicRestrictionKeyDto key, long value)
        {
            GeneratorUtils.NotNull(key, "key is null");
            GeneratorUtils.NotNull(value, "value is null");
            this.key = key;
            this.value = value;
        }
        
        /*
        * Creates an instance of AddressKeyValueBuilder.
        *
        * @param key Key for value.
        * @param value Value associated by key.
        * @return Instance of AddressKeyValueBuilder.
        */
        public static  AddressKeyValueBuilder Create(MosaicRestrictionKeyDto key, long value) {
            return new AddressKeyValueBuilder(key, value);
        }

        /*
        * Gets key for value.
        *
        * @return Key for value.
        */
        public MosaicRestrictionKeyDto GetKey() {
            return key;
        }

        /*
        * Gets value associated by key.
        *
        * @return Value associated by key.
        */
        public long GetValue() {
            return value;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += key.GetSize();
            size += 8; // value
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var keyEntityBytes = (key).Serialize();
            bw.Write(keyEntityBytes, 0, keyEntityBytes.Length);
            bw.Write(GetValue());
            var result = ms.ToArray();
            return result;
        }
    }
}
