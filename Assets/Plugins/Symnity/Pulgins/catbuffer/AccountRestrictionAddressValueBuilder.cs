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
    * Binary layout for address based account restriction
    */
    [Serializable]
    public class AccountRestrictionAddressValueBuilder: ISerializer {

        /* Restriction values. */
        public List<AddressDto> restrictionValues;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountRestrictionAddressValueBuilder(BinaryReader stream)
        {
            try {
                var restrictionValuesCount = stream.ReadInt64();
                restrictionValues = GeneratorUtils.LoadFromBinaryArray(AddressDto.LoadFromBinary, stream, restrictionValuesCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountRestrictionAddressValueBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountRestrictionAddressValueBuilder.
        */
        public static AccountRestrictionAddressValueBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountRestrictionAddressValueBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param restrictionValues Restriction values.
        */
        internal AccountRestrictionAddressValueBuilder(List<AddressDto> restrictionValues)
        {
            GeneratorUtils.NotNull(restrictionValues, "restrictionValues is null");
            this.restrictionValues = restrictionValues;
        }
        
        /*
        * Creates an instance of AccountRestrictionAddressValueBuilder.
        *
        * @param restrictionValues Restriction values.
        * @return Instance of AccountRestrictionAddressValueBuilder.
        */
        public static  AccountRestrictionAddressValueBuilder Create(List<AddressDto> restrictionValues) {
            return new AccountRestrictionAddressValueBuilder(restrictionValues);
        }

        /*
        * Gets restriction values.
        *
        * @return Restriction values.
        */
        public List<AddressDto> GetRestrictionValues() {
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
            size +=  GeneratorUtils.GetSumSize(restrictionValues, 0);
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
