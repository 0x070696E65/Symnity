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
    * Shared content between MosaicAddressRestrictionTransaction and EmbeddedMosaicAddressRestrictionTransaction.
    */
    [Serializable]
    public class MosaicAddressRestrictionTransactionBodyBuilder: ISerializer {

        /* Identifier of the mosaic to which the restriction applies.. */
        public UnresolvedMosaicIdDto mosaicId;
        /* Restriction key.. */
        public long restrictionKey;
        /* Previous restriction value. Set `previousRestrictionValue` to `FFFFFFFFFFFFFFFF` if the target address does not have a previous restriction value for this mosaic id and restriction key.. */
        public long previousRestrictionValue;
        /* New restriction value.. */
        public long newRestrictionValue;
        /* Address being restricted.. */
        public UnresolvedAddressDto targetAddress;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicAddressRestrictionTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                mosaicId = UnresolvedMosaicIdDto.LoadFromBinary(stream);
                restrictionKey = stream.ReadInt64();
                previousRestrictionValue = stream.ReadInt64();
                newRestrictionValue = stream.ReadInt64();
                targetAddress = UnresolvedAddressDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicAddressRestrictionTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicAddressRestrictionTransactionBodyBuilder.
        */
        public static MosaicAddressRestrictionTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicAddressRestrictionTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param mosaicId Identifier of the mosaic to which the restriction applies..
        * @param restrictionKey Restriction key..
        * @param previousRestrictionValue Previous restriction value. Set `previousRestrictionValue` to `FFFFFFFFFFFFFFFF` if the target address does not have a previous restriction value for this mosaic id and restriction key..
        * @param newRestrictionValue New restriction value..
        * @param targetAddress Address being restricted..
        */
        internal MosaicAddressRestrictionTransactionBodyBuilder(UnresolvedMosaicIdDto mosaicId, long restrictionKey, long previousRestrictionValue, long newRestrictionValue, UnresolvedAddressDto targetAddress)
        {
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(restrictionKey, "restrictionKey is null");
            GeneratorUtils.NotNull(previousRestrictionValue, "previousRestrictionValue is null");
            GeneratorUtils.NotNull(newRestrictionValue, "newRestrictionValue is null");
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            this.mosaicId = mosaicId;
            this.restrictionKey = restrictionKey;
            this.previousRestrictionValue = previousRestrictionValue;
            this.newRestrictionValue = newRestrictionValue;
            this.targetAddress = targetAddress;
        }
        
        /*
        * Creates an instance of MosaicAddressRestrictionTransactionBodyBuilder.
        *
        * @param mosaicId Identifier of the mosaic to which the restriction applies..
        * @param restrictionKey Restriction key..
        * @param previousRestrictionValue Previous restriction value. Set `previousRestrictionValue` to `FFFFFFFFFFFFFFFF` if the target address does not have a previous restriction value for this mosaic id and restriction key..
        * @param newRestrictionValue New restriction value..
        * @param targetAddress Address being restricted..
        * @return Instance of MosaicAddressRestrictionTransactionBodyBuilder.
        */
        public static  MosaicAddressRestrictionTransactionBodyBuilder Create(UnresolvedMosaicIdDto mosaicId, long restrictionKey, long previousRestrictionValue, long newRestrictionValue, UnresolvedAddressDto targetAddress) {
            return new MosaicAddressRestrictionTransactionBodyBuilder(mosaicId, restrictionKey, previousRestrictionValue, newRestrictionValue, targetAddress);
        }

        /*
        * Gets Identifier of the mosaic to which the restriction applies..
        *
        * @return Identifier of the mosaic to which the restriction applies..
        */
        public UnresolvedMosaicIdDto GetMosaicId() {
            return mosaicId;
        }

        /*
        * Gets Restriction key..
        *
        * @return Restriction key..
        */
        public long GetRestrictionKey() {
            return restrictionKey;
        }

        /*
        * Gets Previous restriction value. Set `previousRestrictionValue` to `FFFFFFFFFFFFFFFF` if the target address does not have a previous restriction value for this mosaic id and restriction key..
        *
        * @return Previous restriction value. Set `previousRestrictionValue` to `FFFFFFFFFFFFFFFF` if the target address does not have a previous restriction value for this mosaic id and restriction key..
        */
        public long GetPreviousRestrictionValue() {
            return previousRestrictionValue;
        }

        /*
        * Gets New restriction value..
        *
        * @return New restriction value..
        */
        public long GetNewRestrictionValue() {
            return newRestrictionValue;
        }

        /*
        * Gets Address being restricted..
        *
        * @return Address being restricted..
        */
        public UnresolvedAddressDto GetTargetAddress() {
            return targetAddress;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += mosaicId.GetSize();
            size += 8; // restrictionKey
            size += 8; // previousRestrictionValue
            size += 8; // newRestrictionValue
            size += targetAddress.GetSize();
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
            bw.Write(GetRestrictionKey());
            bw.Write(GetPreviousRestrictionValue());
            bw.Write(GetNewRestrictionValue());
            var targetAddressEntityBytes = (targetAddress).Serialize();
            bw.Write(targetAddressEntityBytes, 0, targetAddressEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
