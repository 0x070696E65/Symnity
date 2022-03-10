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
    * Binary layout for a mosaic restriction
    */
    [Serializable]
    public class MosaicGlobalRestrictionEntryBuilder: ISerializer {

        /* Identifier of the mosaic to which the restriction applies. */
        public MosaicIdDto mosaicId;
        /* Global key value restriction set. */
        public GlobalKeyValueSetBuilder keyPairs;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicGlobalRestrictionEntryBuilder(BinaryReader stream)
        {
            try {
                mosaicId = MosaicIdDto.LoadFromBinary(stream);
                keyPairs = GlobalKeyValueSetBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicGlobalRestrictionEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicGlobalRestrictionEntryBuilder.
        */
        public static MosaicGlobalRestrictionEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicGlobalRestrictionEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param mosaicId Identifier of the mosaic to which the restriction applies.
        * @param keyPairs Global key value restriction set.
        */
        internal MosaicGlobalRestrictionEntryBuilder(MosaicIdDto mosaicId, GlobalKeyValueSetBuilder keyPairs)
        {
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(keyPairs, "keyPairs is null");
            this.mosaicId = mosaicId;
            this.keyPairs = keyPairs;
        }
        
        /*
        * Creates an instance of MosaicGlobalRestrictionEntryBuilder.
        *
        * @param mosaicId Identifier of the mosaic to which the restriction applies.
        * @param keyPairs Global key value restriction set.
        * @return Instance of MosaicGlobalRestrictionEntryBuilder.
        */
        public static  MosaicGlobalRestrictionEntryBuilder Create(MosaicIdDto mosaicId, GlobalKeyValueSetBuilder keyPairs) {
            return new MosaicGlobalRestrictionEntryBuilder(mosaicId, keyPairs);
        }

        /*
        * Gets identifier of the mosaic to which the restriction applies.
        *
        * @return Identifier of the mosaic to which the restriction applies.
        */
        public MosaicIdDto GetMosaicId() {
            return mosaicId;
        }

        /*
        * Gets global key value restriction set.
        *
        * @return Global key value restriction set.
        */
        public GlobalKeyValueSetBuilder GetKeyPairs() {
            return keyPairs;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += mosaicId.GetSize();
            size += keyPairs.GetSize();
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
            var mosaicIdEntityBytes = (mosaicId).Serialize();
            bw.Write(mosaicIdEntityBytes, 0, mosaicIdEntityBytes.Length);
            var keyPairsEntityBytes = (keyPairs).Serialize();
            bw.Write(keyPairsEntityBytes, 0, keyPairsEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
