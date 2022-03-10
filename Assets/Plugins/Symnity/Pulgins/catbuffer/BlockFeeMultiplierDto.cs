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

    /* Multiplier applied to the size of a transaction to obtain its fee, in [absolute units](/concepts/mosaic.html#divisibility).
See the [fees documentation](/concepts/fees.html).. */
    [Serializable]
    public struct BlockFeeMultiplierDto : ISerializer
    {
        /* Multiplier applied to the size of a transaction to obtain its fee, in [absolute units](/concepts/mosaic.html#divisibility).
See the [fees documentation](/concepts/fees.html).. */
        private readonly int blockFeeMultiplier;

        /*
         * Constructor.
         *
         * @param blockFeeMultiplier Multiplier applied to the size of a transaction to obtain its fee, in [absolute units](/concepts/mosaic.html#divisibility).
See the [fees documentation](/concepts/fees.html)..
         */
        public BlockFeeMultiplierDto(int blockFeeMultiplier)
        {
            this.blockFeeMultiplier = blockFeeMultiplier;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public BlockFeeMultiplierDto(BinaryReader stream)
        {
            try
            {
                this.blockFeeMultiplier = stream.ReadInt32();
            }
            catch
            {
                throw new Exception("BlockFeeMultiplierDto: ERROR");
            }
        }

        /*
         * Gets Multiplier applied to the size of a transaction to obtain its fee, in [absolute units](/concepts/mosaic.html#divisibility).
See the [fees documentation](/concepts/fees.html)..
         *
         * @return Multiplier applied to the size of a transaction to obtain its fee, in [absolute units](/concepts/mosaic.html#divisibility).
See the [fees documentation](/concepts/fees.html)..
         */
        public int GetBlockFeeMultiplier()
        {
            return this.blockFeeMultiplier;
        }

        /*
         * Gets the size of the object.
         *
         * @return Size in bytes.
         */
        public int GetSize()
        {
            return 4;
        }

        /*
         * Creates an instance of BlockFeeMultiplierDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of BlockFeeMultiplierDto.
         */
        public static BlockFeeMultiplierDto LoadFromBinary(BinaryReader stream)
        {
            return new BlockFeeMultiplierDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetBlockFeeMultiplier());
            var result = ms.ToArray();
            return result;
        }
    }
}

