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
    * Shared content between MosaicSupplyChangeTransaction and EmbeddedMosaicSupplyChangeTransaction.
    */
    [Serializable]
    public class MosaicSupplyChangeTransactionBodyBuilder: ISerializer {

        /* Affected mosaic identifier.. */
        public UnresolvedMosaicIdDto mosaicId;
        /* Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply.. */
        public AmountDto delta;
        /* Supply change action.. */
        public MosaicSupplyChangeActionDto action;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicSupplyChangeTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                mosaicId = UnresolvedMosaicIdDto.LoadFromBinary(stream);
                delta = AmountDto.LoadFromBinary(stream);
                action = (MosaicSupplyChangeActionDto)Enum.ToObject(typeof(MosaicSupplyChangeActionDto), (byte)stream.ReadByte());
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicSupplyChangeTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicSupplyChangeTransactionBodyBuilder.
        */
        public static MosaicSupplyChangeTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicSupplyChangeTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param mosaicId Affected mosaic identifier..
        * @param delta Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        * @param action Supply change action..
        */
        internal MosaicSupplyChangeTransactionBodyBuilder(UnresolvedMosaicIdDto mosaicId, AmountDto delta, MosaicSupplyChangeActionDto action)
        {
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(delta, "delta is null");
            GeneratorUtils.NotNull(action, "action is null");
            this.mosaicId = mosaicId;
            this.delta = delta;
            this.action = action;
        }
        
        /*
        * Creates an instance of MosaicSupplyChangeTransactionBodyBuilder.
        *
        * @param mosaicId Affected mosaic identifier..
        * @param delta Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        * @param action Supply change action..
        * @return Instance of MosaicSupplyChangeTransactionBodyBuilder.
        */
        public static  MosaicSupplyChangeTransactionBodyBuilder Create(UnresolvedMosaicIdDto mosaicId, AmountDto delta, MosaicSupplyChangeActionDto action) {
            return new MosaicSupplyChangeTransactionBodyBuilder(mosaicId, delta, action);
        }

        /*
        * Gets Affected mosaic identifier..
        *
        * @return Affected mosaic identifier..
        */
        public UnresolvedMosaicIdDto GetMosaicId() {
            return mosaicId;
        }

        /*
        * Gets Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        *
        * @return Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        */
        public AmountDto GetDelta() {
            return delta;
        }

        /*
        * Gets Supply change action..
        *
        * @return Supply change action..
        */
        public MosaicSupplyChangeActionDto GetAction() {
            return action;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += mosaicId.GetSize();
            size += delta.GetSize();
            size += action.GetSize();
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
            var deltaEntityBytes = (delta).Serialize();
            bw.Write(deltaEntityBytes, 0, deltaEntityBytes.Length);
            var actionEntityBytes = (action).Serialize();
            bw.Write(actionEntityBytes, 0, actionEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
