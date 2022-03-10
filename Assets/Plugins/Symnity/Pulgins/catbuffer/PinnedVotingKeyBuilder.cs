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
using System.Collections;
using System.Collections.Generic;

namespace Symbol.Builders {
    /*
    * Pinned voting key
    */
    [Serializable]
    public class PinnedVotingKeyBuilder: ISerializer {

        /* Voting key. */
        public VotingPublicKeyDto votingKey;
        /* Start finalization epoch. */
        public FinalizationEpochDto startEpoch;
        /* End finalization epoch. */
        public FinalizationEpochDto endEpoch;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal PinnedVotingKeyBuilder(BinaryReader stream)
        {
            try {
                votingKey = VotingPublicKeyDto.LoadFromBinary(stream);
                startEpoch = FinalizationEpochDto.LoadFromBinary(stream);
                endEpoch = FinalizationEpochDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of PinnedVotingKeyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of PinnedVotingKeyBuilder.
        */
        public static PinnedVotingKeyBuilder LoadFromBinary(BinaryReader stream) {
            return new PinnedVotingKeyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param votingKey Voting key.
        * @param startEpoch Start finalization epoch.
        * @param endEpoch End finalization epoch.
        */
        internal PinnedVotingKeyBuilder(VotingPublicKeyDto votingKey, FinalizationEpochDto startEpoch, FinalizationEpochDto endEpoch)
        {
            GeneratorUtils.NotNull(votingKey, "votingKey is null");
            GeneratorUtils.NotNull(startEpoch, "startEpoch is null");
            GeneratorUtils.NotNull(endEpoch, "endEpoch is null");
            this.votingKey = votingKey;
            this.startEpoch = startEpoch;
            this.endEpoch = endEpoch;
        }
        
        /*
        * Creates an instance of PinnedVotingKeyBuilder.
        *
        * @param votingKey Voting key.
        * @param startEpoch Start finalization epoch.
        * @param endEpoch End finalization epoch.
        * @return Instance of PinnedVotingKeyBuilder.
        */
        public static  PinnedVotingKeyBuilder Create(VotingPublicKeyDto votingKey, FinalizationEpochDto startEpoch, FinalizationEpochDto endEpoch) {
            return new PinnedVotingKeyBuilder(votingKey, startEpoch, endEpoch);
        }

        /*
        * Gets voting key.
        *
        * @return Voting key.
        */
        public VotingPublicKeyDto GetVotingKey() {
            return votingKey;
        }

        /*
        * Gets start finalization epoch.
        *
        * @return Start finalization epoch.
        */
        public FinalizationEpochDto GetStartEpoch() {
            return startEpoch;
        }

        /*
        * Gets end finalization epoch.
        *
        * @return End finalization epoch.
        */
        public FinalizationEpochDto GetEndEpoch() {
            return endEpoch;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += votingKey.GetSize();
            size += startEpoch.GetSize();
            size += endEpoch.GetSize();
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var votingKeyEntityBytes = (votingKey).Serialize();
            bw.Write(votingKeyEntityBytes, 0, votingKeyEntityBytes.Length);
            var startEpochEntityBytes = (startEpoch).Serialize();
            bw.Write(startEpochEntityBytes, 0, startEpochEntityBytes.Length);
            var endEpochEntityBytes = (endEpoch).Serialize();
            bw.Write(endEpochEntityBytes, 0, endEpochEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
