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

    /* How hard it was to harvest this block.
The initial value is 1e14 and it will remain like this as long as blocks are generated every `blockGenerationTargetTime` seconds ([network property](/guides/network/configuring-network-properties.html)).
If blocks start taking more or less time than the configured value, the difficulty will be adjusted (in the range of 1e13 to 1e15) to try to hit the target time.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 8.1.. */
    [Serializable]
    public struct DifficultyDto : ISerializer
    {
        /* How hard it was to harvest this block.
The initial value is 1e14 and it will remain like this as long as blocks are generated every `blockGenerationTargetTime` seconds ([network property](/guides/network/configuring-network-properties.html)).
If blocks start taking more or less time than the configured value, the difficulty will be adjusted (in the range of 1e13 to 1e15) to try to hit the target time.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 8.1.. */
        private readonly long difficulty;

        /*
         * Constructor.
         *
         * @param difficulty How hard it was to harvest this block.
The initial value is 1e14 and it will remain like this as long as blocks are generated every `blockGenerationTargetTime` seconds ([network property](/guides/network/configuring-network-properties.html)).
If blocks start taking more or less time than the configured value, the difficulty will be adjusted (in the range of 1e13 to 1e15) to try to hit the target time.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 8.1..
         */
        public DifficultyDto(long difficulty)
        {
            this.difficulty = difficulty;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public DifficultyDto(BinaryReader stream)
        {
            try
            {
                this.difficulty = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("DifficultyDto: ERROR");
            }
        }

        /*
         * Gets How hard it was to harvest this block.
The initial value is 1e14 and it will remain like this as long as blocks are generated every `blockGenerationTargetTime` seconds ([network property](/guides/network/configuring-network-properties.html)).
If blocks start taking more or less time than the configured value, the difficulty will be adjusted (in the range of 1e13 to 1e15) to try to hit the target time.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 8.1..
         *
         * @return How hard it was to harvest this block.
The initial value is 1e14 and it will remain like this as long as blocks are generated every `blockGenerationTargetTime` seconds ([network property](/guides/network/configuring-network-properties.html)).
If blocks start taking more or less time than the configured value, the difficulty will be adjusted (in the range of 1e13 to 1e15) to try to hit the target time.
See the [Technical Reference](/symbol-technicalref/main.pdf) section 8.1..
         */
        public long GetDifficulty()
        {
            return this.difficulty;
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
         * Creates an instance of DifficultyDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of DifficultyDto.
         */
        public static DifficultyDto LoadFromBinary(BinaryReader stream)
        {
            return new DifficultyDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetDifficulty());
            var result = ms.ToArray();
            return result;
        }
    }
}

