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

    /* Mosaic restriction key. */
    [Serializable]
    public struct MosaicRestrictionKeyDto : ISerializer
    {
        /* Mosaic restriction key. */
        private readonly long mosaicRestrictionKey;

        /*
         * Constructor.
         *
         * @param mosaicRestrictionKey Mosaic restriction key.
         */
        public MosaicRestrictionKeyDto(long mosaicRestrictionKey)
        {
            this.mosaicRestrictionKey = mosaicRestrictionKey;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public MosaicRestrictionKeyDto(BinaryReader stream)
        {
            try
            {
                this.mosaicRestrictionKey = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("MosaicRestrictionKeyDto: ERROR");
            }
        }

        /*
         * Gets Mosaic restriction key.
         *
         * @return Mosaic restriction key.
         */
        public long GetMosaicRestrictionKey()
        {
            return this.mosaicRestrictionKey;
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
         * Creates an instance of MosaicRestrictionKeyDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of MosaicRestrictionKeyDto.
         */
        public static MosaicRestrictionKeyDto LoadFromBinary(BinaryReader stream)
        {
            return new MosaicRestrictionKeyDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetMosaicRestrictionKey());
            var result = ms.ToArray();
            return result;
        }
    }
}

