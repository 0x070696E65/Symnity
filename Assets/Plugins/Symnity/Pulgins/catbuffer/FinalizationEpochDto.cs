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

    /* Index of a [finalization](/concepts/block.html#finalization) epoch.
The first epoch is number 1 and contains only the first block (the [Nemesis](/concepts/block.html#block-creation) block). Epoch duration (in blocks) is defined by the `votingSetGrouping` network property.. */
    [Serializable]
    public struct FinalizationEpochDto : ISerializer
    {
        /* Index of a [finalization](/concepts/block.html#finalization) epoch.
The first epoch is number 1 and contains only the first block (the [Nemesis](/concepts/block.html#block-creation) block). Epoch duration (in blocks) is defined by the `votingSetGrouping` network property.. */
        private readonly int finalizationEpoch;

        /*
         * Constructor.
         *
         * @param finalizationEpoch Index of a [finalization](/concepts/block.html#finalization) epoch.
The first epoch is number 1 and contains only the first block (the [Nemesis](/concepts/block.html#block-creation) block). Epoch duration (in blocks) is defined by the `votingSetGrouping` network property..
         */
        public FinalizationEpochDto(int finalizationEpoch)
        {
            this.finalizationEpoch = finalizationEpoch;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public FinalizationEpochDto(BinaryReader stream)
        {
            try
            {
                this.finalizationEpoch = stream.ReadInt32();
            }
            catch
            {
                throw new Exception("FinalizationEpochDto: ERROR");
            }
        }

        /*
         * Gets Index of a [finalization](/concepts/block.html#finalization) epoch.
The first epoch is number 1 and contains only the first block (the [Nemesis](/concepts/block.html#block-creation) block). Epoch duration (in blocks) is defined by the `votingSetGrouping` network property..
         *
         * @return Index of a [finalization](/concepts/block.html#finalization) epoch.
The first epoch is number 1 and contains only the first block (the [Nemesis](/concepts/block.html#block-creation) block). Epoch duration (in blocks) is defined by the `votingSetGrouping` network property..
         */
        public int GetFinalizationEpoch()
        {
            return this.finalizationEpoch;
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
         * Creates an instance of FinalizationEpochDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of FinalizationEpochDto.
         */
        public static FinalizationEpochDto LoadFromBinary(BinaryReader stream)
        {
            return new FinalizationEpochDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetFinalizationEpoch());
            var result = ms.ToArray();
            return result;
        }
    }
}

