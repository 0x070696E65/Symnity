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
    * Binary layout for non-historical account state
    */
    [Serializable]
    public class AccountStateBuilder: StateHeaderBuilder {

        /* Address of account. */
        public AddressDto address;
        /* Height at which address has been obtained. */
        public HeightDto addressHeight;
        /* Public key of account. */
        public PublicKeyDto publicKey;
        /* Height at which public key has been obtained. */
        public HeightDto publicKeyHeight;
        /* Type of account. */
        public AccountTypeDto accountType;
        /* Account format. */
        public AccountStateFormatDto format;
        /* Mask of supplemental public key flags. */
        public List<AccountKeyTypeFlagsDto> supplementalPublicKeysMask;
        /* Linked account public key. */
        public PublicKeyDto? linkedPublicKey;
        /* Node public key. */
        public PublicKeyDto? nodePublicKey;
        /* Vrf public key. */
        public PublicKeyDto? vrfPublicKey;
        /* Voting public keys. */
        public List<PinnedVotingKeyBuilder> votingPublicKeys;
        /* Current importance snapshot of the account. */
        public ImportanceSnapshotBuilder importanceSnapshots;
        /* Activity buckets of the account. */
        public HeightActivityBucketsBuilder activityBuckets;
        /* Balances of account. */
        public List<MosaicBuilder> balances;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountStateBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                address = AddressDto.LoadFromBinary(stream);
                addressHeight = HeightDto.LoadFromBinary(stream);
                publicKey = PublicKeyDto.LoadFromBinary(stream);
                publicKeyHeight = HeightDto.LoadFromBinary(stream);
                accountType = (AccountTypeDto)Enum.ToObject(typeof(AccountTypeDto), (byte)stream.ReadByte());
                format = (AccountStateFormatDto)Enum.ToObject(typeof(AccountStateFormatDto), (byte)stream.ReadByte());
                supplementalPublicKeysMask = GeneratorUtils.ToSet<AccountKeyTypeFlagsDto>(stream.ReadByte());
                var votingPublicKeysCount = stream.ReadByte();
                if (this.supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.LINKED)) {
                    linkedPublicKey = PublicKeyDto.LoadFromBinary(stream);
                }
                if (this.supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.NODE)) {
                    nodePublicKey = PublicKeyDto.LoadFromBinary(stream);
                }
                if (this.supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.VRF)) {
                    vrfPublicKey = PublicKeyDto.LoadFromBinary(stream);
                }
                votingPublicKeys = GeneratorUtils.LoadFromBinaryArray(PinnedVotingKeyBuilder.LoadFromBinary, stream, votingPublicKeysCount, 0);
                if (this.format == AccountStateFormatDto.HIGH_VALUE) {
                    importanceSnapshots = ImportanceSnapshotBuilder.LoadFromBinary(stream);
                }
                if (this.format == AccountStateFormatDto.HIGH_VALUE) {
                    activityBuckets = HeightActivityBucketsBuilder.LoadFromBinary(stream);
                }
                var balancesCount = stream.ReadInt16();
                balances = GeneratorUtils.LoadFromBinaryArray(MosaicBuilder.LoadFromBinary, stream, balancesCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountStateBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountStateBuilder.
        */
        public new static AccountStateBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountStateBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param address Address of account.
        * @param addressHeight Height at which address has been obtained.
        * @param publicKey Public key of account.
        * @param publicKeyHeight Height at which public key has been obtained.
        * @param accountType Type of account.
        * @param format Account format.
        * @param supplementalPublicKeysMask Mask of supplemental public key flags.
        * @param linkedPublicKey Linked account public key.
        * @param nodePublicKey Node public key.
        * @param vrfPublicKey Vrf public key.
        * @param votingPublicKeys Voting public keys.
        * @param importanceSnapshots Current importance snapshot of the account.
        * @param activityBuckets Activity buckets of the account.
        * @param balances Balances of account.
        */
        internal AccountStateBuilder(short version, AddressDto address, HeightDto addressHeight, PublicKeyDto publicKey, HeightDto publicKeyHeight, AccountTypeDto accountType, AccountStateFormatDto format, List<AccountKeyTypeFlagsDto> supplementalPublicKeysMask, PublicKeyDto? linkedPublicKey, PublicKeyDto? nodePublicKey, PublicKeyDto? vrfPublicKey, List<PinnedVotingKeyBuilder> votingPublicKeys, ImportanceSnapshotBuilder importanceSnapshots, HeightActivityBucketsBuilder activityBuckets, List<MosaicBuilder> balances)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(address, "address is null");
            GeneratorUtils.NotNull(addressHeight, "addressHeight is null");
            GeneratorUtils.NotNull(publicKey, "publicKey is null");
            GeneratorUtils.NotNull(publicKeyHeight, "publicKeyHeight is null");
            GeneratorUtils.NotNull(accountType, "accountType is null");
            GeneratorUtils.NotNull(format, "format is null");
            GeneratorUtils.NotNull(supplementalPublicKeysMask, "supplementalPublicKeysMask is null");
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.LINKED)) {
                GeneratorUtils.NotNull(linkedPublicKey, "linkedPublicKey is null");
            }
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.NODE)) {
                GeneratorUtils.NotNull(nodePublicKey, "nodePublicKey is null");
            }
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.VRF)) {
                GeneratorUtils.NotNull(vrfPublicKey, "vrfPublicKey is null");
            }
            GeneratorUtils.NotNull(votingPublicKeys, "votingPublicKeys is null");
            if (format == AccountStateFormatDto.HIGH_VALUE) {
                GeneratorUtils.NotNull(importanceSnapshots, "importanceSnapshots is null");
            }
            if (format == AccountStateFormatDto.HIGH_VALUE) {
                GeneratorUtils.NotNull(activityBuckets, "activityBuckets is null");
            }
            GeneratorUtils.NotNull(balances, "balances is null");
            this.address = address;
            this.addressHeight = addressHeight;
            this.publicKey = publicKey;
            this.publicKeyHeight = publicKeyHeight;
            this.accountType = accountType;
            this.format = format;
            this.supplementalPublicKeysMask = supplementalPublicKeysMask;
            this.linkedPublicKey = linkedPublicKey;
            this.nodePublicKey = nodePublicKey;
            this.vrfPublicKey = vrfPublicKey;
            this.votingPublicKeys = votingPublicKeys;
            this.importanceSnapshots = importanceSnapshots;
            this.activityBuckets = activityBuckets;
            this.balances = balances;
        }
        
        /*
        * Creates an instance of AccountStateBuilder.
        *
        * @param version Serialization version.
        * @param address Address of account.
        * @param addressHeight Height at which address has been obtained.
        * @param publicKey Public key of account.
        * @param publicKeyHeight Height at which public key has been obtained.
        * @param accountType Type of account.
        * @param supplementalPublicKeysMask Mask of supplemental public key flags.
        * @param linkedPublicKey Linked account public key.
        * @param nodePublicKey Node public key.
        * @param vrfPublicKey Vrf public key.
        * @param votingPublicKeys Voting public keys.
        * @param balances Balances of account.
        * @return Instance of AccountStateBuilder.
        */
        public static  AccountStateBuilder CreateREGULAR(short version, AddressDto address, HeightDto addressHeight, PublicKeyDto publicKey, HeightDto publicKeyHeight, AccountTypeDto accountType, List<AccountKeyTypeFlagsDto> supplementalPublicKeysMask, PublicKeyDto linkedPublicKey, PublicKeyDto nodePublicKey, PublicKeyDto vrfPublicKey, List<PinnedVotingKeyBuilder> votingPublicKeys, List<MosaicBuilder> balances) {
            AccountStateFormatDto format = AccountStateFormatDto.REGULAR;
            return new AccountStateBuilder(version, address, addressHeight, publicKey, publicKeyHeight, accountType, format, supplementalPublicKeysMask, linkedPublicKey, nodePublicKey, vrfPublicKey, votingPublicKeys, null, null, balances);
        }
        
        /*
        * Creates an instance of AccountStateBuilder.
        *
        * @param version Serialization version.
        * @param address Address of account.
        * @param addressHeight Height at which address has been obtained.
        * @param publicKey Public key of account.
        * @param publicKeyHeight Height at which public key has been obtained.
        * @param accountType Type of account.
        * @param supplementalPublicKeysMask Mask of supplemental public key flags.
        * @param linkedPublicKey Linked account public key.
        * @param nodePublicKey Node public key.
        * @param vrfPublicKey Vrf public key.
        * @param votingPublicKeys Voting public keys.
        * @param importanceSnapshots Current importance snapshot of the account.
        * @param activityBuckets Activity buckets of the account.
        * @param balances Balances of account.
        * @return Instance of AccountStateBuilder.
        */
        public static  AccountStateBuilder CreateHIGH_VALUE(short version, AddressDto address, HeightDto addressHeight, PublicKeyDto publicKey, HeightDto publicKeyHeight, AccountTypeDto accountType, List<AccountKeyTypeFlagsDto> supplementalPublicKeysMask, PublicKeyDto linkedPublicKey, PublicKeyDto nodePublicKey, PublicKeyDto vrfPublicKey, List<PinnedVotingKeyBuilder> votingPublicKeys, ImportanceSnapshotBuilder importanceSnapshots, HeightActivityBucketsBuilder activityBuckets, List<MosaicBuilder> balances) {
            AccountStateFormatDto format = AccountStateFormatDto.HIGH_VALUE;
            return new AccountStateBuilder(version, address, addressHeight, publicKey, publicKeyHeight, accountType, format, supplementalPublicKeysMask, linkedPublicKey, nodePublicKey, vrfPublicKey, votingPublicKeys, importanceSnapshots, activityBuckets, balances);
        }

        /*
        * Gets address of account.
        *
        * @return Address of account.
        */
        public AddressDto GetAddress() {
            return address;
        }

        /*
        * Gets height at which address has been obtained.
        *
        * @return Height at which address has been obtained.
        */
        public HeightDto GetAddressHeight() {
            return addressHeight;
        }

        /*
        * Gets public key of account.
        *
        * @return Public key of account.
        */
        public PublicKeyDto GetPublicKey() {
            return publicKey;
        }

        /*
        * Gets height at which public key has been obtained.
        *
        * @return Height at which public key has been obtained.
        */
        public HeightDto GetPublicKeyHeight() {
            return publicKeyHeight;
        }

        /*
        * Gets type of account.
        *
        * @return Type of account.
        */
        public AccountTypeDto GetAccountType() {
            return accountType;
        }

        /*
        * Gets account format.
        *
        * @return Account format.
        */
        public AccountStateFormatDto GetFormat() {
            return format;
        }

        /*
        * Gets mask of supplemental public key flags.
        *
        * @return Mask of supplemental public key flags.
        */
        public List<AccountKeyTypeFlagsDto> GetSupplementalPublicKeysMask() {
            return supplementalPublicKeysMask;
        }

        /*
        * Gets linked account public key.
        *
        * @return Linked account public key.
        */
        public PublicKeyDto? GetLinkedPublicKey() {
            if (!(supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.LINKED))) {
                throw new Exception("supplementalPublicKeysMask is not set to LINKED.");
            }
            return linkedPublicKey;
        }

        /*
        * Gets node public key.
        *
        * @return Node public key.
        */
        public PublicKeyDto? GetNodePublicKey() {
            if (!(supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.NODE))) {
                throw new Exception("supplementalPublicKeysMask is not set to NODE.");
            }
            return nodePublicKey;
        }

        /*
        * Gets vrf public key.
        *
        * @return Vrf public key.
        */
        public PublicKeyDto? GetVrfPublicKey() {
            if (!(supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.VRF))) {
                throw new Exception("supplementalPublicKeysMask is not set to VRF.");
            }
            return vrfPublicKey;
        }

        /*
        * Gets voting public keys.
        *
        * @return Voting public keys.
        */
        public List<PinnedVotingKeyBuilder> GetVotingPublicKeys() {
            return votingPublicKeys;
        }

        /*
        * Gets current importance snapshot of the account.
        *
        * @return Current importance snapshot of the account.
        */
        public ImportanceSnapshotBuilder GetImportanceSnapshots() {
            if (!(format == AccountStateFormatDto.HIGH_VALUE)) {
                throw new Exception("format is not set to HIGH_VALUE.");
            }
            return importanceSnapshots;
        }

        /*
        * Gets activity buckets of the account.
        *
        * @return Activity buckets of the account.
        */
        public HeightActivityBucketsBuilder GetActivityBuckets() {
            if (!(format == AccountStateFormatDto.HIGH_VALUE)) {
                throw new Exception("format is not set to HIGH_VALUE.");
            }
            return activityBuckets;
        }

        /*
        * Gets balances of account.
        *
        * @return Balances of account.
        */
        public List<MosaicBuilder> GetBalances() {
            return balances;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += address.GetSize();
            size += addressHeight.GetSize();
            size += publicKey.GetSize();
            size += publicKeyHeight.GetSize();
            size += accountType.GetSize();
            size += format.GetSize();
            size += 1; // supplementalPublicKeysMask
            size += 1; // votingPublicKeysCount
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.LINKED)) {
                if (linkedPublicKey != null) {
                size += ((PublicKeyDto) linkedPublicKey).GetSize();
            }
            }
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.NODE)) {
                if (nodePublicKey != null) {
                size += ((PublicKeyDto) nodePublicKey).GetSize();
            }
            }
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.VRF)) {
                if (vrfPublicKey != null) {
                size += ((PublicKeyDto) vrfPublicKey).GetSize();
            }
            }
            size +=  GeneratorUtils.GetSumSize(votingPublicKeys, 0);
            if (format == AccountStateFormatDto.HIGH_VALUE) {
                if (importanceSnapshots != null) {
                size += ((ImportanceSnapshotBuilder) importanceSnapshots).GetSize();
            }
            }
            if (format == AccountStateFormatDto.HIGH_VALUE) {
                if (activityBuckets != null) {
                size += ((HeightActivityBucketsBuilder) activityBuckets).GetSize();
            }
            }
            size += 2; // balancesCount
            size +=  GeneratorUtils.GetSumSize(balances, 0);
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
            var addressEntityBytes = (address).Serialize();
            bw.Write(addressEntityBytes, 0, addressEntityBytes.Length);
            var addressHeightEntityBytes = (addressHeight).Serialize();
            bw.Write(addressHeightEntityBytes, 0, addressHeightEntityBytes.Length);
            var publicKeyEntityBytes = (publicKey).Serialize();
            bw.Write(publicKeyEntityBytes, 0, publicKeyEntityBytes.Length);
            var publicKeyHeightEntityBytes = (publicKeyHeight).Serialize();
            bw.Write(publicKeyHeightEntityBytes, 0, publicKeyHeightEntityBytes.Length);
            var accountTypeEntityBytes = (accountType).Serialize();
            bw.Write(accountTypeEntityBytes, 0, accountTypeEntityBytes.Length);
            var formatEntityBytes = (format).Serialize();
            bw.Write(formatEntityBytes, 0, formatEntityBytes.Length);
            bw.Write((byte)GeneratorUtils.ToLong(supplementalPublicKeysMask));
            bw.Write((byte)GeneratorUtils.GetSize(GetVotingPublicKeys()));
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.LINKED)) {
                var linkedPublicKeyEntityBytes = ((PublicKeyDto)linkedPublicKey).Serialize();
            bw.Write(linkedPublicKeyEntityBytes, 0, linkedPublicKeyEntityBytes.Length);
            }
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.NODE)) {
                var nodePublicKeyEntityBytes = ((PublicKeyDto)nodePublicKey).Serialize();
            bw.Write(nodePublicKeyEntityBytes, 0, nodePublicKeyEntityBytes.Length);
            }
            if (supplementalPublicKeysMask.Contains(AccountKeyTypeFlagsDto.VRF)) {
                var vrfPublicKeyEntityBytes = ((PublicKeyDto)vrfPublicKey).Serialize();
            bw.Write(vrfPublicKeyEntityBytes, 0, vrfPublicKeyEntityBytes.Length);
            }
            votingPublicKeys.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            if (format == AccountStateFormatDto.HIGH_VALUE) {
                var importanceSnapshotsEntityBytes = (importanceSnapshots).Serialize();
            bw.Write(importanceSnapshotsEntityBytes, 0, importanceSnapshotsEntityBytes.Length);
            }
            if (format == AccountStateFormatDto.HIGH_VALUE) {
                var activityBucketsEntityBytes = (activityBuckets).Serialize();
            bw.Write(activityBucketsEntityBytes, 0, activityBucketsEntityBytes.Length);
            }
            bw.Write((short)GeneratorUtils.GetSize(GetBalances()));
            balances.ForEach(entity =>
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
