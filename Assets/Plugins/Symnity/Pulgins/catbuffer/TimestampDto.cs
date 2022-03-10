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

    /* Number of milliseconds elapsed since the creation of the [Nemesis](/concepts/block.html#block-creation) block.
The Nemesis block creation time can be found in the `epochAdjustment` field returned by the [/network/properties](/symbol-openapi/v1.0.1/#operation/getNetworkProperties) REST endpoint. This is the number of seconds elapsed since the [UNIX epoch](https://en.wikipedia.org/wiki/Unix_time) and it is always 1615853185 for Symbol's MAINNET.. */
    [Serializable]
    public struct TimestampDto : ISerializer
    {
        /* Number of milliseconds elapsed since the creation of the [Nemesis](/concepts/block.html#block-creation) block.
The Nemesis block creation time can be found in the `epochAdjustment` field returned by the [/network/properties](/symbol-openapi/v1.0.1/#operation/getNetworkProperties) REST endpoint. This is the number of seconds elapsed since the [UNIX epoch](https://en.wikipedia.org/wiki/Unix_time) and it is always 1615853185 for Symbol's MAINNET.. */
        private readonly long timestamp;

        /*
         * Constructor.
         *
         * @param timestamp Number of milliseconds elapsed since the creation of the [Nemesis](/concepts/block.html#block-creation) block.
The Nemesis block creation time can be found in the `epochAdjustment` field returned by the [/network/properties](/symbol-openapi/v1.0.1/#operation/getNetworkProperties) REST endpoint. This is the number of seconds elapsed since the [UNIX epoch](https://en.wikipedia.org/wiki/Unix_time) and it is always 1615853185 for Symbol's MAINNET..
         */
        public TimestampDto(long timestamp)
        {
            this.timestamp = timestamp;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public TimestampDto(BinaryReader stream)
        {
            try
            {
                this.timestamp = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("TimestampDto: ERROR");
            }
        }

        /*
         * Gets Number of milliseconds elapsed since the creation of the [Nemesis](/concepts/block.html#block-creation) block.
The Nemesis block creation time can be found in the `epochAdjustment` field returned by the [/network/properties](/symbol-openapi/v1.0.1/#operation/getNetworkProperties) REST endpoint. This is the number of seconds elapsed since the [UNIX epoch](https://en.wikipedia.org/wiki/Unix_time) and it is always 1615853185 for Symbol's MAINNET..
         *
         * @return Number of milliseconds elapsed since the creation of the [Nemesis](/concepts/block.html#block-creation) block.
The Nemesis block creation time can be found in the `epochAdjustment` field returned by the [/network/properties](/symbol-openapi/v1.0.1/#operation/getNetworkProperties) REST endpoint. This is the number of seconds elapsed since the [UNIX epoch](https://en.wikipedia.org/wiki/Unix_time) and it is always 1615853185 for Symbol's MAINNET..
         */
        public long GetTimestamp()
        {
            return this.timestamp;
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
         * Creates an instance of TimestampDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of TimestampDto.
         */
        public static TimestampDto LoadFromBinary(BinaryReader stream)
        {
            return new TimestampDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetTimestamp());
            var result = ms.ToArray();
            return result;
        }
    }
}

