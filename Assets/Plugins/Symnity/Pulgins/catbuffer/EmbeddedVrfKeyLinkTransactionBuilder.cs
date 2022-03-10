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
    * Embedded version of VrfKeyLinkTransaction.
    */
    [Serializable]
    public class EmbeddedVrfKeyLinkTransactionBuilder: EmbeddedTransactionBuilder {

        /* Vrf key link transaction body. */
        public VrfKeyLinkTransactionBodyBuilder vrfKeyLinkTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedVrfKeyLinkTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                vrfKeyLinkTransactionBody = VrfKeyLinkTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedVrfKeyLinkTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedVrfKeyLinkTransactionBuilder.
        */
        public new static EmbeddedVrfKeyLinkTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedVrfKeyLinkTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param linkedPublicKey Linked VRF public key..
        * @param linkAction Account link action..
        */
        internal EmbeddedVrfKeyLinkTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, PublicKeyDto linkedPublicKey, LinkActionDto linkAction)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(linkedPublicKey, "linkedPublicKey is null");
            GeneratorUtils.NotNull(linkAction, "linkAction is null");
            this.vrfKeyLinkTransactionBody = new VrfKeyLinkTransactionBodyBuilder(linkedPublicKey, linkAction);
        }
        
        /*
        * Creates an instance of EmbeddedVrfKeyLinkTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param linkedPublicKey Linked VRF public key..
        * @param linkAction Account link action..
        * @return Instance of EmbeddedVrfKeyLinkTransactionBuilder.
        */
        public static  EmbeddedVrfKeyLinkTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, PublicKeyDto linkedPublicKey, LinkActionDto linkAction) {
            return new EmbeddedVrfKeyLinkTransactionBuilder(signerPublicKey, version, network, type, linkedPublicKey, linkAction);
        }

        /*
        * Gets Linked VRF public key..
        *
        * @return Linked VRF public key..
        */
        public PublicKeyDto GetLinkedPublicKey() {
            return vrfKeyLinkTransactionBody.GetLinkedPublicKey();
        }

        /*
        * Gets Account link action..
        *
        * @return Account link action..
        */
        public LinkActionDto GetLinkAction() {
            return vrfKeyLinkTransactionBody.GetLinkAction();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += vrfKeyLinkTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new VrfKeyLinkTransactionBodyBuilder GetBody() {
            return vrfKeyLinkTransactionBody;
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
            var vrfKeyLinkTransactionBodyEntityBytes = (vrfKeyLinkTransactionBody).Serialize();
            bw.Write(vrfKeyLinkTransactionBodyEntityBytes, 0, vrfKeyLinkTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
