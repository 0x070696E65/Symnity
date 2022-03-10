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
    * Embedded version of SecretProofTransaction.
    */
    [Serializable]
    public class EmbeddedSecretProofTransactionBuilder: EmbeddedTransactionBuilder {

        /* Secret proof transaction body. */
        public SecretProofTransactionBodyBuilder secretProofTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedSecretProofTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                secretProofTransactionBody = SecretProofTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedSecretProofTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedSecretProofTransactionBuilder.
        */
        public new static EmbeddedSecretProofTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedSecretProofTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param recipientAddress Address that receives the funds once unlocked..
        * @param secret Hashed proof..
        * @param hashAlgorithm Algorithm used to hash the proof..
        * @param proof Original random set of bytes that were hashed..
        */
        internal EmbeddedSecretProofTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto recipientAddress, Hash256Dto secret, LockHashAlgorithmDto hashAlgorithm, byte[] proof)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(recipientAddress, "recipientAddress is null");
            GeneratorUtils.NotNull(secret, "secret is null");
            GeneratorUtils.NotNull(hashAlgorithm, "hashAlgorithm is null");
            GeneratorUtils.NotNull(proof, "proof is null");
            this.secretProofTransactionBody = new SecretProofTransactionBodyBuilder(recipientAddress, secret, hashAlgorithm, proof);
        }
        
        /*
        * Creates an instance of EmbeddedSecretProofTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param recipientAddress Address that receives the funds once unlocked..
        * @param secret Hashed proof..
        * @param hashAlgorithm Algorithm used to hash the proof..
        * @param proof Original random set of bytes that were hashed..
        * @return Instance of EmbeddedSecretProofTransactionBuilder.
        */
        public static  EmbeddedSecretProofTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto recipientAddress, Hash256Dto secret, LockHashAlgorithmDto hashAlgorithm, byte[] proof) {
            return new EmbeddedSecretProofTransactionBuilder(signerPublicKey, version, network, type, recipientAddress, secret, hashAlgorithm, proof);
        }

        /*
        * Gets Address that receives the funds once unlocked..
        *
        * @return Address that receives the funds once unlocked..
        */
        public UnresolvedAddressDto GetRecipientAddress() {
            return secretProofTransactionBody.GetRecipientAddress();
        }

        /*
        * Gets Hashed proof..
        *
        * @return Hashed proof..
        */
        public Hash256Dto GetSecret() {
            return secretProofTransactionBody.GetSecret();
        }

        /*
        * Gets Algorithm used to hash the proof..
        *
        * @return Algorithm used to hash the proof..
        */
        public LockHashAlgorithmDto GetHashAlgorithm() {
            return secretProofTransactionBody.GetHashAlgorithm();
        }

        /*
        * Gets Original random set of bytes that were hashed..
        *
        * @return Original random set of bytes that were hashed..
        */
        public byte[] GetProof() {
            return secretProofTransactionBody.GetProof();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += secretProofTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new SecretProofTransactionBodyBuilder GetBody() {
            return secretProofTransactionBody;
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
            var secretProofTransactionBodyEntityBytes = (secretProofTransactionBody).Serialize();
            bw.Write(secretProofTransactionBodyEntityBytes, 0, secretProofTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
