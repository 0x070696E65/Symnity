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
    * Embedded version of MosaicSupplyRevocationTransaction.
    */
    [Serializable]
    public class EmbeddedMosaicSupplyRevocationTransactionBuilder: EmbeddedTransactionBuilder {

        /* Mosaic supply revocation transaction body. */
        public MosaicSupplyRevocationTransactionBodyBuilder mosaicSupplyRevocationTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedMosaicSupplyRevocationTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaicSupplyRevocationTransactionBody = MosaicSupplyRevocationTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedMosaicSupplyRevocationTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedMosaicSupplyRevocationTransactionBuilder.
        */
        public new static EmbeddedMosaicSupplyRevocationTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedMosaicSupplyRevocationTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param sourceAddress Address from which tokens should be revoked..
        * @param mosaic Revoked mosaic and amount..
        */
        internal EmbeddedMosaicSupplyRevocationTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto sourceAddress, UnresolvedMosaicBuilder mosaic)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(sourceAddress, "sourceAddress is null");
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            this.mosaicSupplyRevocationTransactionBody = new MosaicSupplyRevocationTransactionBodyBuilder(sourceAddress, mosaic);
        }
        
        /*
        * Creates an instance of EmbeddedMosaicSupplyRevocationTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param sourceAddress Address from which tokens should be revoked..
        * @param mosaic Revoked mosaic and amount..
        * @return Instance of EmbeddedMosaicSupplyRevocationTransactionBuilder.
        */
        public static  EmbeddedMosaicSupplyRevocationTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto sourceAddress, UnresolvedMosaicBuilder mosaic) {
            return new EmbeddedMosaicSupplyRevocationTransactionBuilder(signerPublicKey, version, network, type, sourceAddress, mosaic);
        }

        /*
        * Gets Address from which tokens should be revoked..
        *
        * @return Address from which tokens should be revoked..
        */
        public UnresolvedAddressDto GetSourceAddress() {
            return mosaicSupplyRevocationTransactionBody.GetSourceAddress();
        }

        /*
        * Gets Revoked mosaic and amount..
        *
        * @return Revoked mosaic and amount..
        */
        public UnresolvedMosaicBuilder GetMosaic() {
            return mosaicSupplyRevocationTransactionBody.GetMosaic();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += mosaicSupplyRevocationTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new MosaicSupplyRevocationTransactionBodyBuilder GetBody() {
            return mosaicSupplyRevocationTransactionBody;
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
            var mosaicSupplyRevocationTransactionBodyEntityBytes = (mosaicSupplyRevocationTransactionBody).Serialize();
            bw.Write(mosaicSupplyRevocationTransactionBodyEntityBytes, 0, mosaicSupplyRevocationTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
