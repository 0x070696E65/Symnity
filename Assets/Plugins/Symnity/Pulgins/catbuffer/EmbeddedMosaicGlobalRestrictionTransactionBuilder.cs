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
    * Embedded version of MosaicGlobalRestrictionTransaction.
    */
    [Serializable]
    public class EmbeddedMosaicGlobalRestrictionTransactionBuilder: EmbeddedTransactionBuilder {

        /* Mosaic global restriction transaction body. */
        public MosaicGlobalRestrictionTransactionBodyBuilder mosaicGlobalRestrictionTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedMosaicGlobalRestrictionTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaicGlobalRestrictionTransactionBody = MosaicGlobalRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedMosaicGlobalRestrictionTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedMosaicGlobalRestrictionTransactionBuilder.
        */
        public new static EmbeddedMosaicGlobalRestrictionTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedMosaicGlobalRestrictionTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param mosaicId Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        * @param referenceMosaicId Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        * @param restrictionKey Restriction key relative to the reference mosaic identifier..
        * @param previousRestrictionValue Previous restriction value..
        * @param newRestrictionValue New restriction value..
        * @param previousRestrictionType Previous restriction type..
        * @param newRestrictionType New restriction type..
        */
        internal EmbeddedMosaicGlobalRestrictionTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedMosaicIdDto mosaicId, UnresolvedMosaicIdDto referenceMosaicId, long restrictionKey, long previousRestrictionValue, long newRestrictionValue, MosaicRestrictionTypeDto previousRestrictionType, MosaicRestrictionTypeDto newRestrictionType)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(referenceMosaicId, "referenceMosaicId is null");
            GeneratorUtils.NotNull(restrictionKey, "restrictionKey is null");
            GeneratorUtils.NotNull(previousRestrictionValue, "previousRestrictionValue is null");
            GeneratorUtils.NotNull(newRestrictionValue, "newRestrictionValue is null");
            GeneratorUtils.NotNull(previousRestrictionType, "previousRestrictionType is null");
            GeneratorUtils.NotNull(newRestrictionType, "newRestrictionType is null");
            this.mosaicGlobalRestrictionTransactionBody = new MosaicGlobalRestrictionTransactionBodyBuilder(mosaicId, referenceMosaicId, restrictionKey, previousRestrictionValue, newRestrictionValue, previousRestrictionType, newRestrictionType);
        }
        
        /*
        * Creates an instance of EmbeddedMosaicGlobalRestrictionTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param mosaicId Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        * @param referenceMosaicId Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        * @param restrictionKey Restriction key relative to the reference mosaic identifier..
        * @param previousRestrictionValue Previous restriction value..
        * @param newRestrictionValue New restriction value..
        * @param previousRestrictionType Previous restriction type..
        * @param newRestrictionType New restriction type..
        * @return Instance of EmbeddedMosaicGlobalRestrictionTransactionBuilder.
        */
        public static  EmbeddedMosaicGlobalRestrictionTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedMosaicIdDto mosaicId, UnresolvedMosaicIdDto referenceMosaicId, long restrictionKey, long previousRestrictionValue, long newRestrictionValue, MosaicRestrictionTypeDto previousRestrictionType, MosaicRestrictionTypeDto newRestrictionType) {
            return new EmbeddedMosaicGlobalRestrictionTransactionBuilder(signerPublicKey, version, network, type, mosaicId, referenceMosaicId, restrictionKey, previousRestrictionValue, newRestrictionValue, previousRestrictionType, newRestrictionType);
        }

        /*
        * Gets Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        *
        * @return Identifier of the mosaic being restricted. The mosaic creator must be the signer of the transaction..
        */
        public UnresolvedMosaicIdDto GetMosaicId() {
            return mosaicGlobalRestrictionTransactionBody.GetMosaicId();
        }

        /*
        * Gets Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        *
        * @return Identifier of the mosaic providing the restriction key. The mosaic global restriction for the mosaic identifier depends on global restrictions set on the reference mosaic. Set `reference_mosaic_id` to **0** if the mosaic giving the restriction equals the `mosaic_id`..
        */
        public UnresolvedMosaicIdDto GetReferenceMosaicId() {
            return mosaicGlobalRestrictionTransactionBody.GetReferenceMosaicId();
        }

        /*
        * Gets Restriction key relative to the reference mosaic identifier..
        *
        * @return Restriction key relative to the reference mosaic identifier..
        */
        public long GetRestrictionKey() {
            return mosaicGlobalRestrictionTransactionBody.GetRestrictionKey();
        }

        /*
        * Gets Previous restriction value..
        *
        * @return Previous restriction value..
        */
        public long GetPreviousRestrictionValue() {
            return mosaicGlobalRestrictionTransactionBody.GetPreviousRestrictionValue();
        }

        /*
        * Gets New restriction value..
        *
        * @return New restriction value..
        */
        public long GetNewRestrictionValue() {
            return mosaicGlobalRestrictionTransactionBody.GetNewRestrictionValue();
        }

        /*
        * Gets Previous restriction type..
        *
        * @return Previous restriction type..
        */
        public MosaicRestrictionTypeDto GetPreviousRestrictionType() {
            return mosaicGlobalRestrictionTransactionBody.GetPreviousRestrictionType();
        }

        /*
        * Gets New restriction type..
        *
        * @return New restriction type..
        */
        public MosaicRestrictionTypeDto GetNewRestrictionType() {
            return mosaicGlobalRestrictionTransactionBody.GetNewRestrictionType();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += mosaicGlobalRestrictionTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new MosaicGlobalRestrictionTransactionBodyBuilder GetBody() {
            return mosaicGlobalRestrictionTransactionBody;
        }


    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public override byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var superBytes = base.Serialize();
            bw.Write(superBytes, 0, superBytes.Length);
            var mosaicGlobalRestrictionTransactionBodyEntityBytes = (mosaicGlobalRestrictionTransactionBody).Serialize();
            bw.Write(mosaicGlobalRestrictionTransactionBodyEntityBytes, 0, mosaicGlobalRestrictionTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
