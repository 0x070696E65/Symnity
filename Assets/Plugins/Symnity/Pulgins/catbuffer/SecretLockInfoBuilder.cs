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
    * Binary layout for serialized lock transaction
    */
    [Serializable]
    public class SecretLockInfoBuilder: StateHeaderBuilder {

        /* Owner address. */
        public AddressDto ownerAddress;
        /* Mosaic associated with lock. */
        public MosaicBuilder mosaic;
        /* Height at which the lock expires. */
        public HeightDto endHeight;
        /* Flag indicating whether or not the lock was already used. */
        public LockStatusDto status;
        /* Hash algorithm. */
        public LockHashAlgorithmDto hashAlgorithm;
        /* Transaction secret. */
        public Hash256Dto secret;
        /* Transaction recipient. */
        public AddressDto recipient;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal SecretLockInfoBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                ownerAddress = AddressDto.LoadFromBinary(stream);
                mosaic = MosaicBuilder.LoadFromBinary(stream);
                endHeight = HeightDto.LoadFromBinary(stream);
                status = (LockStatusDto)Enum.ToObject(typeof(LockStatusDto), (byte)stream.ReadByte());
                hashAlgorithm = (LockHashAlgorithmDto)Enum.ToObject(typeof(LockHashAlgorithmDto), (byte)stream.ReadByte());
                secret = Hash256Dto.LoadFromBinary(stream);
                recipient = AddressDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of SecretLockInfoBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of SecretLockInfoBuilder.
        */
        public new static SecretLockInfoBuilder LoadFromBinary(BinaryReader stream) {
            return new SecretLockInfoBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param ownerAddress Owner address.
        * @param mosaic Mosaic associated with lock.
        * @param endHeight Height at which the lock expires.
        * @param status Flag indicating whether or not the lock was already used.
        * @param hashAlgorithm Hash algorithm.
        * @param secret Transaction secret.
        * @param recipient Transaction recipient.
        */
        internal SecretLockInfoBuilder(short version, AddressDto ownerAddress, MosaicBuilder mosaic, HeightDto endHeight, LockStatusDto status, LockHashAlgorithmDto hashAlgorithm, Hash256Dto secret, AddressDto recipient)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(ownerAddress, "ownerAddress is null");
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            GeneratorUtils.NotNull(endHeight, "endHeight is null");
            GeneratorUtils.NotNull(status, "status is null");
            GeneratorUtils.NotNull(hashAlgorithm, "hashAlgorithm is null");
            GeneratorUtils.NotNull(secret, "secret is null");
            GeneratorUtils.NotNull(recipient, "recipient is null");
            this.ownerAddress = ownerAddress;
            this.mosaic = mosaic;
            this.endHeight = endHeight;
            this.status = status;
            this.hashAlgorithm = hashAlgorithm;
            this.secret = secret;
            this.recipient = recipient;
        }
        
        /*
        * Creates an instance of SecretLockInfoBuilder.
        *
        * @param version Serialization version.
        * @param ownerAddress Owner address.
        * @param mosaic Mosaic associated with lock.
        * @param endHeight Height at which the lock expires.
        * @param status Flag indicating whether or not the lock was already used.
        * @param hashAlgorithm Hash algorithm.
        * @param secret Transaction secret.
        * @param recipient Transaction recipient.
        * @return Instance of SecretLockInfoBuilder.
        */
        public static  SecretLockInfoBuilder Create(short version, AddressDto ownerAddress, MosaicBuilder mosaic, HeightDto endHeight, LockStatusDto status, LockHashAlgorithmDto hashAlgorithm, Hash256Dto secret, AddressDto recipient) {
            return new SecretLockInfoBuilder(version, ownerAddress, mosaic, endHeight, status, hashAlgorithm, secret, recipient);
        }

        /*
        * Gets owner address.
        *
        * @return Owner address.
        */
        public AddressDto GetOwnerAddress() {
            return ownerAddress;
        }

        /*
        * Gets mosaic associated with lock.
        *
        * @return Mosaic associated with lock.
        */
        public MosaicBuilder GetMosaic() {
            return mosaic;
        }

        /*
        * Gets height at which the lock expires.
        *
        * @return Height at which the lock expires.
        */
        public HeightDto GetEndHeight() {
            return endHeight;
        }

        /*
        * Gets flag indicating whether or not the lock was already used.
        *
        * @return Flag indicating whether or not the lock was already used.
        */
        public LockStatusDto GetStatus() {
            return status;
        }

        /*
        * Gets hash algorithm.
        *
        * @return Hash algorithm.
        */
        public LockHashAlgorithmDto GetHashAlgorithm() {
            return hashAlgorithm;
        }

        /*
        * Gets transaction secret.
        *
        * @return Transaction secret.
        */
        public Hash256Dto GetSecret() {
            return secret;
        }

        /*
        * Gets transaction recipient.
        *
        * @return Transaction recipient.
        */
        public AddressDto GetRecipient() {
            return recipient;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += ownerAddress.GetSize();
            size += mosaic.GetSize();
            size += endHeight.GetSize();
            size += status.GetSize();
            size += hashAlgorithm.GetSize();
            size += secret.GetSize();
            size += recipient.GetSize();
            return size;
        }



    
        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public new byte[] Serialize() {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            var superBytes = base.Serialize();
            bw.Write(superBytes, 0, superBytes.Length);
            var ownerAddressEntityBytes = (ownerAddress).Serialize();
            bw.Write(ownerAddressEntityBytes, 0, ownerAddressEntityBytes.Length);
            var mosaicEntityBytes = (mosaic).Serialize();
            bw.Write(mosaicEntityBytes, 0, mosaicEntityBytes.Length);
            var endHeightEntityBytes = (endHeight).Serialize();
            bw.Write(endHeightEntityBytes, 0, endHeightEntityBytes.Length);
            var statusEntityBytes = (status).Serialize();
            bw.Write(statusEntityBytes, 0, statusEntityBytes.Length);
            var hashAlgorithmEntityBytes = (hashAlgorithm).Serialize();
            bw.Write(hashAlgorithmEntityBytes, 0, hashAlgorithmEntityBytes.Length);
            var secretEntityBytes = (secret).Serialize();
            bw.Write(secretEntityBytes, 0, secretEntityBytes.Length);
            var recipientEntityBytes = (recipient).Serialize();
            bw.Write(recipientEntityBytes, 0, recipientEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
