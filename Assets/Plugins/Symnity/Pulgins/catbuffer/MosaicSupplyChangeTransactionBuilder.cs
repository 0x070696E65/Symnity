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
    * Change the total supply of a mosaic.
    */
    [Serializable]
    public class MosaicSupplyChangeTransactionBuilder: TransactionBuilder {

        /* Mosaic supply change transaction body. */
        public MosaicSupplyChangeTransactionBodyBuilder mosaicSupplyChangeTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicSupplyChangeTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaicSupplyChangeTransactionBody = MosaicSupplyChangeTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicSupplyChangeTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicSupplyChangeTransactionBuilder.
        */
        public new static MosaicSupplyChangeTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicSupplyChangeTransactionBuilder(stream);
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
        * @param mosaicId Affected mosaic identifier..
        * @param delta Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        * @param action Supply change action..
        */
        internal MosaicSupplyChangeTransactionBuilder(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, UnresolvedMosaicIdDto mosaicId, AmountDto delta, MosaicSupplyChangeActionDto action)
            : base(signature, signerPublicKey, version, network, type, fee, deadline)
        {
            GeneratorUtils.NotNull(signature, "signature is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(fee, "fee is null");
            GeneratorUtils.NotNull(deadline, "deadline is null");
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(delta, "delta is null");
            GeneratorUtils.NotNull(action, "action is null");
            this.mosaicSupplyChangeTransactionBody = new MosaicSupplyChangeTransactionBodyBuilder(mosaicId, delta, action);
        }
        
        /*
        * Creates an instance of MosaicSupplyChangeTransactionBuilder.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param mosaicId Affected mosaic identifier..
        * @param delta Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        * @param action Supply change action..
        * @return Instance of MosaicSupplyChangeTransactionBuilder.
        */
        public static  MosaicSupplyChangeTransactionBuilder Create(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, UnresolvedMosaicIdDto mosaicId, AmountDto delta, MosaicSupplyChangeActionDto action) {
            return new MosaicSupplyChangeTransactionBuilder(signature, signerPublicKey, version, network, type, fee, deadline, mosaicId, delta, action);
        }

        /*
        * Gets Affected mosaic identifier..
        *
        * @return Affected mosaic identifier..
        */
        public UnresolvedMosaicIdDto GetMosaicId() {
            return mosaicSupplyChangeTransactionBody.GetMosaicId();
        }

        /*
        * Gets Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        *
        * @return Change amount. It cannot be negative, use the `action` field to indicate if this amount should be **added** or **subtracted** from the current supply..
        */
        public AmountDto GetDelta() {
            return mosaicSupplyChangeTransactionBody.GetDelta();
        }

        /*
        * Gets Supply change action..
        *
        * @return Supply change action..
        */
        public MosaicSupplyChangeActionDto GetAction() {
            return mosaicSupplyChangeTransactionBody.GetAction();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += mosaicSupplyChangeTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new MosaicSupplyChangeTransactionBodyBuilder GetBody() {
            return mosaicSupplyChangeTransactionBody;
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
            var mosaicSupplyChangeTransactionBodyEntityBytes = (mosaicSupplyChangeTransactionBody).Serialize();
            bw.Write(mosaicSupplyChangeTransactionBodyEntityBytes, 0, mosaicSupplyChangeTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
