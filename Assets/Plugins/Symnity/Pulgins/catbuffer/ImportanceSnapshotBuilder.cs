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
    * Temporal importance information
    */
    [Serializable]
    public class ImportanceSnapshotBuilder: ISerializer {

        /* Account importance. */
        public ImportanceDto importance;
        /* Importance height. */
        public ImportanceHeightDto height;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal ImportanceSnapshotBuilder(BinaryReader stream)
        {
            try {
                importance = ImportanceDto.LoadFromBinary(stream);
                height = ImportanceHeightDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of ImportanceSnapshotBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of ImportanceSnapshotBuilder.
        */
        public static ImportanceSnapshotBuilder LoadFromBinary(BinaryReader stream) {
            return new ImportanceSnapshotBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param importance Account importance.
        * @param height Importance height.
        */
        internal ImportanceSnapshotBuilder(ImportanceDto importance, ImportanceHeightDto height)
        {
            GeneratorUtils.NotNull(importance, "importance is null");
            GeneratorUtils.NotNull(height, "height is null");
            this.importance = importance;
            this.height = height;
        }
        
        /*
        * Creates an instance of ImportanceSnapshotBuilder.
        *
        * @param importance Account importance.
        * @param height Importance height.
        * @return Instance of ImportanceSnapshotBuilder.
        */
        public static  ImportanceSnapshotBuilder Create(ImportanceDto importance, ImportanceHeightDto height) {
            return new ImportanceSnapshotBuilder(importance, height);
        }

        /*
        * Gets account importance.
        *
        * @return Account importance.
        */
        public ImportanceDto GetImportance() {
            return importance;
        }

        /*
        * Gets importance height.
        *
        * @return Importance height.
        */
        public ImportanceHeightDto GetHeight() {
            return height;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += importance.GetSize();
            size += height.GetSize();
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
            var importanceEntityBytes = (importance).Serialize();
            bw.Write(importanceEntityBytes, 0, importanceEntityBytes.Length);
            var heightEntityBytes = (height).Serialize();
            bw.Write(heightEntityBytes, 0, heightEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
