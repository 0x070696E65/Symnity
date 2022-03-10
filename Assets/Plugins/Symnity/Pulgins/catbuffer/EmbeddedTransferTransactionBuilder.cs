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
    * Embedded version of TransferTransaction.
    */
    [Serializable]
    public class EmbeddedTransferTransactionBuilder: EmbeddedTransactionBuilder {

        /* Transfer transaction body. */
        public TransferTransactionBodyBuilder transferTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedTransferTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                transferTransactionBody = TransferTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedTransferTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedTransferTransactionBuilder.
        */
        public new static EmbeddedTransferTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedTransferTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param recipientAddress Recipient address.
        * @param mosaics Attached mosaics.
        * @param message Attached message.
        */
        internal EmbeddedTransferTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto recipientAddress, List<UnresolvedMosaicBuilder> mosaics, byte[] message)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(recipientAddress, "recipientAddress is null");
            GeneratorUtils.NotNull(mosaics, "mosaics is null");
            GeneratorUtils.NotNull(message, "message is null");
            this.transferTransactionBody = new TransferTransactionBodyBuilder(recipientAddress, mosaics, message);
        }
        
        /*
        * Creates an instance of EmbeddedTransferTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param recipientAddress Recipient address.
        * @param mosaics Attached mosaics.
        * @param message Attached message.
        * @return Instance of EmbeddedTransferTransactionBuilder.
        */
        public static  EmbeddedTransferTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto recipientAddress, List<UnresolvedMosaicBuilder> mosaics, byte[] message) {
            return new EmbeddedTransferTransactionBuilder(signerPublicKey, version, network, type, recipientAddress, mosaics, message);
        }

        /*
        * Gets recipient address.
        *
        * @return Recipient address.
        */
        public UnresolvedAddressDto GetRecipientAddress() {
            return transferTransactionBody.GetRecipientAddress();
        }

        /*
        * Gets attached mosaics.
        *
        * @return Attached mosaics.
        */
        public List<UnresolvedMosaicBuilder> GetMosaics() {
            return transferTransactionBody.GetMosaics();
        }

        /*
        * Gets attached message.
        *
        * @return Attached message.
        */
        public byte[] GetMessage() {
            return transferTransactionBody.GetMessage();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += transferTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new TransferTransactionBodyBuilder GetBody() {
            return transferTransactionBody;
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
            var transferTransactionBodyEntityBytes = (transferTransactionBody).Serialize();
            bw.Write(transferTransactionBodyEntityBytes, 0, transferTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
