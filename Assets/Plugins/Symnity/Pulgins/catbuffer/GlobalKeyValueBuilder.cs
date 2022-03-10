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
    * Binary layout for a global key-value
    */
    [Serializable]
    public class GlobalKeyValueBuilder: ISerializer {

        /* Key associated with a restriction rule. */
        public MosaicRestrictionKeyDto key;
        /* Restriction rule (the value) associated with a key. */
        public RestrictionRuleBuilder restrictionRule;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal GlobalKeyValueBuilder(BinaryReader stream)
        {
            try {
                key = MosaicRestrictionKeyDto.LoadFromBinary(stream);
                restrictionRule = RestrictionRuleBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of GlobalKeyValueBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of GlobalKeyValueBuilder.
        */
        public static GlobalKeyValueBuilder LoadFromBinary(BinaryReader stream) {
            return new GlobalKeyValueBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param key Key associated with a restriction rule.
        * @param restrictionRule Restriction rule (the value) associated with a key.
        */
        internal GlobalKeyValueBuilder(MosaicRestrictionKeyDto key, RestrictionRuleBuilder restrictionRule)
        {
            GeneratorUtils.NotNull(key, "key is null");
            GeneratorUtils.NotNull(restrictionRule, "restrictionRule is null");
            this.key = key;
            this.restrictionRule = restrictionRule;
        }
        
        /*
        * Creates an instance of GlobalKeyValueBuilder.
        *
        * @param key Key associated with a restriction rule.
        * @param restrictionRule Restriction rule (the value) associated with a key.
        * @return Instance of GlobalKeyValueBuilder.
        */
        public static  GlobalKeyValueBuilder Create(MosaicRestrictionKeyDto key, RestrictionRuleBuilder restrictionRule) {
            return new GlobalKeyValueBuilder(key, restrictionRule);
        }

        /*
        * Gets key associated with a restriction rule.
        *
        * @return Key associated with a restriction rule.
        */
        public MosaicRestrictionKeyDto GetKey() {
            return key;
        }

        /*
        * Gets restriction rule (the value) associated with a key.
        *
        * @return Restriction rule (the value) associated with a key.
        */
        public RestrictionRuleBuilder GetRestrictionRule() {
            return restrictionRule;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += key.GetSize();
            size += restrictionRule.GetSize();
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
            var keyEntityBytes = (key).Serialize();
            bw.Write(keyEntityBytes, 0, keyEntityBytes.Length);
            var restrictionRuleEntityBytes = (restrictionRule).Serialize();
            bw.Write(restrictionRuleEntityBytes, 0, restrictionRuleEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
