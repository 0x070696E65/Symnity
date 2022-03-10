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
    * Binary layout for finalized block header
    */
    [Serializable]
    public class FinalizedBlockHeaderBuilder: ISerializer {

        /* Finalization round. */
        public FinalizationRoundBuilder round;
        /* Finalization height. */
        public HeightDto height;
        /* Finalization hash. */
        public Hash256Dto hash;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal FinalizedBlockHeaderBuilder(BinaryReader stream)
        {
            try {
                round = FinalizationRoundBuilder.LoadFromBinary(stream);
                height = HeightDto.LoadFromBinary(stream);
                hash = Hash256Dto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of FinalizedBlockHeaderBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of FinalizedBlockHeaderBuilder.
        */
        public static FinalizedBlockHeaderBuilder LoadFromBinary(BinaryReader stream) {
            return new FinalizedBlockHeaderBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param round Finalization round.
        * @param height Finalization height.
        * @param hash Finalization hash.
        */
        internal FinalizedBlockHeaderBuilder(FinalizationRoundBuilder round, HeightDto height, Hash256Dto hash)
        {
            GeneratorUtils.NotNull(round, "round is null");
            GeneratorUtils.NotNull(height, "height is null");
            GeneratorUtils.NotNull(hash, "hash is null");
            this.round = round;
            this.height = height;
            this.hash = hash;
        }
        
        /*
        * Creates an instance of FinalizedBlockHeaderBuilder.
        *
        * @param round Finalization round.
        * @param height Finalization height.
        * @param hash Finalization hash.
        * @return Instance of FinalizedBlockHeaderBuilder.
        */
        public static  FinalizedBlockHeaderBuilder Create(FinalizationRoundBuilder round, HeightDto height, Hash256Dto hash) {
            return new FinalizedBlockHeaderBuilder(round, height, hash);
        }

        /*
        * Gets finalization round.
        *
        * @return Finalization round.
        */
        public FinalizationRoundBuilder GetRound() {
            return round;
        }

        /*
        * Gets finalization height.
        *
        * @return Finalization height.
        */
        public HeightDto GetHeight() {
            return height;
        }

        /*
        * Gets finalization hash.
        *
        * @return Finalization hash.
        */
        public Hash256Dto GetHash() {
            return hash;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += round.GetSize();
            size += height.GetSize();
            size += hash.GetSize();
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
            var roundEntityBytes = (round).Serialize();
            bw.Write(roundEntityBytes, 0, roundEntityBytes.Length);
            var heightEntityBytes = (height).Serialize();
            bw.Write(heightEntityBytes, 0, heightEntityBytes.Length);
            var hashEntityBytes = (hash).Serialize();
            bw.Write(hashEntityBytes, 0, hashEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
