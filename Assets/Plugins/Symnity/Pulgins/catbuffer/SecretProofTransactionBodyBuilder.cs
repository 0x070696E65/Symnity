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
    * Shared content between SecretProofTransaction and EmbeddedSecretProofTransaction.
    */
    [Serializable]
    public class SecretProofTransactionBodyBuilder: ISerializer {

        /* Address that receives the funds once unlocked.. */
        public UnresolvedAddressDto recipientAddress;
        /* Hashed proof.. */
        public Hash256Dto secret;
        /* Algorithm used to hash the proof.. */
        public LockHashAlgorithmDto hashAlgorithm;
        /* Original random set of bytes that were hashed.. */
        public byte[] proof;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal SecretProofTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                recipientAddress = UnresolvedAddressDto.LoadFromBinary(stream);
                secret = Hash256Dto.LoadFromBinary(stream);
                var proofSize = stream.ReadInt16();
                hashAlgorithm = (LockHashAlgorithmDto)Enum.ToObject(typeof(LockHashAlgorithmDto), (byte)stream.ReadByte());
                proof = GeneratorUtils.ReadBytes(stream, proofSize);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of SecretProofTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of SecretProofTransactionBodyBuilder.
        */
        public static SecretProofTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new SecretProofTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param recipientAddress Address that receives the funds once unlocked..
        * @param secret Hashed proof..
        * @param hashAlgorithm Algorithm used to hash the proof..
        * @param proof Original random set of bytes that were hashed..
        */
        internal SecretProofTransactionBodyBuilder(UnresolvedAddressDto recipientAddress, Hash256Dto secret, LockHashAlgorithmDto hashAlgorithm, byte[] proof)
        {
            GeneratorUtils.NotNull(recipientAddress, "recipientAddress is null");
            GeneratorUtils.NotNull(secret, "secret is null");
            GeneratorUtils.NotNull(hashAlgorithm, "hashAlgorithm is null");
            GeneratorUtils.NotNull(proof, "proof is null");
            this.recipientAddress = recipientAddress;
            this.secret = secret;
            this.hashAlgorithm = hashAlgorithm;
            this.proof = proof;
        }
        
        /*
        * Creates an instance of SecretProofTransactionBodyBuilder.
        *
        * @param recipientAddress Address that receives the funds once unlocked..
        * @param secret Hashed proof..
        * @param hashAlgorithm Algorithm used to hash the proof..
        * @param proof Original random set of bytes that were hashed..
        * @return Instance of SecretProofTransactionBodyBuilder.
        */
        public static  SecretProofTransactionBodyBuilder Create(UnresolvedAddressDto recipientAddress, Hash256Dto secret, LockHashAlgorithmDto hashAlgorithm, byte[] proof) {
            return new SecretProofTransactionBodyBuilder(recipientAddress, secret, hashAlgorithm, proof);
        }

        /*
        * Gets Address that receives the funds once unlocked..
        *
        * @return Address that receives the funds once unlocked..
        */
        public UnresolvedAddressDto GetRecipientAddress() {
            return recipientAddress;
        }

        /*
        * Gets Hashed proof..
        *
        * @return Hashed proof..
        */
        public Hash256Dto GetSecret() {
            return secret;
        }

        /*
        * Gets Algorithm used to hash the proof..
        *
        * @return Algorithm used to hash the proof..
        */
        public LockHashAlgorithmDto GetHashAlgorithm() {
            return hashAlgorithm;
        }

        /*
        * Gets Original random set of bytes that were hashed..
        *
        * @return Original random set of bytes that were hashed..
        */
        public byte[] GetProof() {
            return proof;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += recipientAddress.GetSize();
            size += secret.GetSize();
            size += 2; // proofSize
            size += hashAlgorithm.GetSize();
            size += proof.Length;
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
            var recipientAddressEntityBytes = (recipientAddress).Serialize();
            bw.Write(recipientAddressEntityBytes, 0, recipientAddressEntityBytes.Length);
            var secretEntityBytes = (secret).Serialize();
            bw.Write(secretEntityBytes, 0, secretEntityBytes.Length);
            bw.Write((short)GeneratorUtils.GetSize(GetProof()));
            var hashAlgorithmEntityBytes = (hashAlgorithm).Serialize();
            bw.Write(hashAlgorithmEntityBytes, 0, hashAlgorithmEntityBytes.Length);
            bw.Write(proof, 0, proof.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
