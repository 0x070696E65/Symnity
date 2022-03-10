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

    /* A 64-byte (512 bit) array certifying that the signed data has not been modified.
Symbol currently uses [Ed25519](https://ed25519.cr.yp.to/) signatures.. */
    [Serializable]
    public struct SignatureDto : ISerializer
    {
        /* A 64-byte (512 bit) array certifying that the signed data has not been modified.
Symbol currently uses [Ed25519](https://ed25519.cr.yp.to/) signatures.. */
        private readonly byte[] signature;

        /*
         * Constructor.
         *
         * @param signature A 64-byte (512 bit) array certifying that the signed data has not been modified.
Symbol currently uses [Ed25519](https://ed25519.cr.yp.to/) signatures..
         */
        public SignatureDto(byte[] signature)
        {
            this.signature = signature;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public SignatureDto(BinaryReader stream)
        {
            try
            {
                this.signature = stream.ReadBytes(64);
            }
            catch
            {
                throw new Exception("SignatureDto: ERROR");
            }
        }

        /*
         * Gets A 64-byte (512 bit) array certifying that the signed data has not been modified.
Symbol currently uses [Ed25519](https://ed25519.cr.yp.to/) signatures..
         *
         * @return A 64-byte (512 bit) array certifying that the signed data has not been modified.
Symbol currently uses [Ed25519](https://ed25519.cr.yp.to/) signatures..
         */
        public byte[] GetSignature()
        {
            return this.signature;
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
         * Creates an instance of SignatureDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of SignatureDto.
         */
        public static SignatureDto LoadFromBinary(BinaryReader stream)
        {
            return new SignatureDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.signature, 0, this.signature.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

