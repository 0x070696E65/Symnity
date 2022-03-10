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
    * Associate a key-value state ([metadata](/concepts/metadata.html)) to a **namespace**.
Compare to AccountMetadataTransaction and MosaicMetadataTransaction.
    */
    [Serializable]
    public class NamespaceMetadataTransactionBuilder: TransactionBuilder {

        /* Namespace metadata transaction body. */
        public NamespaceMetadataTransactionBodyBuilder namespaceMetadataTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal NamespaceMetadataTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                namespaceMetadataTransactionBody = NamespaceMetadataTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of NamespaceMetadataTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NamespaceMetadataTransactionBuilder.
        */
        public new static NamespaceMetadataTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new NamespaceMetadataTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param targetAddress Account owning the namespace whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param targetNamespaceId Namespace whose metadata should be modified..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        internal NamespaceMetadataTransactionBuilder(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, UnresolvedAddressDto targetAddress, long scopedMetadataKey, NamespaceIdDto targetNamespaceId, short valueSizeDelta, byte[] value)
            : base(signature, signerPublicKey, version, network, type, fee, deadline)
        {
            GeneratorUtils.NotNull(signature, "signature is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(fee, "fee is null");
            GeneratorUtils.NotNull(deadline, "deadline is null");
            GeneratorUtils.NotNull(targetAddress, "targetAddress is null");
            GeneratorUtils.NotNull(scopedMetadataKey, "scopedMetadataKey is null");
            GeneratorUtils.NotNull(targetNamespaceId, "targetNamespaceId is null");
            GeneratorUtils.NotNull(valueSizeDelta, "valueSizeDelta is null");
            GeneratorUtils.NotNull(value, "value is null");
            this.namespaceMetadataTransactionBody = new NamespaceMetadataTransactionBodyBuilder(targetAddress, scopedMetadataKey, targetNamespaceId, valueSizeDelta, value);
        }
        
        /*
        * Creates an instance of NamespaceMetadataTransactionBuilder.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param targetAddress Account owning the namespace whose metadata should be modified..
        * @param scopedMetadataKey Metadata key scoped to source, target and type..
        * @param targetNamespaceId Namespace whose metadata should be modified..
        * @param valueSizeDelta Change in value size in bytes, compared to previous size..
        * @param value Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        * @return Instance of NamespaceMetadataTransactionBuilder.
        */
        public static  NamespaceMetadataTransactionBuilder Create(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, UnresolvedAddressDto targetAddress, long scopedMetadataKey, NamespaceIdDto targetNamespaceId, short valueSizeDelta, byte[] value) {
            return new NamespaceMetadataTransactionBuilder(signature, signerPublicKey, version, network, type, fee, deadline, targetAddress, scopedMetadataKey, targetNamespaceId, valueSizeDelta, value);
        }

        /*
        * Gets Account owning the namespace whose metadata should be modified..
        *
        * @return Account owning the namespace whose metadata should be modified..
        */
        public UnresolvedAddressDto GetTargetAddress() {
            return namespaceMetadataTransactionBody.GetTargetAddress();
        }

        /*
        * Gets Metadata key scoped to source, target and type..
        *
        * @return Metadata key scoped to source, target and type..
        */
        public long GetScopedMetadataKey() {
            return namespaceMetadataTransactionBody.GetScopedMetadataKey();
        }

        /*
        * Gets Namespace whose metadata should be modified..
        *
        * @return Namespace whose metadata should be modified..
        */
        public NamespaceIdDto GetTargetNamespaceId() {
            return namespaceMetadataTransactionBody.GetTargetNamespaceId();
        }

        /*
        * Gets Change in value size in bytes, compared to previous size..
        *
        * @return Change in value size in bytes, compared to previous size..
        */
        public short GetValueSizeDelta() {
            return namespaceMetadataTransactionBody.GetValueSizeDelta();
        }

        /*
        * Gets Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        *
        * @return Difference between existing value and new value. \note When there is no existing value, this array is directly used and `value_size_delta`==`value_size`. \note When there is an existing value, the new value is the byte-wise XOR of the previous value and this array..
        */
        public byte[] GetValue() {
            return namespaceMetadataTransactionBody.GetValue();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += namespaceMetadataTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new NamespaceMetadataTransactionBodyBuilder GetBody() {
            return namespaceMetadataTransactionBody;
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
            var namespaceMetadataTransactionBodyEntityBytes = (namespaceMetadataTransactionBody).Serialize();
            bw.Write(namespaceMetadataTransactionBodyEntityBytes, 0, namespaceMetadataTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}