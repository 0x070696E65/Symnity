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
    * Binary layout for an embedded transaction
    */
    [Serializable]
    public class EmbeddedTransactionBuilder: ISerializer {

        /* Entity size in bytes.
This size includes the header and the full payload of the entity. I.e, the size field matches the size reported in the structure documentation (plus the variable part, if there is any).. */
        public int size;
        /* Reserved padding to align end of EmbeddedTransactionHeader on 8-byte boundary. */
        public int embeddedTransactionHeaderReserved1;
        /* Public key of the signer of the entity.. */
        public PublicKeyDto signerPublicKey;
        /* Reserved padding to align end of EntityBody to an 8-byte boundary.. */
        public int entityBodyReserved1;
        /* Version of this structure.. */
        public byte version;
        /* Network on which this entity was created.. */
        public NetworkTypeDto network;
        /* Transaction type. */
        public TransactionTypeDto type;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedTransactionBuilder(BinaryReader stream)
        {
            try {
                size = stream.ReadInt32();
                embeddedTransactionHeaderReserved1 = stream.ReadInt32();
                signerPublicKey = PublicKeyDto.LoadFromBinary(stream);
                entityBodyReserved1 = stream.ReadInt32();
                version = stream.ReadByte();
                network = (NetworkTypeDto)Enum.ToObject(typeof(NetworkTypeDto), (byte)stream.ReadByte());
                type = (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), (short)stream.ReadInt16());
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedTransactionBuilder.
        */
        public static EmbeddedTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        */
        internal EmbeddedTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            this.embeddedTransactionHeaderReserved1 = 0;
            this.signerPublicKey = signerPublicKey;
            this.entityBodyReserved1 = 0;
            this.version = version;
            this.network = network;
            this.type = type;
        }
        
        /*
        * Creates an instance of EmbeddedTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @return Instance of EmbeddedTransactionBuilder.
        */
        public static  EmbeddedTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type) {
            return new EmbeddedTransactionBuilder(signerPublicKey, version, network, type);
        }

        /*
        * Gets Entity size in bytes.
This size includes the header and the full payload of the entity. I.e, the size field matches the size reported in the structure documentation (plus the variable part, if there is any)..
        *
        * @return Entity size in bytes.
This size includes the header and the full payload of the entity. I.e, the size field matches the size reported in the structure documentation (plus the variable part, if there is any)..
        */
        public int GetStreamSize() {
            return size;
        }

        /*
        * Gets reserved padding to align end of EmbeddedTransactionHeader on 8-byte boundary.
        *
        * @return Reserved padding to align end of EmbeddedTransactionHeader on 8-byte boundary.
        */
        private int GetEmbeddedTransactionHeaderReserved1() {
            return embeddedTransactionHeaderReserved1;
        }

        /*
        * Gets Public key of the signer of the entity..
        *
        * @return Public key of the signer of the entity..
        */
        public PublicKeyDto GetSignerPublicKey() {
            return signerPublicKey;
        }

        /*
        * Gets Reserved padding to align end of EntityBody to an 8-byte boundary..
        *
        * @return Reserved padding to align end of EntityBody to an 8-byte boundary..
        */
        private int GetEntityBodyReserved1() {
            return entityBodyReserved1;
        }

        /*
        * Gets Version of this structure..
        *
        * @return Version of this structure..
        */
        public byte GetVersion() {
            return version;
        }

        /*
        * Gets Network on which this entity was created..
        *
        * @return Network on which this entity was created..
        */
        public NetworkTypeDto GetNetwork() {
            return network;
        }

        /*
        * Gets transaction type.
        *
        * @return Transaction type.
        */
        public new TransactionTypeDto GetType() {
            return type;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public virtual int GetSize() {
            var size = 0;
            size += 4; // size
            size += 4; // embeddedTransactionHeaderReserved1
            size += signerPublicKey.GetSize();
            size += 4; // entityBodyReserved1
            size += 1; // version
            size += network.GetSize();
            size += type.GetSize();
            return size;
        }


        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public ISerializer GetBody() {
            return null;
        }

    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public virtual byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write((int)GetSize());
            bw.Write(GetEmbeddedTransactionHeaderReserved1());
            var signerPublicKeyEntityBytes = (signerPublicKey).Serialize();
            bw.Write(signerPublicKeyEntityBytes, 0, signerPublicKeyEntityBytes.Length);
            bw.Write(GetEntityBodyReserved1());
            bw.Write(GetVersion());
            var networkEntityBytes = (network).Serialize();
            bw.Write(networkEntityBytes, 0, networkEntityBytes.Length);
            var typeEntityBytes = (type).Serialize();
            bw.Write(typeEntityBytes, 0, typeEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
