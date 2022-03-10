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

    /* An [address](/concepts/cryptography.html#address) identifies an account and is derived from its PublicKey.. */
    [Serializable]
    public struct AddressDto : ISerializer
    {
        /* An [address](/concepts/cryptography.html#address) identifies an account and is derived from its PublicKey.. */
        private readonly byte[] address;

        /*
         * Constructor.
         *
         * @param address An [address](/concepts/cryptography.html#address) identifies an account and is derived from its PublicKey..
         */
        public AddressDto(byte[] address)
        {
            this.address = address;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public AddressDto(BinaryReader stream)
        {
            try
            {
                this.address = stream.ReadBytes(24);
            }
            catch
            {
                throw new Exception("AddressDto: ERROR");
            }
        }

        /*
         * Gets An [address](/concepts/cryptography.html#address) identifies an account and is derived from its PublicKey..
         *
         * @return An [address](/concepts/cryptography.html#address) identifies an account and is derived from its PublicKey..
         */
        public byte[] GetAddress()
        {
            return this.address;
        }

        /*
         * Gets the size of the object.
         *
         * @return Size in bytes.
         */
        public int GetSize()
        {
            return 24;
        }

        /*
         * Creates an instance of AddressDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of AddressDto.
         */
        public static AddressDto LoadFromBinary(BinaryReader stream)
        {
            return new AddressDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.address, 0, this.address.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

