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
    * Shared content between MosaicSupplyRevocationTransaction and EmbeddedMosaicSupplyRevocationTransaction.
    */
    [Serializable]
    public class MosaicSupplyRevocationTransactionBodyBuilder: ISerializer {

        /* Address from which tokens should be revoked.. */
        public UnresolvedAddressDto sourceAddress;
        /* Revoked mosaic and amount.. */
        public UnresolvedMosaicBuilder mosaic;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicSupplyRevocationTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                sourceAddress = UnresolvedAddressDto.LoadFromBinary(stream);
                mosaic = UnresolvedMosaicBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicSupplyRevocationTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicSupplyRevocationTransactionBodyBuilder.
        */
        public static MosaicSupplyRevocationTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicSupplyRevocationTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param sourceAddress Address from which tokens should be revoked..
        * @param mosaic Revoked mosaic and amount..
        */
        internal MosaicSupplyRevocationTransactionBodyBuilder(UnresolvedAddressDto sourceAddress, UnresolvedMosaicBuilder mosaic)
        {
            GeneratorUtils.NotNull(sourceAddress, "sourceAddress is null");
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            this.sourceAddress = sourceAddress;
            this.mosaic = mosaic;
        }
        
        /*
        * Creates an instance of MosaicSupplyRevocationTransactionBodyBuilder.
        *
        * @param sourceAddress Address from which tokens should be revoked..
        * @param mosaic Revoked mosaic and amount..
        * @return Instance of MosaicSupplyRevocationTransactionBodyBuilder.
        */
        public static  MosaicSupplyRevocationTransactionBodyBuilder Create(UnresolvedAddressDto sourceAddress, UnresolvedMosaicBuilder mosaic) {
            return new MosaicSupplyRevocationTransactionBodyBuilder(sourceAddress, mosaic);
        }

        /*
        * Gets Address from which tokens should be revoked..
        *
        * @return Address from which tokens should be revoked..
        */
        public UnresolvedAddressDto GetSourceAddress() {
            return sourceAddress;
        }

        /*
        * Gets Revoked mosaic and amount..
        *
        * @return Revoked mosaic and amount..
        */
        public UnresolvedMosaicBuilder GetMosaic() {
            return mosaic;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += sourceAddress.GetSize();
            size += mosaic.GetSize();
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
            var sourceAddressEntityBytes = (sourceAddress).Serialize();
            bw.Write(sourceAddressEntityBytes, 0, sourceAddressEntityBytes.Length);
            var mosaicEntityBytes = (mosaic).Serialize();
            bw.Write(mosaicEntityBytes, 0, mosaicEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
