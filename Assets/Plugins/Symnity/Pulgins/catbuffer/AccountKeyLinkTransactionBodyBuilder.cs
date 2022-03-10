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
    * Shared content between AccountKeyLinkTransactionBody and EmbeddedAccountKeyLinkTransaction.
    */
    [Serializable]
    public class AccountKeyLinkTransactionBodyBuilder: ISerializer {

        /* Linked public key.. */
        public PublicKeyDto linkedPublicKey;
        /* Account link action.. */
        public LinkActionDto linkAction;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountKeyLinkTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                linkedPublicKey = PublicKeyDto.LoadFromBinary(stream);
                linkAction = (LinkActionDto)Enum.ToObject(typeof(LinkActionDto), (byte)stream.ReadByte());
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountKeyLinkTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountKeyLinkTransactionBodyBuilder.
        */
        public static AccountKeyLinkTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountKeyLinkTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param linkedPublicKey Linked public key..
        * @param linkAction Account link action..
        */
        internal AccountKeyLinkTransactionBodyBuilder(PublicKeyDto linkedPublicKey, LinkActionDto linkAction)
        {
            GeneratorUtils.NotNull(linkedPublicKey, "linkedPublicKey is null");
            GeneratorUtils.NotNull(linkAction, "linkAction is null");
            this.linkedPublicKey = linkedPublicKey;
            this.linkAction = linkAction;
        }
        
        /*
        * Creates an instance of AccountKeyLinkTransactionBodyBuilder.
        *
        * @param linkedPublicKey Linked public key..
        * @param linkAction Account link action..
        * @return Instance of AccountKeyLinkTransactionBodyBuilder.
        */
        public static  AccountKeyLinkTransactionBodyBuilder Create(PublicKeyDto linkedPublicKey, LinkActionDto linkAction) {
            return new AccountKeyLinkTransactionBodyBuilder(linkedPublicKey, linkAction);
        }

        /*
        * Gets Linked public key..
        *
        * @return Linked public key..
        */
        public PublicKeyDto GetLinkedPublicKey() {
            return linkedPublicKey;
        }

        /*
        * Gets Account link action..
        *
        * @return Account link action..
        */
        public LinkActionDto GetLinkAction() {
            return linkAction;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += linkedPublicKey.GetSize();
            size += linkAction.GetSize();
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
            var linkedPublicKeyEntityBytes = (linkedPublicKey).Serialize();
            bw.Write(linkedPublicKeyEntityBytes, 0, linkedPublicKeyEntityBytes.Length);
            var linkActionEntityBytes = (linkAction).Serialize();
            bw.Write(linkActionEntityBytes, 0, linkActionEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
