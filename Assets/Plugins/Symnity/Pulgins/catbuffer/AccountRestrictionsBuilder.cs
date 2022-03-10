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
    public class AccountRestrictionsBuilder: StateHeaderBuilder {

        /* Address on which restrictions are placed. */
        public AddressDto address;
        /* Account restrictions. */
        public List<AccountRestrictionsInfoBuilder> restrictions;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AccountRestrictionsBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                address = AddressDto.LoadFromBinary(stream);
                var restrictionsCount = stream.ReadInt64();
                restrictions = GeneratorUtils.LoadFromBinaryArray(AccountRestrictionsInfoBuilder.LoadFromBinary, stream, restrictionsCount, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AccountRestrictionsBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AccountRestrictionsBuilder.
        */
        public new static AccountRestrictionsBuilder LoadFromBinary(BinaryReader stream) {
            return new AccountRestrictionsBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Serialization version.
        * @param address Address on which restrictions are placed.
        * @param restrictions Account restrictions.
        */
        internal AccountRestrictionsBuilder(short version, AddressDto address, List<AccountRestrictionsInfoBuilder> restrictions)
            : base(version)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(address, "address is null");
            GeneratorUtils.NotNull(restrictions, "restrictions is null");
            this.address = address;
            this.restrictions = restrictions;
        }
        
        /*
        * Creates an instance of AccountRestrictionsBuilder.
        *
        * @param version Serialization version.
        * @param address Address on which restrictions are placed.
        * @param restrictions Account restrictions.
        * @return Instance of AccountRestrictionsBuilder.
        */
        public static  AccountRestrictionsBuilder Create(short version, AddressDto address, List<AccountRestrictionsInfoBuilder> restrictions) {
            return new AccountRestrictionsBuilder(version, address, restrictions);
        }

        /*
        * Gets address on which restrictions are placed.
        *
        * @return Address on which restrictions are placed.
        */
        public AddressDto GetAddress() {
            return address;
        }

        /*
        * Gets account restrictions.
        *
        * @return Account restrictions.
        */
        public List<AccountRestrictionsInfoBuilder> GetRestrictions() {
            return restrictions;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += address.GetSize();
            size += 8; // restrictionsCount
            size +=  GeneratorUtils.GetSumSize(restrictions, 0);
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
            bw.Write((long)GeneratorUtils.GetSize(GetRestrictions()));
            restrictions.ForEach(entity =>
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
