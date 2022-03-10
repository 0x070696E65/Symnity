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

namespace Symbol.Builders {

    /* A 64-byte (512 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used.. */
    [Serializable]
    public struct Hash512Dto : ISerializer
    {
        /* A 64-byte (512 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used.. */
        private readonly byte[] hash512;

        /*
         * Constructor.
         *
         * @param hash512 A 64-byte (512 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used..
         */
        public Hash512Dto(byte[] hash512)
        {
            this.hash512 = hash512;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public Hash512Dto(BinaryReader stream)
        {
            try
            {
                this.hash512 = stream.ReadBytes(64);
            }
            catch
            {
                throw new Exception("Hash512Dto: ERROR");
            }
        }

        /*
         * Gets A 64-byte (512 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used..
         *
         * @return A 64-byte (512 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used..
         */
        public byte[] GetHash512()
        {
            return this.hash512;
        }

        /*
         * Gets the size of the object.
         *
         * @return Size in bytes.
         */
        public int GetSize()
        {
            return 64;
        }

        /*
         * Creates an instance of Hash512Dto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of Hash512Dto.
         */
        public static Hash512Dto LoadFromBinary(BinaryReader stream)
        {
            return new Hash512Dto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.hash512, 0, this.hash512.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

