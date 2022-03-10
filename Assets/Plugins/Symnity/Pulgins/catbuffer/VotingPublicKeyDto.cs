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

    /* A PublicKey used for voting during the [finalization process](/concepts/block.html#finalization).. */
    [Serializable]
    public struct VotingPublicKeyDto : ISerializer
    {
        /* A PublicKey used for voting during the [finalization process](/concepts/block.html#finalization).. */
        private readonly byte[] votingPublicKey;

        /*
         * Constructor.
         *
         * @param votingPublicKey A PublicKey used for voting during the [finalization process](/concepts/block.html#finalization)..
         */
        public VotingPublicKeyDto(byte[] votingPublicKey)
        {
            this.votingPublicKey = votingPublicKey;
        }

        /*
         * Constructor - Creates an object from stream.
         *
         * @param stream Byte stream to use to serialize.
         */
        public VotingPublicKeyDto(BinaryReader stream)
        {
            try
            {
                this.votingPublicKey = stream.ReadBytes(32);
            }
            catch
            {
                throw new Exception("VotingPublicKeyDto: ERROR");
            }
        }

        /*
         * Gets A PublicKey used for voting during the [finalization process](/concepts/block.html#finalization)..
         *
         * @return A PublicKey used for voting during the [finalization process](/concepts/block.html#finalization)..
         */
        public byte[] GetVotingPublicKey()
        {
            return this.votingPublicKey;
        }

        /*
         * Gets the size of the object.
         *
         * @return Size in bytes.
         */
        public int GetSize()
        {
            return 32;
        }

        /*
         * Creates an instance of VotingPublicKeyDto from a stream.
         *
         * @param stream Byte stream to use to serialize the object.
         * @return Instance of VotingPublicKeyDto.
         */
        public static VotingPublicKeyDto LoadFromBinary(BinaryReader stream)
        {
            return new VotingPublicKeyDto(stream);
        }

        /*
         * Serializes an object to bytes.
         *
         * @return Serialized bytes.
         */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(this.votingPublicKey, 0, this.votingPublicKey.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}

