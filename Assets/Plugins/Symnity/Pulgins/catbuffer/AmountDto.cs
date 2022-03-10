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

    /* A quantity of mosaics in [absolute units](/concepts/mosaic.html#divisibility).
It can only be positive or zero. Negative quantities must be indicated by other means (See for example MosaicSupplyChangeTransaction and MosaicSupplyChangeAction).. */
    [Serializable]
    public struct AmountDto : ISerializer
    {
        /* A quantity of mosaics in [absolute units](/concepts/mosaic.html#divisibility).
It can only be positive or zero. Negative quantities must be indicated by other means (See for example MosaicSupplyChangeTransaction and MosaicSupplyChangeAction).. */
        private readonly long amount;

        /*
         * Constructor.
         *
         * @param amount A quantity of mosaics in [absolute units](/concepts/mosaic.html#divisibility).
It can only be positive or zero. Negative quantities must be indicated by other means (See for example MosaicSupplyChangeTransaction and MosaicSupplyChangeAction)..
         */
        public AmountDto(long amount)
        {
            this.amount = amount;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public AmountDto(BinaryReader stream)
        {
            try
            {
                this.amount = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("AmountDto: ERROR");
            }
        }

        /*
         * Gets A quantity of mosaics in [absolute units](/concepts/mosaic.html#divisibility).
It can only be positive or zero. Negative quantities must be indicated by other means (See for example MosaicSupplyChangeTransaction and MosaicSupplyChangeAction)..
         *
         * @return A quantity of mosaics in [absolute units](/concepts/mosaic.html#divisibility).
It can only be positive or zero. Negative quantities must be indicated by other means (See for example MosaicSupplyChangeTransaction and MosaicSupplyChangeAction)..
         */
        public long GetAmount()
        {
            return this.amount;
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
         * Creates an instance of AmountDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of AmountDto.
         */
        public static AmountDto LoadFromBinary(BinaryReader stream)
        {
            return new AmountDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetAmount());
            var result = ms.ToArray();
            return result;
        }
    }
}

