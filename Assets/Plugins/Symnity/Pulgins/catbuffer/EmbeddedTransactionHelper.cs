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

/* Factory in charge of creating the right transaction builder from the streamed data. */

namespace Symbol.Builders {
    [Serializable]
    public class EmbeddedTransactionHelper {

        /* Deserialize an embedded transaction builder from binary */
        public static EmbeddedTransactionBuilder LoadFromBinary(BinaryReader stream) {

            var header = EmbeddedTransactionBuilder.LoadFromBinary(stream);

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            if (header.type == TransactionTypeDto.ACCOUNT_KEY_LINK && header.version == 1) {
                AccountKeyLinkTransactionBodyBuilder body = AccountKeyLinkTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedAccountKeyLinkTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.NODE_KEY_LINK && header.version == 1) {
                NodeKeyLinkTransactionBodyBuilder body = NodeKeyLinkTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedNodeKeyLinkTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                                                            if (header.type == TransactionTypeDto.VOTING_KEY_LINK && header.version == 1) {
                VotingKeyLinkTransactionBodyBuilder body = VotingKeyLinkTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedVotingKeyLinkTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.VRF_KEY_LINK && header.version == 1) {
                VrfKeyLinkTransactionBodyBuilder body = VrfKeyLinkTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedVrfKeyLinkTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.HASH_LOCK && header.version == 1) {
                HashLockTransactionBodyBuilder body = HashLockTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedHashLockTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.SECRET_LOCK && header.version == 1) {
                SecretLockTransactionBodyBuilder body = SecretLockTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedSecretLockTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.SECRET_PROOF && header.version == 1) {
                SecretProofTransactionBodyBuilder body = SecretProofTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedSecretProofTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.ACCOUNT_METADATA && header.version == 1) {
                AccountMetadataTransactionBodyBuilder body = AccountMetadataTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedAccountMetadataTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_METADATA && header.version == 1) {
                MosaicMetadataTransactionBodyBuilder body = MosaicMetadataTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicMetadataTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.NAMESPACE_METADATA && header.version == 1) {
                NamespaceMetadataTransactionBodyBuilder body = NamespaceMetadataTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedNamespaceMetadataTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_DEFINITION && header.version == 1) {
                MosaicDefinitionTransactionBodyBuilder body = MosaicDefinitionTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicDefinitionTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_SUPPLY_CHANGE && header.version == 1) {
                MosaicSupplyChangeTransactionBodyBuilder body = MosaicSupplyChangeTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicSupplyChangeTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_SUPPLY_REVOCATION && header.version == 1) {
                MosaicSupplyRevocationTransactionBodyBuilder body = MosaicSupplyRevocationTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicSupplyRevocationTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MULTISIG_ACCOUNT_MODIFICATION && header.version == 1) {
                MultisigAccountModificationTransactionBodyBuilder body = MultisigAccountModificationTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMultisigAccountModificationTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.ADDRESS_ALIAS && header.version == 1) {
                AddressAliasTransactionBodyBuilder body = AddressAliasTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedAddressAliasTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_ALIAS && header.version == 1) {
                MosaicAliasTransactionBodyBuilder body = MosaicAliasTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicAliasTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.NAMESPACE_REGISTRATION && header.version == 1) {
                NamespaceRegistrationTransactionBodyBuilder body = NamespaceRegistrationTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedNamespaceRegistrationTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.ACCOUNT_ADDRESS_RESTRICTION && header.version == 1) {
                AccountAddressRestrictionTransactionBodyBuilder body = AccountAddressRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedAccountAddressRestrictionTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.ACCOUNT_MOSAIC_RESTRICTION && header.version == 1) {
                AccountMosaicRestrictionTransactionBodyBuilder body = AccountMosaicRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedAccountMosaicRestrictionTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.ACCOUNT_OPERATION_RESTRICTION && header.version == 1) {
                AccountOperationRestrictionTransactionBodyBuilder body = AccountOperationRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedAccountOperationRestrictionTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_ADDRESS_RESTRICTION && header.version == 1) {
                MosaicAddressRestrictionTransactionBodyBuilder body = MosaicAddressRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicAddressRestrictionTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.MOSAIC_GLOBAL_RESTRICTION && header.version == 1) {
                MosaicGlobalRestrictionTransactionBodyBuilder body = MosaicGlobalRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedMosaicGlobalRestrictionTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
                                    if (header.type == TransactionTypeDto.TRANSFER && header.version == 1) {
                TransferTransactionBodyBuilder body = TransferTransactionBodyBuilder.LoadFromBinary(stream);
                byte[] headerSerialized = header.Serialize();
                byte[] bodySerialized = body.Serialize();
                byte[] newArray = new byte[headerSerialized.Length + bodySerialized.Length];
                headerSerialized.CopyTo(newArray, 0);
                bodySerialized.CopyTo(newArray, headerSerialized.Length);
                MemoryStream ms = new MemoryStream(newArray);
                return EmbeddedTransferTransactionBuilder.LoadFromBinary(new BinaryReader(ms));
            }
            return header;
        }
    }
}
