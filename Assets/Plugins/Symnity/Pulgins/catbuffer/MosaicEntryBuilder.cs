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
    * Binary layout for mosaic entry
    */
    [Serializable]
    public class MosaicEntryBuilder: StateHeaderBuilder {

        /* Entry id. */
        public MosaicIdDto mosaicId;
        /* Total supply amount. */
        public AmountDto supply;
        /* Definition comprised of entry properties. */
        public MosaicDefinitionBuilder definition;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicEntryBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaicId = MosaicIdDto.LoadFromBinary(stream);
                supply = AmountDto.LoadFromBinary(stream);
                definition = MosaicDefinitionBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicEntryBuilder.
        */
        public new static MosaicEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param mosaicId Entry id.
        * @param supply Total supply amount.
        * @param definition Definition comprised of entry properties.
        */
        internal MosaicEntryBuilder(short version, MosaicIdDto mosaicId, AmountDto supply, MosaicDefinitionBuilder definition)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(supply, "supply is null");
            GeneratorUtils.NotNull(definition, "definition is null");
            this.mosaicId = mosaicId;
            this.supply = supply;
            this.definition = definition;
        }
        
        /*
        * Creates an instance of MosaicEntryBuilder.
        *
        * @param version Serialization version.
        * @param mosaicId Entry id.
        * @param supply Total supply amount.
        * @param definition Definition comprised of entry properties.
        * @return Instance of MosaicEntryBuilder.
        */
        public static  MosaicEntryBuilder Create(short version, MosaicIdDto mosaicId, AmountDto supply, MosaicDefinitionBuilder definition) {
            return new MosaicEntryBuilder(version, mosaicId, supply, definition);
        }

        /*
        * Gets entry id.
        *
        * @return Entry id.
        */
        public MosaicIdDto GetMosaicId() {
            return mosaicId;
        }

        /*
        * Gets total supply amount.
        *
        * @return Total supply amount.
        */
        public AmountDto GetSupply() {
            return supply;
        }

        /*
        * Gets definition comprised of entry properties.
        *
        * @return Definition comprised of entry properties.
        */
        public MosaicDefinitionBuilder GetDefinition() {
            return definition;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += mosaicId.GetSize();
            size += supply.GetSize();
            size += definition.GetSize();
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public new byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var superBytes = base.Serialize();
            bw.Write(superBytes, 0, superBytes.Length);
            var mosaicIdEntityBytes = (mosaicId).Serialize();
            bw.Write(mosaicIdEntityBytes, 0, mosaicIdEntityBytes.Length);
            var supplyEntityBytes = (supply).Serialize();
            bw.Write(supplyEntityBytes, 0, supplyEntityBytes.Length);
            var definitionEntityBytes = (definition).Serialize();
            bw.Write(definitionEntityBytes, 0, definitionEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
