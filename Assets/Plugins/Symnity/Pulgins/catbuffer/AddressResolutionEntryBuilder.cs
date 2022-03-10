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
    * Actual Address behind a NamespaceId at the time a transaction was confirmed.
    */
    [Serializable]
    public class AddressResolutionEntryBuilder: ISerializer {

        /* Information about the transaction that triggered the receipt.. */
        public ReceiptSourceBuilder source;
        /* Resolved Address.. */
        public AddressDto resolved;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AddressResolutionEntryBuilder(BinaryReader stream)
        {
            try {
                source = ReceiptSourceBuilder.LoadFromBinary(stream);
                resolved = AddressDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AddressResolutionEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AddressResolutionEntryBuilder.
        */
        public static AddressResolutionEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new AddressResolutionEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param source Information about the transaction that triggered the receipt..
        * @param resolved Resolved Address..
        */
        internal AddressResolutionEntryBuilder(ReceiptSourceBuilder source, AddressDto resolved)
        {
            GeneratorUtils.NotNull(source, "source is null");
            GeneratorUtils.NotNull(resolved, "resolved is null");
            this.source = source;
            this.resolved = resolved;
        }
        
        /*
        * Creates an instance of AddressResolutionEntryBuilder.
        *
        * @param source Information about the transaction that triggered the receipt..
        * @param resolved Resolved Address..
        * @return Instance of AddressResolutionEntryBuilder.
        */
        public static  AddressResolutionEntryBuilder Create(ReceiptSourceBuilder source, AddressDto resolved) {
            return new AddressResolutionEntryBuilder(source, resolved);
        }

        /*
        * Gets Information about the transaction that triggered the receipt..
        *
        * @return Information about the transaction that triggered the receipt..
        */
        public ReceiptSourceBuilder GetSource() {
            return source;
        }

        /*
        * Gets Resolved Address..
        *
        * @return Resolved Address..
        */
        public AddressDto GetResolved() {
            return resolved;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += source.GetSize();
            size += resolved.GetSize();
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
            var sourceEntityBytes = (source).Serialize();
            bw.Write(sourceEntityBytes, 0, sourceEntityBytes.Length);
            var resolvedEntityBytes = (resolved).Serialize();
            bw.Write(resolvedEntityBytes, 0, resolvedEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
