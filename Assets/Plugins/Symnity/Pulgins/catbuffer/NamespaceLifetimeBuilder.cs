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
    * Binary layout for namespace lifetime
    */
    [Serializable]
    public class NamespaceLifetimeBuilder: ISerializer {

        /* Start height. */
        public HeightDto lifetimeStart;
        /* End height. */
        public HeightDto lifetimeEnd;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal NamespaceLifetimeBuilder(BinaryReader stream)
        {
            try {
                lifetimeStart = HeightDto.LoadFromBinary(stream);
                lifetimeEnd = HeightDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of NamespaceLifetimeBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NamespaceLifetimeBuilder.
        */
        public static NamespaceLifetimeBuilder LoadFromBinary(BinaryReader stream) {
            return new NamespaceLifetimeBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param lifetimeStart Start height.
        * @param lifetimeEnd End height.
        */
        internal NamespaceLifetimeBuilder(HeightDto lifetimeStart, HeightDto lifetimeEnd)
        {
            GeneratorUtils.NotNull(lifetimeStart, "lifetimeStart is null");
            GeneratorUtils.NotNull(lifetimeEnd, "lifetimeEnd is null");
            this.lifetimeStart = lifetimeStart;
            this.lifetimeEnd = lifetimeEnd;
        }
        
        /*
        * Creates an instance of NamespaceLifetimeBuilder.
        *
        * @param lifetimeStart Start height.
        * @param lifetimeEnd End height.
        * @return Instance of NamespaceLifetimeBuilder.
        */
        public static  NamespaceLifetimeBuilder Create(HeightDto lifetimeStart, HeightDto lifetimeEnd) {
            return new NamespaceLifetimeBuilder(lifetimeStart, lifetimeEnd);
        }

        /*
        * Gets start height.
        *
        * @return Start height.
        */
        public HeightDto GetLifetimeStart() {
            return lifetimeStart;
        }

        /*
        * Gets end height.
        *
        * @return End height.
        */
        public HeightDto GetLifetimeEnd() {
            return lifetimeEnd;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += lifetimeStart.GetSize();
            size += lifetimeEnd.GetSize();
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
            var lifetimeStartEntityBytes = (lifetimeStart).Serialize();
            bw.Write(lifetimeStartEntityBytes, 0, lifetimeStartEntityBytes.Length);
            var lifetimeEndEntityBytes = (lifetimeEnd).Serialize();
            bw.Write(lifetimeEndEntityBytes, 0, lifetimeEndEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
