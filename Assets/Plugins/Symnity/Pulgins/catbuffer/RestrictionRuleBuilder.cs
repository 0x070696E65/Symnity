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
    * Binary layout of restriction rule being applied
    */
    [Serializable]
    public class RestrictionRuleBuilder: ISerializer {

        /* Identifier of the mosaic providing the restriction key. */
        public MosaicIdDto referenceMosaicId;
        /* Restriction value. */
        public long restrictionValue;
        /* Restriction type. */
        public MosaicRestrictionTypeDto restrictionType;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal RestrictionRuleBuilder(BinaryReader stream)
        {
            try {
                referenceMosaicId = MosaicIdDto.LoadFromBinary(stream);
                restrictionValue = stream.ReadInt64();
                restrictionType = (MosaicRestrictionTypeDto)Enum.ToObject(typeof(MosaicRestrictionTypeDto), (byte)stream.ReadByte());
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of RestrictionRuleBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of RestrictionRuleBuilder.
        */
        public static RestrictionRuleBuilder LoadFromBinary(BinaryReader stream) {
            return new RestrictionRuleBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param referenceMosaicId Identifier of the mosaic providing the restriction key.
        * @param restrictionValue Restriction value.
        * @param restrictionType Restriction type.
        */
        internal RestrictionRuleBuilder(MosaicIdDto referenceMosaicId, long restrictionValue, MosaicRestrictionTypeDto restrictionType)
        {
            GeneratorUtils.NotNull(referenceMosaicId, "referenceMosaicId is null");
            GeneratorUtils.NotNull(restrictionValue, "restrictionValue is null");
            GeneratorUtils.NotNull(restrictionType, "restrictionType is null");
            this.referenceMosaicId = referenceMosaicId;
            this.restrictionValue = restrictionValue;
            this.restrictionType = restrictionType;
        }
        
        /*
        * Creates an instance of RestrictionRuleBuilder.
        *
        * @param referenceMosaicId Identifier of the mosaic providing the restriction key.
        * @param restrictionValue Restriction value.
        * @param restrictionType Restriction type.
        * @return Instance of RestrictionRuleBuilder.
        */
        public static  RestrictionRuleBuilder Create(MosaicIdDto referenceMosaicId, long restrictionValue, MosaicRestrictionTypeDto restrictionType) {
            return new RestrictionRuleBuilder(referenceMosaicId, restrictionValue, restrictionType);
        }

        /*
        * Gets identifier of the mosaic providing the restriction key.
        *
        * @return Identifier of the mosaic providing the restriction key.
        */
        public MosaicIdDto GetReferenceMosaicId() {
            return referenceMosaicId;
        }

        /*
        * Gets restriction value.
        *
        * @return Restriction value.
        */
        public long GetRestrictionValue() {
            return restrictionValue;
        }

        /*
        * Gets restriction type.
        *
        * @return Restriction type.
        */
        public MosaicRestrictionTypeDto GetRestrictionType() {
            return restrictionType;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += referenceMosaicId.GetSize();
            size += 8; // restrictionValue
            size += restrictionType.GetSize();
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
            var referenceMosaicIdEntityBytes = (referenceMosaicId).Serialize();
            bw.Write(referenceMosaicIdEntityBytes, 0, referenceMosaicIdEntityBytes.Length);
            bw.Write(GetRestrictionValue());
            var restrictionTypeEntityBytes = (restrictionType).Serialize();
            bw.Write(restrictionTypeEntityBytes, 0, restrictionTypeEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
