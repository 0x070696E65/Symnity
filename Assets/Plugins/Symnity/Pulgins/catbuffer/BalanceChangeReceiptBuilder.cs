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
    * An invisible state change modified an account's balance.
    */
    [Serializable]
    public class BalanceChangeReceiptBuilder: ReceiptBuilder {

        /* Modified mosaic.. */
        public MosaicBuilder mosaic;
        /* Address of the affected account.. */
        public AddressDto targetAddress;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal BalanceChangeReceiptBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaic = MosaicBuilder.LoadFromBinary(stream);
                targetAddress = AddressDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of BalanceChangeReceiptBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of BalanceChangeReceiptBuilder.
        */
        public new static BalanceChangeReceiptBuilder LoadFromBinary(BinaryReader stream) {
            return new BalanceChangeReceiptBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param mosaic Modified mosaic..
        * @param targetAddress Address of the affected account..
        */
        internal BalanceChangeReceiptBuilder(short version, ReceiptTypeDto type, MosaicBuilder mosaic, AddressDto targetAddress)
            : base(version, type)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            this.mosaic = mosaic;
            this.targetAddress = targetAddress;
        }
        
        /*
        * Creates an instance of BalanceChangeReceiptBuilder.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param mosaic Modified mosaic..
        * @param targetAddress Address of the affected account..
        * @return Instance of BalanceChangeReceiptBuilder.
        */
        public static  BalanceChangeReceiptBuilder Create(short version, ReceiptTypeDto type, MosaicBuilder mosaic, AddressDto targetAddress) {
            return new BalanceChangeReceiptBuilder(version, type, mosaic, targetAddress);
        }

        /*
        * Gets Modified mosaic..
        *
        * @return Modified mosaic..
        */
        public MosaicBuilder GetMosaic() {
            return mosaic;
        }

        /*
        * Gets Address of the affected account..
        *
        * @return Address of the affected account..
        */
        public AddressDto GetTargetAddress() {
            return targetAddress;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += mosaic.GetSize();
            size += targetAddress.GetSize();
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
            var mosaicEntityBytes = (mosaic).Serialize();
            bw.Write(mosaicEntityBytes, 0, mosaicEntityBytes.Length);
            var targetAddressEntityBytes = (targetAddress).Serialize();
            bw.Write(targetAddressEntityBytes, 0, targetAddressEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
