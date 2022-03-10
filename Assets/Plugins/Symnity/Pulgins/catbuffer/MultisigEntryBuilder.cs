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
    * Binary layout for a multisig entry
    */
    [Serializable]
    public class MultisigEntryBuilder: StateHeaderBuilder {

        /* Minimum approval for modifications. */
        public int minApproval;
        /* Minimum approval for removal. */
        public int minRemoval;
        /* Account address. */
        public AddressDto accountAddress;
        /* Cosignatories for account. */
        public List<AddressDto> cosignatoryAddresses;
        /* Accounts for which the entry is cosignatory. */
        public List<AddressDto> multisigAddresses;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MultisigEntryBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                minApproval = stream.ReadInt32();
                minRemoval = stream.ReadInt32();
                accountAddress = AddressDto.LoadFromBinary(stream);
                var cosignatoryAddressesCount = stream.ReadInt64();
                cosignatoryAddresses = GeneratorUtils.LoadFromBinaryArray(AddressDto.LoadFromBinary, stream, cosignatoryAddressesCount, 0);
                var multisigAddressesCount = stream.ReadInt64();
                multisigAddresses = GeneratorUtils.LoadFromBinaryArray(AddressDto.LoadFromBinary, stream, multisigAddressesCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MultisigEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MultisigEntryBuilder.
        */
        public new static MultisigEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MultisigEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param minApproval Minimum approval for modifications.
        * @param minRemoval Minimum approval for removal.
        * @param accountAddress Account address.
        * @param cosignatoryAddresses Cosignatories for account.
        * @param multisigAddresses Accounts for which the entry is cosignatory.
        */
        internal MultisigEntryBuilder(short version, int minApproval, int minRemoval, AddressDto accountAddress, List<AddressDto> cosignatoryAddresses, List<AddressDto> multisigAddresses)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(minApproval, "minApproval is null");
            GeneratorUtils.NotNull(minRemoval, "minRemoval is null");
            GeneratorUtils.NotNull(accountAddress, "accountAddress is null");
            GeneratorUtils.NotNull(cosignatoryAddresses, "cosignatoryAddresses is null");
            GeneratorUtils.NotNull(multisigAddresses, "multisigAddresses is null");
            this.minApproval = minApproval;
            this.minRemoval = minRemoval;
            this.accountAddress = accountAddress;
            this.cosignatoryAddresses = cosignatoryAddresses;
            this.multisigAddresses = multisigAddresses;
        }
        
        /*
        * Creates an instance of MultisigEntryBuilder.
        *
        * @param version Serialization version.
        * @param minApproval Minimum approval for modifications.
        * @param minRemoval Minimum approval for removal.
        * @param accountAddress Account address.
        * @param cosignatoryAddresses Cosignatories for account.
        * @param multisigAddresses Accounts for which the entry is cosignatory.
        * @return Instance of MultisigEntryBuilder.
        */
        public static  MultisigEntryBuilder Create(short version, int minApproval, int minRemoval, AddressDto accountAddress, List<AddressDto> cosignatoryAddresses, List<AddressDto> multisigAddresses) {
            return new MultisigEntryBuilder(version, minApproval, minRemoval, accountAddress, cosignatoryAddresses, multisigAddresses);
        }

        /*
        * Gets minimum approval for modifications.
        *
        * @return Minimum approval for modifications.
        */
        public int GetMinApproval() {
            return minApproval;
        }

        /*
        * Gets minimum approval for removal.
        *
        * @return Minimum approval for removal.
        */
        public int GetMinRemoval() {
            return minRemoval;
        }

        /*
        * Gets account address.
        *
        * @return Account address.
        */
        public AddressDto GetAccountAddress() {
            return accountAddress;
        }

        /*
        * Gets cosignatories for account.
        *
        * @return Cosignatories for account.
        */
        public List<AddressDto> GetCosignatoryAddresses() {
            return cosignatoryAddresses;
        }

        /*
        * Gets accounts for which the entry is cosignatory.
        *
        * @return Accounts for which the entry is cosignatory.
        */
        public List<AddressDto> GetMultisigAddresses() {
            return multisigAddresses;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += 4; // minApproval
            size += 4; // minRemoval
            size += accountAddress.GetSize();
            size += 8; // cosignatoryAddressesCount
            size +=  GeneratorUtils.GetSumSize(cosignatoryAddresses, 0);
            size += 8; // multisigAddressesCount
            size +=  GeneratorUtils.GetSumSize(multisigAddresses, 0);
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
            bw.Write(GetMinApproval());
            bw.Write(GetMinRemoval());
            var accountAddressEntityBytes = (accountAddress).Serialize();
            bw.Write(accountAddressEntityBytes, 0, accountAddressEntityBytes.Length);
            bw.Write((long)GeneratorUtils.GetSize(GetCosignatoryAddresses()));
            cosignatoryAddresses.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            bw.Write((long)GeneratorUtils.GetSize(GetMultisigAddresses()));
            multisigAddresses.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            var result = ms.ToArray();
            return result;
        }
    }
}
