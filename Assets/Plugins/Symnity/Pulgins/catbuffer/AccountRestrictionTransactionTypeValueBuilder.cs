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
    * Binary layout for transaction type based account restriction
    */
    [Serializable]
    public class AccountRestrictionTransactionTypeValueBuilder: ISerializer {

        /* Restriction values. */
        public List<TransactionTypeDto> restrictionValues;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountRestrictionTransactionTypeValueBuilder(BinaryReader stream)
        {
            try {
                var restrictionValuesCount = stream.ReadInt64();
                restrictionValues = new List<TransactionTypeDto>(){};
                for (var i = 0; i < restrictionValuesCount; i++)
                {
                    int restrictionValuesStream = stream.ReadInt16();
                    foreach (TransactionTypeDto tt in Enum.GetValues(typeof(TransactionTypeDto))) {
                        if ((int)(object)tt == restrictionValuesStream)
                        {
                            restrictionValues.Add(tt);
                            GeneratorUtils.SkipPadding(tt.GetSize(), stream, 0);
                            break;
                        }
                    }
                }
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountRestrictionTransactionTypeValueBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountRestrictionTransactionTypeValueBuilder.
        */
        public static AccountRestrictionTransactionTypeValueBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountRestrictionTransactionTypeValueBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param restrictionValues Restriction values.
        */
        internal AccountRestrictionTransactionTypeValueBuilder(List<TransactionTypeDto> restrictionValues)
        {
            GeneratorUtils.NotNull(restrictionValues, "restrictionValues is null");
            this.restrictionValues = restrictionValues;
        }
        
        /*
        * Creates an instance of AccountRestrictionTransactionTypeValueBuilder.
        *
        * @param restrictionValues Restriction values.
        * @return Instance of AccountRestrictionTransactionTypeValueBuilder.
        */
        public static  AccountRestrictionTransactionTypeValueBuilder Create(List<TransactionTypeDto> restrictionValues) {
            return new AccountRestrictionTransactionTypeValueBuilder(restrictionValues);
        }

        /*
        * Gets restriction values.
        *
        * @return Restriction values.
        */
        public List<TransactionTypeDto> GetRestrictionValues() {
            return restrictionValues;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 8; // restrictionValuesCount
            restrictionValues.ForEach(value =>
            {
                size += value.GetSize();
            });
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
            bw.Write((long)GeneratorUtils.GetSize(GetRestrictionValues()));
            restrictionValues.ForEach(entity =>
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
