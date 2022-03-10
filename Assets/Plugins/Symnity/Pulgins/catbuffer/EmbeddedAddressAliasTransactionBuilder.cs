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
    * Embedded version of AddressAliasTransaction.
    */
    [Serializable]
    public class EmbeddedAddressAliasTransactionBuilder: EmbeddedTransactionBuilder {

        /* Address alias transaction body. */
        public AddressAliasTransactionBodyBuilder addressAliasTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedAddressAliasTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                addressAliasTransactionBody = AddressAliasTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedAddressAliasTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedAddressAliasTransactionBuilder.
        */
        public new static EmbeddedAddressAliasTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedAddressAliasTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param namespaceId Identifier of the namespace that will become (or stop being) an alias for the address..
        * @param address Aliased address..
        * @param aliasAction Alias action..
        */
        internal EmbeddedAddressAliasTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, NamespaceIdDto namespaceId, AddressDto address, AliasActionDto aliasAction)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(namespaceId, "namespaceId is null");
            GeneratorUtils.NotNull(address, "address is null");
            GeneratorUtils.NotNull(aliasAction, "aliasAction is null");
            this.addressAliasTransactionBody = new AddressAliasTransactionBodyBuilder(namespaceId, address, aliasAction);
        }
        
        /*
        * Creates an instance of EmbeddedAddressAliasTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param namespaceId Identifier of the namespace that will become (or stop being) an alias for the address..
        * @param address Aliased address..
        * @param aliasAction Alias action..
        * @return Instance of EmbeddedAddressAliasTransactionBuilder.
        */
        public static  EmbeddedAddressAliasTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, NamespaceIdDto namespaceId, AddressDto address, AliasActionDto aliasAction) {
            return new EmbeddedAddressAliasTransactionBuilder(signerPublicKey, version, network, type, namespaceId, address, aliasAction);
        }

        /*
        * Gets Identifier of the namespace that will become (or stop being) an alias for the address..
        *
        * @return Identifier of the namespace that will become (or stop being) an alias for the address..
        */
        public NamespaceIdDto GetNamespaceId() {
            return addressAliasTransactionBody.GetNamespaceId();
        }

        /*
        * Gets Aliased address..
        *
        * @return Aliased address..
        */
        public AddressDto GetAddress() {
            return addressAliasTransactionBody.GetAddress();
        }

        /*
        * Gets Alias action..
        *
        * @return Alias action..
        */
        public AliasActionDto GetAliasAction() {
            return addressAliasTransactionBody.GetAliasAction();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += addressAliasTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new AddressAliasTransactionBodyBuilder GetBody() {
            return addressAliasTransactionBody;
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
            var addressAliasTransactionBodyEntityBytes = (addressAliasTransactionBody).Serialize();
            bw.Write(addressAliasTransactionBodyEntityBytes, 0, addressAliasTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
