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

    /* Namespace id. */
    [Serializable]
    public struct NamespaceIdDto : ISerializer
    {
        /* Namespace id. */
        private readonly long namespaceId;

        /*
         * Constructor.
         *
         * @param namespaceId Namespace id.
         */
        public NamespaceIdDto(long namespaceId)
        {
            this.namespaceId = namespaceId;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public NamespaceIdDto(BinaryReader stream)
        {
            try
            {
                this.namespaceId = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("NamespaceIdDto: ERROR");
            }
        }

        /*
         * Gets Namespace id.
         *
         * @return Namespace id.
         */
        public long GetNamespaceId()
        {
            return this.namespaceId;
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
         * Creates an instance of NamespaceIdDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of NamespaceIdDto.
         */
        public static NamespaceIdDto LoadFromBinary(BinaryReader stream)
        {
            return new NamespaceIdDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetNamespaceId());
            var result = ms.ToArray();
            return result;
        }
    }
}

