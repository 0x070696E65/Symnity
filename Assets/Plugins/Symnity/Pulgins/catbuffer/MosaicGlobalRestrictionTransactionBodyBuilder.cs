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
    * Shared content between MosaicGlobalRestrictionTransaction and EmbeddedMosaicGlobalRestrictionTransaction.
    */
    [Serializable]
    public class MosaicGlobalRestrictionTransactionBodyBuilder: ISerializer {

        /* Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction.. */
        public UnresolvedMosaicIdDto mosaicId;
        /* Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`.. */
        public UnresolvedMosaicIdDto referenceMosaicId;
        /* Restriction key relative to the reference mosaic identifier.. */
        public long restrictionKey;
        /* Previous restriction value.. */
        public long previousRestrictionValue;
        /* New restriction value.. */
        public long newRestrictionValue;
        /* Previous restriction type.. */
        public MosaicRestrictionTypeDto previousRestrictionType;
        /* New restriction type.. */
        public MosaicRestrictionTypeDto newRestrictionType;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicGlobalRestrictionTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                mosaicId = UnresolvedMosaicIdDto.LoadFromBinary(stream);
                referenceMosaicId = UnresolvedMosaicIdDto.LoadFromBinary(stream);
                restrictionKey = stream.ReadInt64();
                previousRestrictionValue = stream.ReadInt64();
                newRestrictionValue = stream.ReadInt64();
                previousRestrictionType = (MosaicRestrictionTypeDto)Enum.ToObject(typeof(MosaicRestrictionTypeDto), (byte)stream.ReadByte());
                newRestrictionType = (MosaicRestrictionTypeDto)Enum.ToObject(typeof(MosaicRestrictionTypeDto), (byte)stream.ReadByte());
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicGlobalRestrictionTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicGlobalRestrictionTransactionBodyBuilder.
        */
        public static MosaicGlobalRestrictionTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicGlobalRestrictionTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param mosaicId Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        * @param referenceMosaicId Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        * @param restrictionKey Restriction key relative to the reference mosaic identifier..
        * @param previousRestrictionValue Previous restriction value..
        * @param newRestrictionValue New restriction value..
        * @param previousRestrictionType Previous restriction type..
        * @param newRestrictionType New restriction type..
        */
        internal MosaicGlobalRestrictionTransactionBodyBuilder(UnresolvedMosaicIdDto mosaicId, UnresolvedMosaicIdDto referenceMosaicId, long restrictionKey, long previousRestrictionValue, long newRestrictionValue, MosaicRestrictionTypeDto previousRestrictionType, MosaicRestrictionTypeDto newRestrictionType)
        {
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(referenceMosaicId, "referenceMosaicId is null");
            GeneratorUtils.NotNull(restrictionKey, "restrictionKey is null");
            GeneratorUtils.NotNull(previousRestrictionValue, "previousRestrictionValue is null");
            GeneratorUtils.NotNull(newRestrictionValue, "newRestrictionValue is null");
            GeneratorUtils.NotNull(previousRestrictionType, "previousRestrictionType is null");
            GeneratorUtils.NotNull(newRestrictionType, "newRestrictionType is null");
            this.mosaicId = mosaicId;
            this.referenceMosaicId = referenceMosaicId;
            this.restrictionKey = restrictionKey;
            this.previousRestrictionValue = previousRestrictionValue;
            this.newRestrictionValue = newRestrictionValue;
            this.previousRestrictionType = previousRestrictionType;
            this.newRestrictionType = newRestrictionType;
        }
        
        /*
        * Creates an instance of MosaicGlobalRestrictionTransactionBodyBuilder.
        *
        * @param mosaicId Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        * @param referenceMosaicId Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        * @param restrictionKey Restriction key relative to the reference mosaic identifier..
        * @param previousRestrictionValue Previous restriction value..
        * @param newRestrictionValue New restriction value..
        * @param previousRestrictionType Previous restriction type..
        * @param newRestrictionType New restriction type..
        * @return Instance of MosaicGlobalRestrictionTransactionBodyBuilder.
        */
        public static  MosaicGlobalRestrictionTransactionBodyBuilder Create(UnresolvedMosaicIdDto mosaicId, UnresolvedMosaicIdDto referenceMosaicId, long restrictionKey, long previousRestrictionValue, long newRestrictionValue, MosaicRestrictionTypeDto previousRestrictionType, MosaicRestrictionTypeDto newRestrictionType) {
            return new MosaicGlobalRestrictionTransactionBodyBuilder(mosaicId, referenceMosaicId, restrictionKey, previousRestrictionValue, newRestrictionValue, previousRestrictionType, newRestrictionType);
        }

        /*
        * Gets Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        *
        * @return Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        */
        public UnresolvedMosaicIdDto GetMosaicId() {
            return mosaicId;
        }

        /*
        * Gets Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        *
        * @return Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        */
        public UnresolvedMosaicIdDto GetReferenceMosaicId() {
            return referenceMosaicId;
        }

        /*
        * Gets Restriction key relative to the reference mosaic identifier..
        *
        * @return Restriction key relative to the reference mosaic identifier..
        */
        public long GetRestrictionKey() {
            return restrictionKey;
        }

        /*
        * Gets Previous restriction value..
        *
        * @return Previous restriction value..
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
        * Gets Previous restriction type..
        *
        * @return Previous restriction type..
        */
        public MosaicRestrictionTypeDto GetPreviousRestrictionType() {
            return previousRestrictionType;
        }

        /*
        * Gets New restriction type..
        *
        * @return New restriction type..
        */
        public MosaicRestrictionTypeDto GetNewRestrictionType() {
            return newRestrictionType;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += mosaicId.GetSize();
            size += referenceMosaicId.GetSize();
            size += 8; // restrictionKey
            size += 8; // previousRestrictionValue
            size += 8; // newRestrictionValue
            size += previousRestrictionType.GetSize();
            size += newRestrictionType.GetSize();
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
            var referenceMosaicIdEntityBytes = (referenceMosaicId).Serialize();
            bw.Write(referenceMosaicIdEntityBytes, 0, referenceMosaicIdEntityBytes.Length);
            bw.Write(GetRestrictionKey());
            bw.Write(GetPreviousRestrictionValue());
            bw.Write(GetNewRestrictionValue());
            var previousRestrictionTypeEntityBytes = (previousRestrictionType).Serialize();
            bw.Write(previousRestrictionTypeEntityBytes, 0, previousRestrictionTypeEntityBytes.Length);
            var newRestrictionTypeEntityBytes = (newRestrictionType).Serialize();
            bw.Write(newRestrictionTypeEntityBytes, 0, newRestrictionTypeEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
