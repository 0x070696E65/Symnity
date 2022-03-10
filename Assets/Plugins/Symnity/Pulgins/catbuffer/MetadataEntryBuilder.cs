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
    * Binary layout of a metadata entry
    */
    [Serializable]
    public class MetadataEntryBuilder: StateHeaderBuilder {

        /* Metadata source address (provider). */
        public AddressDto sourceAddress;
        /* Metadata target address. */
        public AddressDto targetAddress;
        /* Metadata key scoped to source, target and type. */
        public ScopedMetadataKeyDto scopedMetadataKey;
        /* Target id. */
        public long targetId;
        /* Metadata type. */
        public MetadataTypeDto metadataType;
        /* Value. */
        public MetadataValueBuilder value;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MetadataEntryBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                sourceAddress = AddressDto.LoadFromBinary(stream);
                targetAddress = AddressDto.LoadFromBinary(stream);
                scopedMetadataKey = ScopedMetadataKeyDto.LoadFromBinary(stream);
                targetId = stream.ReadInt64();
                metadataType = (MetadataTypeDto)Enum.ToObject(typeof(MetadataTypeDto), (byte)stream.ReadByte());
                value = MetadataValueBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MetadataEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MetadataEntryBuilder.
        */
        public new static MetadataEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MetadataEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param sourceAddress Metadata source address (provider).
        * @param targetAddress Metadata target address.
        * @param scopedMetadataKey Metadata key scoped to source, target and type.
        * @param targetId Target id.
        * @param metadataType Metadata type.
        * @param value Value.
        */
        internal MetadataEntryBuilder(short version, AddressDto sourceAddress, AddressDto targetAddress, ScopedMetadataKeyDto scopedMetadataKey, long targetId, MetadataTypeDto metadataType, MetadataValueBuilder value)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(sourceAddress, "sourceAddress is null");
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            GeneratorUtils.NotNull(scopedMetadataKey, "scopedMetadataKey is null");
            GeneratorUtils.NotNull(targetId, "targetId is null");
            GeneratorUtils.NotNull(metadataType, "metadataType is null");
            GeneratorUtils.NotNull(value, "value is null");
            this.sourceAddress = sourceAddress;
            this.targetAddress = targetAddress;
            this.scopedMetadataKey = scopedMetadataKey;
            this.targetId = targetId;
            this.metadataType = metadataType;
            this.value = value;
        }
        
        /*
        * Creates an instance of MetadataEntryBuilder.
        *
        * @param version Serialization version.
        * @param sourceAddress Metadata source address (provider).
        * @param targetAddress Metadata target address.
        * @param scopedMetadataKey Metadata key scoped to source, target and type.
        * @param targetId Target id.
        * @param metadataType Metadata type.
        * @param value Value.
        * @return Instance of MetadataEntryBuilder.
        */
        public static  MetadataEntryBuilder Create(short version, AddressDto sourceAddress, AddressDto targetAddress, ScopedMetadataKeyDto scopedMetadataKey, long targetId, MetadataTypeDto metadataType, MetadataValueBuilder value) {
            return new MetadataEntryBuilder(version, sourceAddress, targetAddress, scopedMetadataKey, targetId, metadataType, value);
        }

        /*
        * Gets metadata source address (provider).
        *
        * @return Metadata source address (provider).
        */
        public AddressDto GetSourceAddress() {
            return sourceAddress;
        }

        /*
        * Gets metadata target address.
        *
        * @return Metadata target address.
        */
        public AddressDto GetTargetAddress() {
            return targetAddress;
        }

        /*
        * Gets metadata key scoped to source, target and type.
        *
        * @return Metadata key scoped to source, target and type.
        */
        public ScopedMetadataKeyDto GetScopedMetadataKey() {
            return scopedMetadataKey;
        }

        /*
        * Gets target id.
        *
        * @return Target id.
        */
        public long GetTargetId() {
            return targetId;
        }

        /*
        * Gets metadata type.
        *
        * @return Metadata type.
        */
        public MetadataTypeDto GetMetadataType() {
            return metadataType;
        }

        /*
        * Gets value.
        *
        * @return Value.
        */
        public MetadataValueBuilder GetValue() {
            return value;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += sourceAddress.GetSize();
            size += targetAddress.GetSize();
            size += scopedMetadataKey.GetSize();
            size += 8; // targetId
            size += metadataType.GetSize();
            size += value.GetSize();
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
            var sourceAddressEntityBytes = (sourceAddress).Serialize();
            bw.Write(sourceAddressEntityBytes, 0, sourceAddressEntityBytes.Length);
            var targetAddressEntityBytes = (targetAddress).Serialize();
            bw.Write(targetAddressEntityBytes, 0, targetAddressEntityBytes.Length);
            var scopedMetadataKeyEntityBytes = (scopedMetadataKey).Serialize();
            bw.Write(scopedMetadataKeyEntityBytes, 0, scopedMetadataKeyEntityBytes.Length);
            bw.Write(GetTargetId());
            var metadataTypeEntityBytes = (metadataType).Serialize();
            bw.Write(metadataTypeEntityBytes, 0, metadataTypeEntityBytes.Length);
            var valueEntityBytes = (value).Serialize();
            bw.Write(valueEntityBytes, 0, valueEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
