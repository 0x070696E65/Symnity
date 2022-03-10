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

    /* A particular point in time inside a [finalization](/concepts/block.html#finalization) epoch.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 15.2.. */
    [Serializable]
    public struct FinalizationPointDto : ISerializer
    {
        /* A particular point in time inside a [finalization](/concepts/block.html#finalization) epoch.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 15.2.. */
        private readonly int finalizationPoint;

        /*
         * Constructor.
         *
         * @param finalizationPoint A particular point in time inside a [finalization](/concepts/block.html#finalization) epoch.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 15.2..
         */
        public FinalizationPointDto(int finalizationPoint)
        {
            this.finalizationPoint = finalizationPoint;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public FinalizationPointDto(BinaryReader stream)
        {
            try
            {
                this.finalizationPoint = stream.ReadInt32();
            }
            catch
            {
                throw new Exception("FinalizationPointDto: ERROR");
            }
        }

        /*
         * Gets A particular point in time inside a [finalization](/concepts/block.html#finalization) epoch.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 15.2..
         *
         * @return A particular point in time inside a [finalization](/concepts/block.html#finalization) epoch.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 15.2..
         */
        public int GetFinalizationPoint()
        {
            return this.finalizationPoint;
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
         * Creates an instance of FinalizationPointDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of FinalizationPointDto.
         */
        public static FinalizationPointDto LoadFromBinary(BinaryReader stream)
        {
            return new FinalizationPointDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetFinalizationPoint());
            var result = ms.ToArray();
            return result;
        }
    }
}

