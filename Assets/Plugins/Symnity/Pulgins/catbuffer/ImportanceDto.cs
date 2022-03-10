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

    /* [Importance score](/concepts/consensus-algorithm.html#importance-score) for an account.
See also ImportanceHeight and ImportanceSnapshot.. */
    [Serializable]
    public struct ImportanceDto : ISerializer
    {
        /* [Importance score](/concepts/consensus-algorithm.html#importance-score) for an account.
See also ImportanceHeight and ImportanceSnapshot.. */
        private readonly long importance;

        /*
         * Constructor.
         *
         * @param importance [Importance score](/concepts/consensus-algorithm.html#importance-score) for an account.
See also ImportanceHeight and ImportanceSnapshot..
         */
        public ImportanceDto(long importance)
        {
            this.importance = importance;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public ImportanceDto(BinaryReader stream)
        {
            try
            {
                this.importance = stream.ReadInt64();
            }
            catch
            {
                throw new Exception("ImportanceDto: ERROR");
            }
        }

        /*
         * Gets [Importance score](/concepts/consensus-algorithm.html#importance-score) for an account.
See also ImportanceHeight and ImportanceSnapshot..
         *
         * @return [Importance score](/concepts/consensus-algorithm.html#importance-score) for an account.
See also ImportanceHeight and ImportanceSnapshot..
         */
        public long GetImportance()
        {
            return this.importance;
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
         * Creates an instance of ImportanceDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of ImportanceDto.
         */
        public static ImportanceDto LoadFromBinary(BinaryReader stream)
        {
            return new ImportanceDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.GetImportance());
            var result = ms.ToArray();
            return result;
        }
    }
}

