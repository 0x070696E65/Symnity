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
    * Binary layout of a metadata entry value
    */
    [Serializable]
    public class MetadataValueBuilder: ISerializer {

        /* Data of the value. */
        public byte[] data;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MetadataValueBuilder(BinaryReader stream)
        {
            try {
                var size = stream.ReadInt16();
                data = GeneratorUtils.ReadBytes(stream, size);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MetadataValueBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MetadataValueBuilder.
        */
        public static MetadataValueBuilder LoadFromBinary(BinaryReader stream) {
            return new MetadataValueBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param data Data of the value.
        */
        internal MetadataValueBuilder(byte[] data)
        {
            GeneratorUtils.NotNull(data, "data is null");
            this.data = data;
        }
        
        /*
        * Creates an instance of MetadataValueBuilder.
        *
        * @param data Data of the value.
        * @return Instance of MetadataValueBuilder.
        */
        public static  MetadataValueBuilder Create(byte[] data) {
            return new MetadataValueBuilder(data);
        }

        /*
        * Gets data of the value.
        *
        * @return Data of the value.
        */
        public byte[] GetData() {
            return data;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 2; // size
            size += data.Length;
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
            bw.Write((short)GeneratorUtils.GetSize(GetData()));
            bw.Write(data, 0, data.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
