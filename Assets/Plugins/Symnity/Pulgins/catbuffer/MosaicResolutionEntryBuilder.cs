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
    * Actual MosaicId behind a NamespaceId at the time a transaction was confirmed.
    */
    [Serializable]
    public class MosaicResolutionEntryBuilder: ISerializer {

        /* Information about the transaction that triggered the receipt.. */
        public ReceiptSourceBuilder source;
        /* Resolved MosaicId.. */
        public MosaicIdDto resolved;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicResolutionEntryBuilder(BinaryReader stream)
        {
            try {
                source = ReceiptSourceBuilder.LoadFromBinary(stream);
                resolved = MosaicIdDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicResolutionEntryBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicResolutionEntryBuilder.
        */
        public static MosaicResolutionEntryBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicResolutionEntryBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param source Information about the transaction that triggered the receipt..
        * @param resolved Resolved MosaicId..
        */
        internal MosaicResolutionEntryBuilder(ReceiptSourceBuilder source, MosaicIdDto resolved)
        {
            GeneratorUtils.NotNull(source, "source is null");
            GeneratorUtils.NotNull(resolved, "resolved is null");
            this.source = source;
            this.resolved = resolved;
        }
        
        /*
        * Creates an instance of MosaicResolutionEntryBuilder.
        *
        * @param source Information about the transaction that triggered the receipt..
        * @param resolved Resolved MosaicId..
        * @return Instance of MosaicResolutionEntryBuilder.
        */
        public static  MosaicResolutionEntryBuilder Create(ReceiptSourceBuilder source, MosaicIdDto resolved) {
            return new MosaicResolutionEntryBuilder(source, resolved);
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
        * Gets Resolved MosaicId..
        *
        * @return Resolved MosaicId..
        */
        public MosaicIdDto GetResolved() {
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
