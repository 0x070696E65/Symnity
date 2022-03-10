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
    * Header common to all serialized states
    */
    [Serializable]
    public class StateHeaderBuilder: ISerializer {

        /* Serialization version. */
        public short version;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal StateHeaderBuilder(BinaryReader stream)
        {
            try {
                version = stream.ReadInt16();
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of StateHeaderBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of StateHeaderBuilder.
        */
        public static StateHeaderBuilder LoadFromBinary(BinaryReader stream) {
            return new StateHeaderBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        */
        internal StateHeaderBuilder(short version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            this.version = version;
        }
        
        /*
        * Creates an instance of StateHeaderBuilder.
        *
        * @param version Serialization version.
        * @return Instance of StateHeaderBuilder.
        */
        public static  StateHeaderBuilder Create(short version) {
            return new StateHeaderBuilder(version);
        }

        /*
        * Gets serialization version.
        *
        * @return Serialization version.
        */
        public short GetVersion() {
            return version;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 2; // version
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
            bw.Write(GetVersion());
            var result = ms.ToArray();
            return result;
        }
    }
}
