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
    * Embedded version of AccountMosaicRestrictionTransaction.
    */
    [Serializable]
    public class EmbeddedAccountMosaicRestrictionTransactionBuilder: EmbeddedTransactionBuilder {

        /* Account mosaic restriction transaction body. */
        public AccountMosaicRestrictionTransactionBodyBuilder accountMosaicRestrictionTransactionBody;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal EmbeddedAccountMosaicRestrictionTransactionBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                accountMosaicRestrictionTransactionBody = AccountMosaicRestrictionTransactionBodyBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of EmbeddedAccountMosaicRestrictionTransactionBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of EmbeddedAccountMosaicRestrictionTransactionBuilder.
        */
        public new static EmbeddedAccountMosaicRestrictionTransactionBuilder LoadFromBinary(BinaryReader stream) {
            return new EmbeddedAccountMosaicRestrictionTransactionBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param restrictionFlags Type of restriction being applied to the listed mosaics..
        * @param restrictionAdditions Array of mosaics being added to the restricted list..
        * @param restrictionDeletions Array of mosaics being removed from the restricted list..
        */
        internal EmbeddedAccountMosaicRestrictionTransactionBuilder(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, List<AccountRestrictionFlagsDto> restrictionFlags, List<UnresolvedMosaicIdDto> restrictionAdditions, List<UnresolvedMosaicIdDto> restrictionDeletions)
            : base(signerPublicKey, version, network, type)
        {
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(network, "network is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(restrictionFlags, "restrictionFlags is null");
            GeneratorUtils.NotNull(restrictionAdditions, "restrictionAdditions is null");
            GeneratorUtils.NotNull(restrictionDeletions, "restrictionDeletions is null");
            this.accountMosaicRestrictionTransactionBody = new AccountMosaicRestrictionTransactionBodyBuilder(restrictionFlags, restrictionAdditions, restrictionDeletions);
        }
        
        /*
        * Creates an instance of EmbeddedAccountMosaicRestrictionTransactionBuilder.
        *
        * @param signerPublicKey Public key of the signer of the entity..
        * @param version Version of this structure..
        * @param network Network on which this entity was created..
        * @param type Transaction type.
        * @param restrictionFlags Type of restriction being applied to the listed mosaics..
        * @param restrictionAdditions Array of mosaics being added to the restricted list..
        * @param restrictionDeletions Array of mosaics being removed from the restricted list..
        * @return Instance of EmbeddedAccountMosaicRestrictionTransactionBuilder.
        */
        public static  EmbeddedAccountMosaicRestrictionTransactionBuilder Create(PublicKeyDto signerPublicKey, byte version, NetworkTypeDto network, TransactionTypeDto type, List<AccountRestrictionFlagsDto> restrictionFlags, List<UnresolvedMosaicIdDto> restrictionAdditions, List<UnresolvedMosaicIdDto> restrictionDeletions) {
            return new EmbeddedAccountMosaicRestrictionTransactionBuilder(signerPublicKey, version, network, type, restrictionFlags, restrictionAdditions, restrictionDeletions);
        }

        /*
        * Gets Type of restriction being applied to the listed mosaics..
        *
        * @return Type of restriction being applied to the listed mosaics..
        */
        public List<AccountRestrictionFlagsDto> GetRestrictionFlags() {
            return accountMosaicRestrictionTransactionBody.GetRestrictionFlags();
        }

        /*
        * Gets Array of mosaics being added to the restricted list..
        *
        * @return Array of mosaics being added to the restricted list..
        */
        public List<UnresolvedMosaicIdDto> GetRestrictionAdditions() {
            return accountMosaicRestrictionTransactionBody.GetRestrictionAdditions();
        }

        /*
        * Gets Array of mosaics being removed from the restricted list..
        *
        * @return Array of mosaics being removed from the restricted list..
        */
        public List<UnresolvedMosaicIdDto> GetRestrictionDeletions() {
            return accountMosaicRestrictionTransactionBody.GetRestrictionDeletions();
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public override int GetSize() {
            var size = base.GetSize();
            size += accountMosaicRestrictionTransactionBody.GetSize();
            return size;
        }

        /*
        * Gets the body builder of the object.
        *
        * @return Body builder.
        */
        public new AccountMosaicRestrictionTransactionBodyBuilder GetBody() {
            return accountMosaicRestrictionTransactionBody;
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
            var accountMosaicRestrictionTransactionBodyEntityBytes = (accountMosaicRestrictionTransactionBody).Serialize();
            bw.Write(accountMosaicRestrictionTransactionBodyEntityBytes, 0, accountMosaicRestrictionTransactionBodyEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
