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
    * The transaction inside the block that triggered the receipt.
    */
    [Serializable]
    public class ReceiptSourceBuilder: ISerializer {

        /* Transaction primary source (e.g. index within the block).. */
        public int primaryId;
        /* Transaction secondary source (e.g. index within aggregate).. */
        public int secondaryId;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal ReceiptSourceBuilder(BinaryReader stream)
        {
            try {
                primaryId = stream.ReadInt32();
                secondaryId = stream.ReadInt32();
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of ReceiptSourceBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of ReceiptSourceBuilder.
        */
        public static ReceiptSourceBuilder LoadFromBinary(BinaryReader stream) {
            return new ReceiptSourceBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param primaryId Transaction primary source (e.g. index within the block)..
        * @param secondaryId Transaction secondary source (e.g. index within aggregate)..
        */
        internal ReceiptSourceBuilder(int primaryId, int secondaryId)
        {
            GeneratorUtils.NotNull(primaryId, "primaryId is null");
            GeneratorUtils.NotNull(secondaryId, "secondaryId is null");
            this.primaryId = primaryId;
            this.secondaryId = secondaryId;
        }
        
        /*
        * Creates an instance of ReceiptSourceBuilder.
        *
        * @param primaryId Transaction primary source (e.g. index within the block)..
        * @param secondaryId Transaction secondary source (e.g. index within aggregate)..
        * @return Instance of ReceiptSourceBuilder.
        */
        public static  ReceiptSourceBuilder Create(int primaryId, int secondaryId) {
            return new ReceiptSourceBuilder(primaryId, secondaryId);
        }

        /*
        * Gets Transaction primary source (e.g. index within the block)..
        *
        * @return Transaction primary source (e.g. index within the block)..
        */
        public int GetPrimaryId() {
            return primaryId;
        }

        /*
        * Gets Transaction secondary source (e.g. index within aggregate)..
        *
        * @return Transaction secondary source (e.g. index within aggregate)..
        */
        public int GetSecondaryId() {
            return secondaryId;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 4; // primaryId
            size += 4; // secondaryId
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
            bw.Write(GetPrimaryId());
            bw.Write(GetSecondaryId());
            var result = ms.ToArray();
            return result;
        }
    }
}
