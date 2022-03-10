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

    /* Either a MosaicId or a NamespaceId.
The **most**-significant bit of the first byte is 0 for MosaicId's and 1 for NamespaceId's.. */
    [Serializable]
    public struct UnresolvedMosaicIdDto : ISerializer
    {
        /* Either a MosaicId or a NamespaceId.
The **most**-significant bit of the first byte is 0 for MosaicId's and 1 for NamespaceId's.. */
        private readonly long unresolvedMosaicId;

        /*
         * Constructor.
         *
         * @param unresolvedMosaicId Either a MosaicId or a NamespaceId.
The **most**-significant bit of the first byte is 0 for MosaicId's and 1 for NamespaceId's..
         */
        public UnresolvedMosaicIdDto(long unresolvedMosaicId)
        {
            this.unresolvedMosaicId = unresolvedMosaicId;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public UnresolvedMosaicIdDto(BinaryReader stream)
        {
            try
            {
                this.unresolvedMosaicId = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("UnresolvedMosaicIdDto: ERROR");
            }
        }

        /*
         * Gets Either a MosaicId or a NamespaceId.
The **most**-significant bit of the first byte is 0 for MosaicId's and 1 for NamespaceId's..
         *
         * @return Either a MosaicId or a NamespaceId.
The **most**-significant bit of the first byte is 0 for MosaicId's and 1 for NamespaceId's..
         */
        public long GetUnresolvedMosaicId()
        {
            return this.unresolvedMosaicId;
        }

        /*
         * Gets the size of the object.
         *
         * @return Size in bytes.
         */
        public int GetSize()
        {
            return 8;
        }

        /*
         * Creates an instance of UnresolvedMosaicIdDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of UnresolvedMosaicIdDto.
         */
        public static UnresolvedMosaicIdDto LoadFromBinary(BinaryReader stream)
        {
            return new UnresolvedMosaicIdDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetUnresolvedMosaicId());
            var result = ms.ToArray();
            return result;
        }
    }
}

