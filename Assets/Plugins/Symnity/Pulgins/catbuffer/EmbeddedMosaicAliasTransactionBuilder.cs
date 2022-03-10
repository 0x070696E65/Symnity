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
    * Embedded version of MosaicAliasTransaction
    */
    [Serializable]
    public class EmbeddedMosaicAliasTransactionBuilder: EmbeddedTransactionBuilder {

        /* Mosaic alias transaction body. */
        public MosaicAliasTransactionBodyBuilder mosaicAliasTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedMosaicAliasTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaicAliasTransactionBody = MosaicAliasTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedMosaicAliasTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedMosaicAliasTransactionBuilder.
        */
        public new static EmbeddedMosaicAliasTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedMosaicAliasTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param namespaceId Identifier of the namespace that will become (or stop being) an alias for the Mosaic..
        * @param mosaicId Aliased mosaic identifier..
        * @param aliasAction Alias action..
        */
        internal EmbeddedMosaicAliasTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, NamespaceIdDto namespaceId, MosaicIdDto mosaicId, AliasActionDto aliasAction)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(namespaceId, "namespaceId is null");
            GeneratorUtils.NotNull(mosaicId, "mosaicId is null");
            GeneratorUtils.NotNull(aliasAction, "aliasAction is null");
            this.mosaicAliasTransactionBody = new MosaicAliasTransactionBodyBuilder(namespaceId, mosaicId, aliasAction);
        }
        
        /*
        * Creates an instance of EmbeddedMosaicAliasTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param namespaceId Identifier of the namespace that will become (or stop being) an alias for the Mosaic..
        * @param mosaicId Aliased mosaic identifier..
        * @param aliasAction Alias action..
        * @return Instance of EmbeddedMosaicAliasTransactionBuilder.
        */
        public static  EmbeddedMosaicAliasTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, NamespaceIdDto namespaceId, MosaicIdDto mosaicId, AliasActionDto aliasAction) {
            return new EmbeddedMosaicAliasTransactionBuilder(signerPublicKey, version, network, type, namespaceId, mosaicId, aliasAction);
        }

        /*
        * Gets Identifier of the namespace that will become (or stop being) an alias for the Mosaic..
        *
        * @return Identifier of the namespace that will become (or stop being) an alias for the Mosaic..
        */
        public NamespaceIdDto GetNamespaceId() {
            return mosaicAliasTransactionBody.GetNamespaceId();
        }

        /*
        * Gets Aliased mosaic identifier..
        *
        * @return Aliased mosaic identifier..
        */
        public MosaicIdDto GetMosaicId() {
            return mosaicAliasTransactionBody.GetMosaicId();
        }

        /*
        * Gets Alias action..
        *
        * @return Alias action..
        */
        public AliasActionDto GetAliasAction() {
            return mosaicAliasTransactionBody.GetAliasAction();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += mosaicAliasTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new MosaicAliasTransactionBodyBuilder GetBody() {
            return mosaicAliasTransactionBody;
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
            var mosaicAliasTransactionBodyEntityBytes = (mosaicAliasTransactionBody).Serialize();
            bw.Write(mosaicAliasTransactionBodyEntityBytes, 0, mosaicAliasTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
