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
    * Shared content between AddressAliasTransaction and EmbeddedAddressAliasTransaction.
    */
    [Serializable]
    public class AddressAliasTransactionBodyBuilder: ISerializer {

        /* Identifier of the namespace that will become (or stop being) an alias for the address.. */
        public NamespaceIdDto namespaceId;
        /* Aliased address.. */
        public AddressDto address;
        /* Alias action.. */
        public AliasActionDto aliasAction;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AddressAliasTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                namespaceId = NamespaceIdDto.LoadFromBinary(stream);
                address = AddressDto.LoadFromBinary(stream);
                aliasAction = (AliasActionDto)Enum.ToObject(typeof(AliasActionDto), (byte)stream.ReadByte());
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AddressAliasTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AddressAliasTransactionBodyBuilder.
        */
        public static AddressAliasTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new AddressAliasTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param namespaceId Identifier of the namespace that will become (or stop being) an alias for the address..
        * @param address Aliased address..
        * @param aliasAction Alias action..
        */
        internal AddressAliasTransactionBodyBuilder(NamespaceIdDto namespaceId, AddressDto address, AliasActionDto aliasAction)
        {
            GeneratorUtils.NotNull(namespaceId, "namespaceId is null");
            GeneratorUtils.NotNull(address, "address is null");
            GeneratorUtils.NotNull(aliasAction, "aliasAction is null");
            this.namespaceId = namespaceId;
            this.address = address;
            this.aliasAction = aliasAction;
        }
        
        /*
        * Creates an instance of AddressAliasTransactionBodyBuilder.
        *
        * @param namespaceId Identifier of the namespace that will become (or stop being) an alias for the address..
        * @param address Aliased address..
        * @param aliasAction Alias action..
        * @return Instance of AddressAliasTransactionBodyBuilder.
        */
        public static  AddressAliasTransactionBodyBuilder Create(NamespaceIdDto namespaceId, AddressDto address, AliasActionDto aliasAction) {
            return new AddressAliasTransactionBodyBuilder(namespaceId, address, aliasAction);
        }

        /*
        * Gets Identifier of the namespace that will become (or stop being) an alias for the address..
        *
        * @return Identifier of the namespace that will become (or stop being) an alias for the address..
        */
        public NamespaceIdDto GetNamespaceId() {
            return namespaceId;
        }

        /*
        * Gets Aliased address..
        *
        * @return Aliased address..
        */
        public AddressDto GetAddress() {
            return address;
        }

        /*
        * Gets Alias action..
        *
        * @return Alias action..
        */
        public AliasActionDto GetAliasAction() {
            return aliasAction;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += namespaceId.GetSize();
            size += address.GetSize();
            size += aliasAction.GetSize();
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
            var namespaceIdEntityBytes = (namespaceId).Serialize();
            bw.Write(namespaceIdEntityBytes, 0, namespaceIdEntityBytes.Length);
            var addressEntityBytes = (address).Serialize();
            bw.Write(addressEntityBytes, 0, addressEntityBytes.Length);
            var aliasActionEntityBytes = (aliasAction).Serialize();
            bw.Write(aliasActionEntityBytes, 0, aliasActionEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
