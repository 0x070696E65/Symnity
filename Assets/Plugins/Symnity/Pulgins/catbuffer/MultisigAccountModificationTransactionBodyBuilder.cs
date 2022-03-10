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
    * Shared content between MultisigAccountModificationTransaction and EmbeddedMultisigAccountModificationTransaction.
    */
    [Serializable]
    public class MultisigAccountModificationTransactionBodyBuilder: ISerializer {

        /* Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**.. */
        public byte minRemovalDelta;
        /* Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**.. */
        public byte minApprovalDelta;
        /* Reserved padding to align addressAdditions to an 8-byte boundary.. */
        public int multisigAccountModificationTransactionBodyReserved1;
        /* Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values.. */
        public List<UnresolvedAddressDto> addressAdditions;
        /* Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value.. */
        public List<UnresolvedAddressDto> addressDeletions;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MultisigAccountModificationTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                minRemovalDelta = stream.ReadByte();
                minApprovalDelta = stream.ReadByte();
                var addressAdditionsCount = stream.ReadByte();
                var addressDeletionsCount = stream.ReadByte();
                multisigAccountModificationTransactionBodyReserved1 = stream.ReadInt32();
                addressAdditions = GeneratorUtils.LoadFromBinaryArray(UnresolvedAddressDto.LoadFromBinary, stream, addressAdditionsCount, 0);
                addressDeletions = GeneratorUtils.LoadFromBinaryArray(UnresolvedAddressDto.LoadFromBinary, stream, addressDeletionsCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MultisigAccountModificationTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MultisigAccountModificationTransactionBodyBuilder.
        */
        public static MultisigAccountModificationTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MultisigAccountModificationTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param minRemovalDelta Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param minApprovalDelta Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param addressAdditions Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        * @param addressDeletions Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        */
        internal MultisigAccountModificationTransactionBodyBuilder(byte minRemovalDelta, byte minApprovalDelta, List<UnresolvedAddressDto> addressAdditions, List<UnresolvedAddressDto> addressDeletions)
        {
            GeneratorUtils.NotNull(minRemovalDelta, "minRemovalDelta is null");
            GeneratorUtils.NotNull(minApprovalDelta, "minApprovalDelta is null");
            GeneratorUtils.NotNull(addressAdditions, "addressAdditions is null");
            GeneratorUtils.NotNull(addressDeletions, "addressDeletions is null");
            this.minRemovalDelta = minRemovalDelta;
            this.minApprovalDelta = minApprovalDelta;
            this.multisigAccountModificationTransactionBodyReserved1 = 0;
            this.addressAdditions = addressAdditions;
            this.addressDeletions = addressDeletions;
        }
        
        /*
        * Creates an instance of MultisigAccountModificationTransactionBodyBuilder.
        *
        * @param minRemovalDelta Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param minApprovalDelta Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param addressAdditions Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        * @param addressDeletions Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        * @return Instance of MultisigAccountModificationTransactionBodyBuilder.
        */
        public static  MultisigAccountModificationTransactionBodyBuilder Create(byte minRemovalDelta, byte minApprovalDelta, List<UnresolvedAddressDto> addressAdditions, List<UnresolvedAddressDto> addressDeletions) {
            return new MultisigAccountModificationTransactionBodyBuilder(minRemovalDelta, minApprovalDelta, addressAdditions, addressDeletions);
        }

        /*
        * Gets Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        *
        * @return Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        */
        public byte GetMinRemovalDelta() {
            return minRemovalDelta;
        }

        /*
        * Gets Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        *
        * @return Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        */
        public byte GetMinApprovalDelta() {
            return minApprovalDelta;
        }

        /*
        * Gets Reserved padding to align addressAdditions to an 8-byte boundary..
        *
        * @return Reserved padding to align addressAdditions to an 8-byte boundary..
        */
        private int GetMultisigAccountModificationTransactionBodyReserved1() {
            return multisigAccountModificationTransactionBodyReserved1;
        }

        /*
        * Gets Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        *
        * @return Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        */
        public List<UnresolvedAddressDto> GetAddressAdditions() {
            return addressAdditions;
        }

        /*
        * Gets Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        *
        * @return Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        */
        public List<UnresolvedAddressDto> GetAddressDeletions() {
            return addressDeletions;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 1; // minRemovalDelta
            size += 1; // minApprovalDelta
            size += 1; // addressAdditionsCount
            size += 1; // addressDeletionsCount
            size += 4; // multisigAccountModificationTransactionBodyReserved1
            size +=  GeneratorUtils.GetSumSize(addressAdditions, 0);
            size +=  GeneratorUtils.GetSumSize(addressDeletions, 0);
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
            bw.Write(GetMinRemovalDelta());
            bw.Write(GetMinApprovalDelta());
            bw.Write((byte)GeneratorUtils.GetSize(GetAddressAdditions()));
            bw.Write((byte)GeneratorUtils.GetSize(GetAddressDeletions()));
            bw.Write(GetMultisigAccountModificationTransactionBodyReserved1());
            addressAdditions.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            addressDeletions.ForEach(entity =>
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
