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

    /* A 32-byte (256 bit) integer derived from a private key.
It serves as the public identifier of the [key pair](/concepts/cryptography.html#key-pair) and can be disseminated widely. It is used to prove that an entity was signed with the paired private key.. */
    [Serializable]
    public struct PublicKeyDto : ISerializer
    {
        /* A 32-byte (256 bit) integer derived from a private key.
It serves as the public identifier of the [key pair](/concepts/cryptography.html#key-pair) and can be disseminated widely. It is used to prove that an entity was signed with the paired private key.. */
        private readonly byte[] publicKey;

        /*
         * Constructor.
         *
         * @param publicKey A 32-byte (256 bit) integer derived from a private key.
It serves as the public identifier of the [key pair](/concepts/cryptography.html#key-pair) and can be disseminated widely. It is used to prove that an entity was signed with the paired private key..
         */
        public PublicKeyDto(byte[] publicKey)
        {
            this.publicKey = publicKey;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public PublicKeyDto(BinaryReader stream)
        {
            try
            {
                this.publicKey = stream.ReadBytes(32);
            }
            catch
            {
                throw new Exception("PublicKeyDto: ERROR");
            }
        }

        /*
         * Gets A 32-byte (256 bit) integer derived from a private key.
It serves as the public identifier of the [key pair](/concepts/cryptography.html#key-pair) and can be disseminated widely. It is used to prove that an entity was signed with the paired private key..
         *
         * @return A 32-byte (256 bit) integer derived from a private key.
It serves as the public identifier of the [key pair](/concepts/cryptography.html#key-pair) and can be disseminated widely. It is used to prove that an entity was signed with the paired private key..
         */
        public byte[] GetPublicKey()
        {
            return this.publicKey;
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
         * Creates an instance of PublicKeyDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of PublicKeyDto.
         */
        public static PublicKeyDto LoadFromBinary(BinaryReader stream)
        {
            return new PublicKeyDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.publicKey, 0, this.publicKey.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

