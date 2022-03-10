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
    * Create or modify a [multi-signature](/concepts/multisig-account.html) account.
This transaction allows you to: - Transform a regular account into a multisig account. - Change the configurable properties of a multisig account. - Add or delete cosignatories from a multisig account (removing all cosignatories turns a multisig account into a regular account again).
    */
    [Serializable]
    public class MultisigAccountModificationTransactionBuilder: TransactionBuilder {

        /* Multisig account modification transaction body. */
        public MultisigAccountModificationTransactionBodyBuilder multisigAccountModificationTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MultisigAccountModificationTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                multisigAccountModificationTransactionBody = MultisigAccountModificationTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MultisigAccountModificationTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MultisigAccountModificationTransactionBuilder.
        */
        public new static MultisigAccountModificationTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new MultisigAccountModificationTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param minRemovalDelta Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param minApprovalDelta Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param addressAdditions Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        * @param addressDeletions Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        */
        internal MultisigAccountModificationTransactionBuilder(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, byte minRemovalDelta, byte minApprovalDelta, List<UnresolvedAddressDto> addressAdditions, List<UnresolvedAddressDto> addressDeletions)
            : base(signature, signerPublicKey, version, network, type, fee, deadline)
        {
            GeneratorUtils.NotNull(signature, "signature is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(fee, "fee is null");
            GeneratorUtils.NotNull(deadline, "deadline is null");
            GeneratorUtils.NotNull(minRemovalDelta, "minRemovalDelta is null");
            GeneratorUtils.NotNull(minApprovalDelta, "minApprovalDelta is null");
            GeneratorUtils.NotNull(addressAdditions, "addressAdditions is null");
            GeneratorUtils.NotNull(addressDeletions, "addressDeletions is null");
            this.multisigAccountModificationTransactionBody = new MultisigAccountModificationTransactionBodyBuilder(minRemovalDelta, minApprovalDelta, addressAdditions, addressDeletions);
        }
        
        /*
        * Creates an instance of MultisigAccountModificationTransactionBuilder.
        *
        * @param signature Entity's signature generated by the signing account..
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param fee Transaction fee.
        * @param deadline Transaction deadline.
        * @param minRemovalDelta Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param minApprovalDelta Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        * @param addressAdditions Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        * @param addressDeletions Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        * @return Instance of MultisigAccountModificationTransactionBuilder.
        */
        public static  MultisigAccountModificationTransactionBuilder Create(SignatureDto signature, PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, AmountDto fee, TimestampDto deadline, byte minRemovalDelta, byte minApprovalDelta, List<UnresolvedAddressDto> addressAdditions, List<UnresolvedAddressDto> addressDeletions) {
            return new MultisigAccountModificationTransactionBuilder(signature, signerPublicKey, version, network, type, fee, deadline, minRemovalDelta, minApprovalDelta, addressAdditions, addressDeletions);
        }

        /*
        * Gets Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        *
        * @return Relative change to the **minimum** number of cosignatures required when **removing a cosignatory**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        */
        public byte GetMinRemovalDelta() {
            return multisigAccountModificationTransactionBody.GetMinRemovalDelta();
        }

        /*
        * Gets Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        *
        * @return Relative change to the **minimum** number of cosignatures required when **approving a transaction**.
E.g., when moving from 0 to 2 cosignatures this number would be **2**. When moving from 4 to 3 cosignatures, the number would be **-1**..
        */
        public byte GetMinApprovalDelta() {
            return multisigAccountModificationTransactionBody.GetMinApprovalDelta();
        }

        /*
        * Gets Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        *
        * @return Cosignatory address additions.
All accounts in this list will be able to cosign transactions on behalf of the multisig account. The number of required cosignatures depends on the configured minimum approval and minimum removal values..
        */
        public List<UnresolvedAddressDto> GetAddressAdditions() {
            return multisigAccountModificationTransactionBody.GetAddressAdditions();
        }

        /*
        * Gets Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        *
        * @return Cosignatory address deletions.
All accounts in this list will stop being able to cosign transactions on behalf of the multisig account. A transaction containing **any** address in this array requires a number of cosignatures at least equal to the minimum removal value..
        */
        public List<UnresolvedAddressDto> GetAddressDeletions() {
            return multisigAccountModificationTransactionBody.GetAddressDeletions();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += multisigAccountModificationTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new MultisigAccountModificationTransactionBodyBuilder GetBody() {
            return multisigAccountModificationTransactionBody;
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
            var multisigAccountModificationTransactionBodyEntityBytes = (multisigAccountModificationTransactionBody).Serialize();
            bw.Write(multisigAccountModificationTransactionBodyEntityBytes, 0, multisigAccountModificationTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}