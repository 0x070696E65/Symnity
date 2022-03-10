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
    * Shared content between HashLockTransaction and EmbeddedHashLockTransaction.
    */
    [Serializable]
    public class HashLockTransactionBodyBuilder: ISerializer {

        /* Locked mosaic.. */
        public UnresolvedMosaicBuilder mosaic;
        /* Number of blocks for which a lock should be valid.
The default maximum is 48h (See the `maxHashLockDuration` network property).. */
        public BlockDurationDto duration;
        /* Hash of the AggregateBondedTransaction to be confirmed before unlocking the mosaics.. */
        public Hash256Dto hash;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal HashLockTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                mosaic = UnresolvedMosaicBuilder.LoadFromBinary(stream);
                duration = BlockDurationDto.LoadFromBinary(stream);
                hash = Hash256Dto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of HashLockTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of HashLockTransactionBodyBuilder.
        */
        public static HashLockTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new HashLockTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param mosaic Locked mosaic..
        * @param duration Number of blocks for which a lock should be valid.
The default maximum is 48h (See the `maxHashLockDuration` network property)..
        * @param hash Hash of the AggregateBondedTransaction to be confirmed before unlocking the mosaics..
        */
        internal HashLockTransactionBodyBuilder(UnresolvedMosaicBuilder mosaic, BlockDurationDto duration, Hash256Dto hash)
        {
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            GeneratorUtils.NotNull(duration, "duration is null");
            GeneratorUtils.NotNull(hash, "hash is null");
            this.mosaic = mosaic;
            this.duration = duration;
            this.hash = hash;
        }
        
        /*
        * Creates an instance of HashLockTransactionBodyBuilder.
        *
        * @param mosaic Locked mosaic..
        * @param duration Number of blocks for which a lock should be valid.
The default maximum is 48h (See the `maxHashLockDuration` network property)..
        * @param hash Hash of the AggregateBondedTransaction to be confirmed before unlocking the mosaics..
        * @return Instance of HashLockTransactionBodyBuilder.
        */
        public static  HashLockTransactionBodyBuilder Create(UnresolvedMosaicBuilder mosaic, BlockDurationDto duration, Hash256Dto hash) {
            return new HashLockTransactionBodyBuilder(mosaic, duration, hash);
        }

        /*
        * Gets Locked mosaic..
        *
        * @return Locked mosaic..
        */
        public UnresolvedMosaicBuilder GetMosaic() {
            return mosaic;
        }

        /*
        * Gets Number of blocks for which a lock should be valid.
The default maximum is 48h (See the `maxHashLockDuration` network property)..
        *
        * @return Number of blocks for which a lock should be valid.
The default maximum is 48h (See the `maxHashLockDuration` network property)..
        */
        public BlockDurationDto GetDuration() {
            return duration;
        }

        /*
        * Gets Hash of the AggregateBondedTransaction to be confirmed before unlocking the mosaics..
        *
        * @return Hash of the AggregateBondedTransaction to be confirmed before unlocking the mosaics..
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
            size += mosaic.GetSize();
            size += duration.GetSize();
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
            var mosaicEntityBytes = (mosaic).Serialize();
            bw.Write(mosaicEntityBytes, 0, mosaicEntityBytes.Length);
            var durationEntityBytes = (duration).Serialize();
            bw.Write(durationEntityBytes, 0, durationEntityBytes.Length);
            var hashEntityBytes = (hash).Serialize();
            bw.Write(hashEntityBytes, 0, hashEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
