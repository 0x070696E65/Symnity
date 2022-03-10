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
    * Binary layout for mosaic definition
    */
    [Serializable]
    public class MosaicDefinitionBuilder: ISerializer {

        /* Block height. */
        public HeightDto startHeight;
        /* Mosaic owner. */
        public AddressDto ownerAddress;
        /* Revision. */
        public int revision;
        /* Properties. */
        public MosaicPropertiesBuilder properties;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicDefinitionBuilder(BinaryReader stream)
        {
            try {
                startHeight = HeightDto.LoadFromBinary(stream);
                ownerAddress = AddressDto.LoadFromBinary(stream);
                revision = stream.ReadInt32();
                properties = MosaicPropertiesBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicDefinitionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicDefinitionBuilder.
        */
        public static MosaicDefinitionBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicDefinitionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param startHeight Block height.
        * @param ownerAddress Mosaic owner.
        * @param revision Revision.
        * @param properties Properties.
        */
        internal MosaicDefinitionBuilder(HeightDto startHeight, AddressDto ownerAddress, int revision, MosaicPropertiesBuilder properties)
        {
            GeneratorUtils.NotNull(startHeight, "startHeight is null");
            GeneratorUtils.NotNull(ownerAddress, "ownerAddress is null");
            GeneratorUtils.NotNull(revision, "revision is null");
            GeneratorUtils.NotNull(properties, "properties is null");
            this.startHeight = startHeight;
            this.ownerAddress = ownerAddress;
            this.revision = revision;
            this.properties = properties;
        }
        
        /*
        * Creates an instance of MosaicDefinitionBuilder.
        *
        * @param startHeight Block height.
        * @param ownerAddress Mosaic owner.
        * @param revision Revision.
        * @param properties Properties.
        * @return Instance of MosaicDefinitionBuilder.
        */
        public static  MosaicDefinitionBuilder Create(HeightDto startHeight, AddressDto ownerAddress, int revision, MosaicPropertiesBuilder properties) {
            return new MosaicDefinitionBuilder(startHeight, ownerAddress, revision, properties);
        }

        /*
        * Gets block height.
        *
        * @return Block height.
        */
        public HeightDto GetStartHeight() {
            return startHeight;
        }

        /*
        * Gets mosaic owner.
        *
        * @return Mosaic owner.
        */
        public AddressDto GetOwnerAddress() {
            return ownerAddress;
        }

        /*
        * Gets revision.
        *
        * @return Revision.
        */
        public int GetRevision() {
            return revision;
        }

        /*
        * Gets properties.
        *
        * @return Properties.
        */
        public MosaicPropertiesBuilder GetProperties() {
            return properties;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += startHeight.GetSize();
            size += ownerAddress.GetSize();
            size += 4; // revision
            size += properties.GetSize();
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
            var startHeightEntityBytes = (startHeight).Serialize();
            bw.Write(startHeightEntityBytes, 0, startHeightEntityBytes.Length);
            var ownerAddressEntityBytes = (ownerAddress).Serialize();
            bw.Write(ownerAddressEntityBytes, 0, ownerAddressEntityBytes.Length);
            bw.Write(GetRevision());
            var propertiesEntityBytes = (properties).Serialize();
            bw.Write(propertiesEntityBytes, 0, propertiesEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
