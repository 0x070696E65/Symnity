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
    public class MosaicAddressRestrictionEntryBuilder: ISerializer {

        /* Identifier of the mosaic to which the restriction applies. */
        public MosaicIdDto mosaicId;
        /* Address being restricted. */
        public AddressDto address;
        /* Address key value restriction set. */
        public AddressKeyValueSetBuilder keyPairs;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicAddressRestrictionEntryBuilder(BinaryReader stream)
        {
            try {
                mosaicId = MosaicIdDto.LoadFromBinary(stream);
                address = AddressDto.LoadFromBinary(stream);
                keyPairs = AddressKeyValueSetBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicAddressRestrictionEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicAddressRestrictionEntryBuilder.
        */
        public static MosaicAddressRestrictionEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicAddressRestrictionEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param mosaicId Identifier of the mosaic to which the restriction applies.
        * @param address Address being restricted.
        * @param keyPairs Address key value restriction set.
        */
        internal MosaicAddressRestrictionEntryBuilder(MosaicIdDto mosaicId, AddressDto address, AddressKeyValueSetBuilder keyPairs)
        {
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(address, "address is null");
            GeneratorUtils.NotNull(keyPairs, "keyPairs is null");
            this.mosaicId = mosaicId;
            this.address = address;
            this.keyPairs = keyPairs;
        }
        
        /*
        * Creates an instance of MosaicAddressRestrictionEntryBuilder.
        *
        * @param mosaicId Identifier of the mosaic to which the restriction applies.
        * @param address Address being restricted.
        * @param keyPairs Address key value restriction set.
        * @return Instance of MosaicAddressRestrictionEntryBuilder.
        */
        public static  MosaicAddressRestrictionEntryBuilder Create(MosaicIdDto mosaicId, AddressDto address, AddressKeyValueSetBuilder keyPairs) {
            return new MosaicAddressRestrictionEntryBuilder(mosaicId, address, keyPairs);
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
        * Gets address being restricted.
        *
        * @return Address being restricted.
        */
        public AddressDto GetAddress() {
            return address;
        }

        /*
        * Gets address key value restriction set.
        *
        * @return Address key value restriction set.
        */
        public AddressKeyValueSetBuilder GetKeyPairs() {
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
            size += address.GetSize();
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
            var addressEntityBytes = (address).Serialize();
            bw.Write(addressEntityBytes, 0, addressEntityBytes.Length);
            var keyPairsEntityBytes = (keyPairs).Serialize();
            bw.Write(keyPairsEntityBytes, 0, keyPairsEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
