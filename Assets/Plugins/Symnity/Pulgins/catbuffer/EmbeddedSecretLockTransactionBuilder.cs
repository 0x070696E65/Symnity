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
    * Embedded version of SecretLockTransaction.
    */
    [Serializable]
    public class EmbeddedSecretLockTransactionBuilder: EmbeddedTransactionBuilder {

        /* Secret lock transaction body. */
        public SecretLockTransactionBodyBuilder secretLockTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedSecretLockTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                secretLockTransactionBody = SecretLockTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedSecretLockTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedSecretLockTransactionBuilder.
        */
        public new static EmbeddedSecretLockTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedSecretLockTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param recipientAddress Address that receives the funds once successfully unlocked by a SecretProofTransaction..
        * @param secret Hashed proof..
        * @param mosaic Locked mosaics..
        * @param duration Number of blocks to wait for the SecretProofTransaction..
        * @param hashAlgorithm Algorithm used to hash the proof..
        */
        internal EmbeddedSecretLockTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto recipientAddress, Hash256Dto secret, UnresolvedMosaicBuilder mosaic, BlockDurationDto duration, LockHashAlgorithmDto hashAlgorithm)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(recipientAddress, "recipientAddress is null");
            GeneratorUtils.NotNull(secret, "secret is null");
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            GeneratorUtils.NotNull(duration, "duration is null");
            GeneratorUtils.NotNull(hashAlgorithm, "hashAlgorithm is null");
            this.secretLockTransactionBody = new SecretLockTransactionBodyBuilder(recipientAddress, secret, mosaic, duration, hashAlgorithm);
        }
        
        /*
        * Creates an instance of EmbeddedSecretLockTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param recipientAddress Address that receives the funds once successfully unlocked by a SecretProofTransaction..
        * @param secret Hashed proof..
        * @param mosaic Locked mosaics..
        * @param duration Number of blocks to wait for the SecretProofTransaction..
        * @param hashAlgorithm Algorithm used to hash the proof..
        * @return Instance of EmbeddedSecretLockTransactionBuilder.
        */
        public static  EmbeddedSecretLockTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, UnresolvedAddressDto recipientAddress, Hash256Dto secret, UnresolvedMosaicBuilder mosaic, BlockDurationDto duration, LockHashAlgorithmDto hashAlgorithm) {
            return new EmbeddedSecretLockTransactionBuilder(signerPublicKey, version, network, type, recipientAddress, secret, mosaic, duration, hashAlgorithm);
        }

        /*
        * Gets Address that receives the funds once successfully unlocked by a SecretProofTransaction..
        *
        * @return Address that receives the funds once successfully unlocked by a SecretProofTransaction..
        */
        public UnresolvedAddressDto GetRecipientAddress() {
            return secretLockTransactionBody.GetRecipientAddress();
        }

        /*
        * Gets Hashed proof..
        *
        * @return Hashed proof..
        */
        public Hash256Dto GetSecret() {
            return secretLockTransactionBody.GetSecret();
        }

        /*
        * Gets Locked mosaics..
        *
        * @return Locked mosaics..
        */
        public UnresolvedMosaicBuilder GetMosaic() {
            return secretLockTransactionBody.GetMosaic();
        }

        /*
        * Gets Number of blocks to wait for the SecretProofTransaction..
        *
        * @return Number of blocks to wait for the SecretProofTransaction..
        */
        public BlockDurationDto GetDuration() {
            return secretLockTransactionBody.GetDuration();
        }

        /*
        * Gets Algorithm used to hash the proof..
        *
        * @return Algorithm used to hash the proof..
        */
        public LockHashAlgorithmDto GetHashAlgorithm() {
            return secretLockTransactionBody.GetHashAlgorithm();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += secretLockTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new SecretLockTransactionBodyBuilder GetBody() {
            return secretLockTransactionBody;
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
            var secretLockTransactionBodyEntityBytes = (secretLockTransactionBody).Serialize();
            bw.Write(secretLockTransactionBodyEntityBytes, 0, secretLockTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
