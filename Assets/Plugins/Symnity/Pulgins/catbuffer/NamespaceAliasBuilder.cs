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
    * Binary layout for alias
    */
    [Serializable]
    public class NamespaceAliasBuilder: ISerializer {

        /* Namespace alias type. */
        public NamespaceAliasTypeDto namespaceAliasType;
        /* Mosaic alias. */
        public MosaicIdDto? mosaicAlias;
        /* Address alias. */
        public AddressDto? addressAlias;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal NamespaceAliasBuilder(BinaryReader stream)
        {
            try {
                namespaceAliasType = (NamespaceAliasTypeDto)Enum.ToObject(typeof(NamespaceAliasTypeDto), (byte)stream.ReadByte());
                if (this.namespaceAliasType == NamespaceAliasTypeDto.MOSAIC_ID) {
                    mosaicAlias = MosaicIdDto.LoadFromBinary(stream);
                }
                if (this.namespaceAliasType == NamespaceAliasTypeDto.ADDRESS) {
                    addressAlias = AddressDto.LoadFromBinary(stream);
                }
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of NamespaceAliasBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NamespaceAliasBuilder.
        */
        public static NamespaceAliasBuilder LoadFromBinary(BinaryReader stream) {
            return new NamespaceAliasBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param namespaceAliasType Namespace alias type.
        * @param mosaicAlias Mosaic alias.
        * @param addressAlias Address alias.
        */
        internal NamespaceAliasBuilder(NamespaceAliasTypeDto namespaceAliasType, MosaicIdDto? mosaicAlias, AddressDto? addressAlias)
        {
            GeneratorUtils.NotNull(namespaceAliasType, "namespaceAliasType is null");
            if (namespaceAliasType == NamespaceAliasTypeDto.MOSAIC_ID) {
                GeneratorUtils.NotNull(mosaicAlias, "mosaicAlias is null");
            }
            if (namespaceAliasType == NamespaceAliasTypeDto.ADDRESS) {
                GeneratorUtils.NotNull(addressAlias, "addressAlias is null");
            }
            this.namespaceAliasType = namespaceAliasType;
            this.mosaicAlias = mosaicAlias;
            this.addressAlias = addressAlias;
        }
        
        /*
        * Creates an instance of NamespaceAliasBuilder.
        *
        * @param addressAlias Address alias.
        * @return Instance of NamespaceAliasBuilder.
        */
        public static  NamespaceAliasBuilder CreateADDRESS(AddressDto addressAlias) {
            NamespaceAliasTypeDto namespaceAliasType = NamespaceAliasTypeDto.ADDRESS;
            return new NamespaceAliasBuilder(namespaceAliasType, null, addressAlias);
        }
        
        /*
        * Creates an instance of NamespaceAliasBuilder.
        *
        * @param mosaicAlias Mosaic alias.
        * @return Instance of NamespaceAliasBuilder.
        */
        public static  NamespaceAliasBuilder CreateMOSAIC_ID(MosaicIdDto mosaicAlias) {
            NamespaceAliasTypeDto namespaceAliasType = NamespaceAliasTypeDto.MOSAIC_ID;
            return new NamespaceAliasBuilder(namespaceAliasType, mosaicAlias, null);
        }
        
        /*
        * Creates an instance of NamespaceAliasBuilder.
        *
        * @return Instance of NamespaceAliasBuilder.
        */
        public static  NamespaceAliasBuilder CreateNONE() {
            NamespaceAliasTypeDto namespaceAliasType = NamespaceAliasTypeDto.NONE;
            return new NamespaceAliasBuilder(namespaceAliasType, null, null);
        }

        /*
        * Gets namespace alias type.
        *
        * @return Namespace alias type.
        */
        public NamespaceAliasTypeDto GetNamespaceAliasType() {
            return namespaceAliasType;
        }

        /*
        * Gets mosaic alias.
        *
        * @return Mosaic alias.
        */
        public MosaicIdDto? GetMosaicAlias() {
            if (!(namespaceAliasType == NamespaceAliasTypeDto.MOSAIC_ID)) {
                throw new Exception("namespaceAliasType is not set to MOSAIC_ID.");
            }
            return mosaicAlias;
        }

        /*
        * Gets address alias.
        *
        * @return Address alias.
        */
        public AddressDto? GetAddressAlias() {
            if (!(namespaceAliasType == NamespaceAliasTypeDto.ADDRESS)) {
                throw new Exception("namespaceAliasType is not set to ADDRESS.");
            }
            return addressAlias;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += namespaceAliasType.GetSize();
            if (namespaceAliasType == NamespaceAliasTypeDto.MOSAIC_ID) {
                if (mosaicAlias != null) {
                size += ((MosaicIdDto) mosaicAlias).GetSize();
            }
            }
            if (namespaceAliasType == NamespaceAliasTypeDto.ADDRESS) {
                if (addressAlias != null) {
                size += ((AddressDto) addressAlias).GetSize();
            }
            }
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
            var namespaceAliasTypeEntityBytes = (namespaceAliasType).Serialize();
            bw.Write(namespaceAliasTypeEntityBytes, 0, namespaceAliasTypeEntityBytes.Length);
            if (namespaceAliasType == NamespaceAliasTypeDto.MOSAIC_ID) {
                var mosaicAliasEntityBytes = ((MosaicIdDto)mosaicAlias).Serialize();
            bw.Write(mosaicAliasEntityBytes, 0, mosaicAliasEntityBytes.Length);
            }
            if (namespaceAliasType == NamespaceAliasTypeDto.ADDRESS) {
                var addressAliasEntityBytes = ((AddressDto)addressAlias).Serialize();
            bw.Write(addressAliasEntityBytes, 0, addressAliasEntityBytes.Length);
            }
            var result = ms.ToArray();
            return result;
        }
    }
}
