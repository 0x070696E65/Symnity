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
    * Binary layout for account restrictions
    */
    [Serializable]
    public class AccountRestrictionsInfoBuilder: ISerializer {

        /* Raw restriction flags. */
        public List<AccountRestrictionFlagsDto> restrictionFlags;
        /* Address restrictions. */
        public AccountRestrictionAddressValueBuilder addressRestrictions;
        /* Mosaic identifier restrictions. */
        public AccountRestrictionMosaicValueBuilder mosaicIdRestrictions;
        /* Transaction type restrictions. */
        public AccountRestrictionTransactionTypeValueBuilder transactionTypeRestrictions;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountRestrictionsInfoBuilder(BinaryReader stream)
        {
            try {
                restrictionFlags = GeneratorUtils.ToSet<AccountRestrictionFlagsDto>(stream.ReadInt16());
                if (this.restrictionFlags.Contains(AccountRestrictionFlagsDto.ADDRESS)) {
                    addressRestrictions = AccountRestrictionAddressValueBuilder.LoadFromBinary(stream);
                }
                if (this.restrictionFlags.Contains(AccountRestrictionFlagsDto.MOSAIC_ID)) {
                    mosaicIdRestrictions = AccountRestrictionMosaicValueBuilder.LoadFromBinary(stream);
                }
                if (this.restrictionFlags.Contains(AccountRestrictionFlagsDto.TRANSACTION_TYPE)) {
                    transactionTypeRestrictions = AccountRestrictionTransactionTypeValueBuilder.LoadFromBinary(stream);
                }
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountRestrictionsInfoBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountRestrictionsInfoBuilder.
        */
        public static AccountRestrictionsInfoBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountRestrictionsInfoBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param restrictionFlags Raw restriction flags.
        * @param addressRestrictions Address restrictions.
        * @param mosaicIdRestrictions Mosaic identifier restrictions.
        * @param transactionTypeRestrictions Transaction type restrictions.
        */
        internal AccountRestrictionsInfoBuilder(List<AccountRestrictionFlagsDto> restrictionFlags, AccountRestrictionAddressValueBuilder addressRestrictions, AccountRestrictionMosaicValueBuilder mosaicIdRestrictions, AccountRestrictionTransactionTypeValueBuilder transactionTypeRestrictions)
        {
            GeneratorUtils.NotNull(restrictionFlags, "restrictionFlags is null");
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.ADDRESS)) {
                GeneratorUtils.NotNull(addressRestrictions, "addressRestrictions is null");
            }
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.MOSAIC_ID)) {
                GeneratorUtils.NotNull(mosaicIdRestrictions, "mosaicIdRestrictions is null");
            }
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.TRANSACTION_TYPE)) {
                GeneratorUtils.NotNull(transactionTypeRestrictions, "transactionTypeRestrictions is null");
            }
            this.restrictionFlags = restrictionFlags;
            this.addressRestrictions = addressRestrictions;
            this.mosaicIdRestrictions = mosaicIdRestrictions;
            this.transactionTypeRestrictions = transactionTypeRestrictions;
        }
        
        /*
        * Creates an instance of AccountRestrictionsInfoBuilder.
        *
        * @param restrictionFlags Raw restriction flags.
        * @param addressRestrictions Address restrictions.
        * @param mosaicIdRestrictions Mosaic identifier restrictions.
        * @param transactionTypeRestrictions Transaction type restrictions.
        * @return Instance of AccountRestrictionsInfoBuilder.
        */
        public static  AccountRestrictionsInfoBuilder Create(List<AccountRestrictionFlagsDto> restrictionFlags, AccountRestrictionAddressValueBuilder addressRestrictions, AccountRestrictionMosaicValueBuilder mosaicIdRestrictions, AccountRestrictionTransactionTypeValueBuilder transactionTypeRestrictions) {
            return new AccountRestrictionsInfoBuilder(restrictionFlags, addressRestrictions, mosaicIdRestrictions, transactionTypeRestrictions);
        }

        /*
        * Gets raw restriction flags.
        *
        * @return Raw restriction flags.
        */
        public List<AccountRestrictionFlagsDto> GetRestrictionFlags() {
            return restrictionFlags;
        }

        /*
        * Gets address restrictions.
        *
        * @return Address restrictions.
        */
        public AccountRestrictionAddressValueBuilder GetAddressRestrictions() {
            if (!(restrictionFlags.Contains(AccountRestrictionFlagsDto.ADDRESS))) {
                throw new Exception("restrictionFlags is not set to ADDRESS.");
            }
            return addressRestrictions;
        }

        /*
        * Gets mosaic identifier restrictions.
        *
        * @return Mosaic identifier restrictions.
        */
        public AccountRestrictionMosaicValueBuilder GetMosaicIdRestrictions() {
            if (!(restrictionFlags.Contains(AccountRestrictionFlagsDto.MOSAIC_ID))) {
                throw new Exception("restrictionFlags is not set to MOSAIC_ID.");
            }
            return mosaicIdRestrictions;
        }

        /*
        * Gets transaction type restrictions.
        *
        * @return Transaction type restrictions.
        */
        public AccountRestrictionTransactionTypeValueBuilder GetTransactionTypeRestrictions() {
            if (!(restrictionFlags.Contains(AccountRestrictionFlagsDto.TRANSACTION_TYPE))) {
                throw new Exception("restrictionFlags is not set to TRANSACTION_TYPE.");
            }
            return transactionTypeRestrictions;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 2; // restrictionFlags
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.ADDRESS)) {
                if (addressRestrictions != null) {
                size += ((AccountRestrictionAddressValueBuilder) addressRestrictions).GetSize();
            }
            }
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.MOSAIC_ID)) {
                if (mosaicIdRestrictions != null) {
                size += ((AccountRestrictionMosaicValueBuilder) mosaicIdRestrictions).GetSize();
            }
            }
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.TRANSACTION_TYPE)) {
                if (transactionTypeRestrictions != null) {
                size += ((AccountRestrictionTransactionTypeValueBuilder) transactionTypeRestrictions).GetSize();
            }
            }
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
            bw.Write((short)GeneratorUtils.ToLong(restrictionFlags));
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.ADDRESS)) {
                var addressRestrictionsEntityBytes = (addressRestrictions).Serialize();
            bw.Write(addressRestrictionsEntityBytes, 0, addressRestrictionsEntityBytes.Length);
            }
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.MOSAIC_ID)) {
                var mosaicIdRestrictionsEntityBytes = (mosaicIdRestrictions).Serialize();
            bw.Write(mosaicIdRestrictionsEntityBytes, 0, mosaicIdRestrictionsEntityBytes.Length);
            }
            if (restrictionFlags.Contains(AccountRestrictionFlagsDto.TRANSACTION_TYPE)) {
                var transactionTypeRestrictionsEntityBytes = (transactionTypeRestrictions).Serialize();
            bw.Write(transactionTypeRestrictionsEntityBytes, 0, transactionTypeRestrictionsEntityBytes.Length);
            }
            var result = ms.ToArray();
            return result;
        }
    }
}
