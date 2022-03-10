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
    public class MosaicRestrictionEntryBuilder: StateHeaderBuilder {

        /* Type of restriction being placed upon the entity. */
        public MosaicRestrictionEntryTypeDto entryType;
        /* Address restriction rule. */
        public MosaicAddressRestrictionEntryBuilder addressEntry;
        /* Global mosaic rule. */
        public MosaicGlobalRestrictionEntryBuilder globalEntry;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicRestrictionEntryBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                entryType = (MosaicRestrictionEntryTypeDto)Enum.ToObject(typeof(MosaicRestrictionEntryTypeDto), (byte)stream.ReadByte());
                if (this.entryType == MosaicRestrictionEntryTypeDto.ADDRESS) {
                    addressEntry = MosaicAddressRestrictionEntryBuilder.LoadFromBinary(stream);
                }
                if (this.entryType == MosaicRestrictionEntryTypeDto.GLOBAL) {
                    globalEntry = MosaicGlobalRestrictionEntryBuilder.LoadFromBinary(stream);
                }
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicRestrictionEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicRestrictionEntryBuilder.
        */
        public new static MosaicRestrictionEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicRestrictionEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param entryType Type of restriction being placed upon the entity.
        * @param addressEntry Address restriction rule.
        * @param globalEntry Global mosaic rule.
        */
        internal MosaicRestrictionEntryBuilder(short version, MosaicRestrictionEntryTypeDto entryType, MosaicAddressRestrictionEntryBuilder addressEntry, MosaicGlobalRestrictionEntryBuilder globalEntry)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(entryType, "entryType is null");
            if (entryType == MosaicRestrictionEntryTypeDto.ADDRESS) {
                GeneratorUtils.NotNull(addressEntry, "addressEntry is null");
            }
            if (entryType == MosaicRestrictionEntryTypeDto.GLOBAL) {
                GeneratorUtils.NotNull(globalEntry, "globalEntry is null");
            }
            this.entryType = entryType;
            this.addressEntry = addressEntry;
            this.globalEntry = globalEntry;
        }
        
        /*
        * Creates an instance of MosaicRestrictionEntryBuilder.
        *
        * @param version Serialization version.
        * @param globalEntry Global mosaic rule.
        * @return Instance of MosaicRestrictionEntryBuilder.
        */
        public static  MosaicRestrictionEntryBuilder CreateGLOBAL(short version, MosaicGlobalRestrictionEntryBuilder globalEntry) {
            MosaicRestrictionEntryTypeDto entryType = MosaicRestrictionEntryTypeDto.GLOBAL;
            return new MosaicRestrictionEntryBuilder(version, entryType, null, globalEntry);
        }
        
        /*
        * Creates an instance of MosaicRestrictionEntryBuilder.
        *
        * @param version Serialization version.
        * @param addressEntry Address restriction rule.
        * @return Instance of MosaicRestrictionEntryBuilder.
        */
        public static  MosaicRestrictionEntryBuilder CreateADDRESS(short version, MosaicAddressRestrictionEntryBuilder addressEntry) {
            MosaicRestrictionEntryTypeDto entryType = MosaicRestrictionEntryTypeDto.ADDRESS;
            return new MosaicRestrictionEntryBuilder(version, entryType, addressEntry, null);
        }

        /*
        * Gets type of restriction being placed upon the entity.
        *
        * @return Type of restriction being placed upon the entity.
        */
        public MosaicRestrictionEntryTypeDto GetEntryType() {
            return entryType;
        }

        /*
        * Gets address restriction rule.
        *
        * @return Address restriction rule.
        */
        public MosaicAddressRestrictionEntryBuilder GetAddressEntry() {
            if (!(entryType == MosaicRestrictionEntryTypeDto.ADDRESS)) {
                throw new Exception("entryType is not set to ADDRESS.");
            }
            return addressEntry;
        }

        /*
        * Gets global mosaic rule.
        *
        * @return Global mosaic rule.
        */
        public MosaicGlobalRestrictionEntryBuilder GetGlobalEntry() {
            if (!(entryType == MosaicRestrictionEntryTypeDto.GLOBAL)) {
                throw new Exception("entryType is not set to GLOBAL.");
            }
            return globalEntry;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += entryType.GetSize();
            if (entryType == MosaicRestrictionEntryTypeDto.ADDRESS) {
                if (addressEntry != null) {
                size += ((MosaicAddressRestrictionEntryBuilder) addressEntry).GetSize();
            }
            }
            if (entryType == MosaicRestrictionEntryTypeDto.GLOBAL) {
                if (globalEntry != null) {
                size += ((MosaicGlobalRestrictionEntryBuilder) globalEntry).GetSize();
            }
            }
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
            var entryTypeEntityBytes = (entryType).Serialize();
            bw.Write(entryTypeEntityBytes, 0, entryTypeEntityBytes.Length);
            if (entryType == MosaicRestrictionEntryTypeDto.ADDRESS) {
                var addressEntryEntityBytes = (addressEntry).Serialize();
            bw.Write(addressEntryEntityBytes, 0, addressEntryEntityBytes.Length);
            }
            if (entryType == MosaicRestrictionEntryTypeDto.GLOBAL) {
                var globalEntryEntityBytes = (globalEntry).Serialize();
            bw.Write(globalEntryEntityBytes, 0, globalEntryEntityBytes.Length);
            }
            var result = ms.ToArray();
            return result;
        }
    }
}
