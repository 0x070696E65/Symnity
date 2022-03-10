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
    * Binary layout for a global restriction key-value set
    */
    [Serializable]
    public class GlobalKeyValueSetBuilder: ISerializer {

        /* Key value array. */
        public List<GlobalKeyValueBuilder> keys;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal GlobalKeyValueSetBuilder(BinaryReader stream)
        {
            try {
                var keyValueCount = stream.ReadByte();
                keys = GeneratorUtils.LoadFromBinaryArray(GlobalKeyValueBuilder.LoadFromBinary, stream, keyValueCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of GlobalKeyValueSetBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of GlobalKeyValueSetBuilder.
        */
        public static GlobalKeyValueSetBuilder LoadFromBinary(BinaryReader stream) {
            return new GlobalKeyValueSetBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param keys Key value array.
        */
        internal GlobalKeyValueSetBuilder(List<GlobalKeyValueBuilder> keys)
        {
            GeneratorUtils.NotNull(keys, "keys is null");
            this.keys = keys;
        }
        
        /*
        * Creates an instance of GlobalKeyValueSetBuilder.
        *
        * @param keys Key value array.
        * @return Instance of GlobalKeyValueSetBuilder.
        */
        public static  GlobalKeyValueSetBuilder Create(List<GlobalKeyValueBuilder> keys) {
            return new GlobalKeyValueSetBuilder(keys);
        }

        /*
        * Gets key value array.
        *
        * @return Key value array.
        */
        public List<GlobalKeyValueBuilder> GetKeys() {
            return keys;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 1; // keyValueCount
            size +=  GeneratorUtils.GetSumSize(keys, 0);
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
            bw.Write((byte)GeneratorUtils.GetSize(GetKeys()));
            keys.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            var result = ms.ToArray();
            return result;
        }
    }
}
