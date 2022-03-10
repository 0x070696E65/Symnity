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

    /* A 32-byte (256 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used.. */
    [Serializable]
    public struct Hash256Dto : ISerializer
    {
        /* A 32-byte (256 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used.. */
        private readonly byte[] hash256;

        /*
         * Constructor.
         *
         * @param hash256 A 32-byte (256 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used..
         */
        public Hash256Dto(byte[] hash256)
        {
            this.hash256 = hash256;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public Hash256Dto(BinaryReader stream)
        {
            try
            {
                this.hash256 = stream.ReadBytes(32);
            }
            catch
            {
                throw new Exception("Hash256Dto: ERROR");
            }
        }

        /*
         * Gets A 32-byte (256 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used..
         *
         * @return A 32-byte (256 bit) hash.
The exact algorithm is unspecified as it can change depending on where it is used..
         */
        public byte[] GetHash256()
        {
            return this.hash256;
        }

        /*
         * Gets the size of the object.
         *
         * @return Size in bytes.
         */
        public int GetSize()
        {
            return 32;
        }

        /*
         * Creates an instance of Hash256Dto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of Hash256Dto.
         */
        public static Hash256Dto LoadFromBinary(BinaryReader stream)
        {
            return new Hash256Dto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.hash256, 0, this.hash256.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

