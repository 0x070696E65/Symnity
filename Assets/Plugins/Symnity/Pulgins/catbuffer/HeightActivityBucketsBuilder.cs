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
    * Account activity buckets
    */
    [Serializable]
    public class HeightActivityBucketsBuilder: ISerializer {

        /* Account activity buckets. */
        public List<HeightActivityBucketBuilder> buckets;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal HeightActivityBucketsBuilder(BinaryReader stream)
        {
            try {
                buckets = GeneratorUtils.LoadFromBinaryArray(HeightActivityBucketBuilder.LoadFromBinary, stream, 5, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of HeightActivityBucketsBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of HeightActivityBucketsBuilder.
        */
        public static HeightActivityBucketsBuilder LoadFromBinary(BinaryReader stream) {
            return new HeightActivityBucketsBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param buckets Account activity buckets.
        */
        internal HeightActivityBucketsBuilder(List<HeightActivityBucketBuilder> buckets)
        {
            GeneratorUtils.NotNull(buckets, "buckets is null");
            this.buckets = buckets;
        }
        
        /*
        * Creates an instance of HeightActivityBucketsBuilder.
        *
        * @param buckets Account activity buckets.
        * @return Instance of HeightActivityBucketsBuilder.
        */
        public static  HeightActivityBucketsBuilder Create(List<HeightActivityBucketBuilder> buckets) {
            return new HeightActivityBucketsBuilder(buckets);
        }

        /*
        * Gets account activity buckets.
        *
        * @return Account activity buckets.
        */
        public List<HeightActivityBucketBuilder> GetBuckets() {
            return buckets;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size +=  GeneratorUtils.GetSumSize(buckets, 0);
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
            buckets.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            var result = ms.ToArray();
            return result;
        }
    }
}
