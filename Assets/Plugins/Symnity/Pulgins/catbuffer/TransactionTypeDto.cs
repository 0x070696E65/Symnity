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

/*
* Enumeration of Transaction types
*/

namespace Symbol.Builders {

    [Serializable]
    public enum TransactionTypeDto {
        /* AccountKeyLinkTransaction. */
        ACCOUNT_KEY_LINK = 16716,
        /* NodeKeyLinkTransaction. */
        NODE_KEY_LINK = 16972,
        /* AggregateCompleteTransaction. */
        AGGREGATE_COMPLETE = 16705,
        /* AggregateBondedTransaction. */
        AGGREGATE_BONDED = 16961,
        /* VotingKeyLinkTransaction. */
        VOTING_KEY_LINK = 16707,
        /* VrfKeyLinkTransaction. */
        VRF_KEY_LINK = 16963,
        /* HashLockTransaction. */
        HASH_LOCK = 16712,
        /* SecretLockTransaction. */
        SECRET_LOCK = 16722,
        /* SecretProofTransaction. */
        SECRET_PROOF = 16978,
        /* AccountMetadataTransaction. */
        ACCOUNT_METADATA = 16708,
        /* MosaicMetadataTransaction. */
        MOSAIC_METADATA = 16964,
        /* NamespaceMetadataTransaction. */
        NAMESPACE_METADATA = 17220,
        /* MosaicDefinitionTransaction. */
        MOSAIC_DEFINITION = 16717,
        /* MosaicSupplyChangeTransaction. */
        MOSAIC_SUPPLY_CHANGE = 16973,
        /* MosaicSupplyRevocationTransaction. */
        MOSAIC_SUPPLY_REVOCATION = 17229,
        /* MultisigAccountModificationTransaction. */
        MULTISIG_ACCOUNT_MODIFICATION = 16725,
        /* AddressAliasTransaction. */
        ADDRESS_ALIAS = 16974,
        /* MosaicAliasTransaction. */
        MOSAIC_ALIAS = 17230,
        /* NamespaceRegistrationTransaction. */
        NAMESPACE_REGISTRATION = 16718,
        /* AccountAddressRestrictionTransaction. */
        ACCOUNT_ADDRESS_RESTRICTION = 16720,
        /* AccountMosaicRestrictionTransaction. */
        ACCOUNT_MOSAIC_RESTRICTION = 16976,
        /* AccountOperationRestrictionTransaction. */
        ACCOUNT_OPERATION_RESTRICTION = 17232,
        /* MosaicAddressRestrictionTransaction. */
        MOSAIC_ADDRESS_RESTRICTION = 16977,
        /* MosaicGlobalRestrictionTransaction. */
        MOSAIC_GLOBAL_RESTRICTION = 16721,
        /* TransferTransaction. */
        TRANSFER = 16724,
    }
    
    public static class TransactionTypeDtoExtensions
    {
        /* Enum value. */
        private static short value(this TransactionTypeDto self) {
            return (short)self;
        }

        /*
        * Gets enum value.
        *
        * @param value Raw value of the enum.
        * @return Enum value.
        */
        public static TransactionTypeDto RawValueOf(this TransactionTypeDto self, short value) {
            return (TransactionTypeDto)Enum.ToObject(typeof(TransactionTypeDto), value);
            throw new Exception(value + " was not a backing value for TransactionTypeDto.");
        }

        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public static int GetSize(this TransactionTypeDto type)
        {
            return 2;
        }

        /*
        * Creates an instance of TransactionTypeDto from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of TransactionTypeDto.
        */
        public static TransactionTypeDto LoadFromBinary(this TransactionTypeDto self, BinaryReader stream) {
            try {
                short streamValue = stream.ReadInt16();
                return RawValueOf(self, streamValue);
            } catch(Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Serializes an object to bytes.
        *
        * @return Serialized bytes.
        */
        public static byte[] Serialize(this TransactionTypeDto self) {
            var ms = new MemoryStream();
            var bw = new BinaryWriter(ms);
            bw.Write(self.value());
            var result = ms.ToArray();
            return result;
        }
    }
}
