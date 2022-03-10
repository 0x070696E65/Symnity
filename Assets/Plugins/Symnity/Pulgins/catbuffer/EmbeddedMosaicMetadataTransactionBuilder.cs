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
    * Embedded version of MosaicMetadataTransaction.
    */
    [Serializable]
    public class EmbeddedMosaicMetadataTransactionBuilder: EmbeddedTransactionBuilder {

        /* Mosaic metadata transaction body. */
        public MosaicMetadataTransactionBodyBuilder mosaicMetadataTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedMosaicMetadataTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaicMetadataTransactionBody = MosaicMetadataTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedMosaicMetadataTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedMosaicMetadataTransactionBuilder.
        */
        public new static EmbeddedMosaicMetadataTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedMosaicMetadataTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param targetAddress Account owning the mosaic whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param targetMosaicId Mosaic whose metadata should be modified..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        internal EmbeddedMosaicMetadataTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto targetAddress, long scopedMetadataKey, UnresolvedMosaicIdDto targetMosaicId, short valueSizeDelta, byte[] value)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            GeneratorUtils.NotNull(scopedMetadataKey, "scopedMetadataKey is null");
            GeneratorUtils.NotNull(targetMosaicId, "targetMosaicId is null");
            GeneratorUtils.NotNull(valueSizeDelta, "valueSizeDelta is null");
            GeneratorUtils.NotNull(value, "value is null");
            this.mosaicMetadataTransactionBody = new MosaicMetadataTransactionBodyBuilder(targetAddress, scopedMetadataKey, targetMosaicId, valueSizeDelta, value);
        }
        
        /*
        * Creates an instance of EmbeddedMosaicMetadataTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param targetAddress Account owning the mosaic whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param targetMosaicId Mosaic whose metadata should be modified..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        * @return Instance of EmbeddedMosaicMetadataTransactionBuilder.
        */
        public static  EmbeddedMosaicMetadataTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto targetAddress, long scopedMetadataKey, UnresolvedMosaicIdDto targetMosaicId, short valueSizeDelta, byte[] value) {
            return new EmbeddedMosaicMetadataTransactionBuilder(signerPublicKey, version, network, type, targetAddress, scopedMetadataKey, targetMosaicId, valueSizeDelta, value);
        }

        /*
        * Gets Account owning the mosaic whose metadata should be modified..
        *
        * @return Account owning the mosaic whose metadata should be modified..
        */
        public UnresolvedAddressDto GetTargetAddress() {
            return mosaicMetadataTransactionBody.GetTargetAddress();
        }

        /*
        * Gets Metadata key scoped to source, target and type..
        *
        * @return Metadata key scoped to source, target and type..
        */
        public long GetScopedMetadataKey() {
            return mosaicMetadataTransactionBody.GetScopedMetadataKey();
        }

        /*
        * Gets Mosaic whose metadata should be modified..
        *
        * @return Mosaic whose metadata should be modified..
        */
        public UnresolvedMosaicIdDto GetTargetMosaicId() {
            return mosaicMetadataTransactionBody.GetTargetMosaicId();
        }

        /*
        * Gets Change in value size in bytes, compared to previous size..
        *
        * @return Change in value size in bytes, compared to previous size..
        */
        public short GetValueSizeDelta() {
            return mosaicMetadataTransactionBody.GetValueSizeDelta();
        }

        /*
        * Gets Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        *
        * @return Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        public byte[] GetValue() {
            return mosaicMetadataTransactionBody.GetValue();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += mosaicMetadataTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new MosaicMetadataTransactionBodyBuilder GetBody() {
            return mosaicMetadataTransactionBody;
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
            var mosaicMetadataTransactionBodyEntityBytes = (mosaicMetadataTransactionBody).Serialize();
            bw.Write(mosaicMetadataTransactionBodyEntityBytes, 0, mosaicMetadataTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
