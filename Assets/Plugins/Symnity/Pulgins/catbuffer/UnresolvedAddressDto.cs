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

    /* Either an Address or a NamespaceId.
The **least**-significant bit of the first byte is 0 for Addresses and 1 for NamespaceId's.. */
    [Serializable]
    public struct UnresolvedAddressDto : ISerializer
    {
        /* Either an Address or a NamespaceId.
The **least**-significant bit of the first byte is 0 for Addresses and 1 for NamespaceId's.. */
        private readonly byte[] unresolvedAddress;

        /*
         * Constructor.
         *
         * @param unresolvedAddress Either an Address or a NamespaceId.
The **least**-significant bit of the first byte is 0 for Addresses and 1 for NamespaceId's..
         */
        public UnresolvedAddressDto(byte[] unresolvedAddress)
        {
            this.unresolvedAddress = unresolvedAddress;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public UnresolvedAddressDto(BinaryReader stream)
        {
            try
            {
                this.unresolvedAddress = stream.ReadBytes(24);
            }
            catch
            {
                throw new Exception("UnresolvedAddressDto: ERROR");
            }
        }

        /*
         * Gets Either an Address or a NamespaceId.
The **least**-significant bit of the first byte is 0 for Addresses and 1 for NamespaceId's..
         *
         * @return Either an Address or a NamespaceId.
The **least**-significant bit of the first byte is 0 for Addresses and 1 for NamespaceId's..
         */
        public byte[] GetUnresolvedAddress()
        {
            return this.unresolvedAddress;
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
         * Creates an instance of UnresolvedAddressDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of UnresolvedAddressDto.
         */
        public static UnresolvedAddressDto LoadFromBinary(BinaryReader stream)
        {
            return new UnresolvedAddressDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.unresolvedAddress, 0, this.unresolvedAddress.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

