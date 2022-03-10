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
    * An Address resolution statement links a namespace alias used in a transaction to the real address **at the time of the transaction**.
    */
    [Serializable]
    public class AddressResolutionStatementBuilder: ReceiptBuilder {

        /* Unresolved address.. */
        public UnresolvedAddressDto unresolved;
        /* Resolution entries.. */
        public List<AddressResolutionEntryBuilder> resolutionEntries;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal AddressResolutionStatementBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                unresolved = UnresolvedAddressDto.LoadFromBinary(stream);
                resolutionEntries = GeneratorUtils.LoadFromBinaryArrayRemaining(AddressResolutionEntryBuilder.LoadFromBinary, stream, 0);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of AddressResolutionStatementBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of AddressResolutionStatementBuilder.
        */
        public new static AddressResolutionStatementBuilder LoadFromBinary(BinaryReader stream) {
            return new AddressResolutionStatementBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param unresolved Unresolved address..
        * @param resolutionEntries Resolution entries..
        */
        internal AddressResolutionStatementBuilder(short version, ReceiptTypeDto type, UnresolvedAddressDto unresolved, List<AddressResolutionEntryBuilder> resolutionEntries)
            : base(version, type)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(unresolved, "unresolved is null");
            GeneratorUtils.NotNull(resolutionEntries, "resolutionEntries is null");
            this.unresolved = unresolved;
            this.resolutionEntries = resolutionEntries;
        }
        
        /*
        * Creates an instance of AddressResolutionStatementBuilder.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param unresolved Unresolved address..
        * @param resolutionEntries Resolution entries..
        * @return Instance of AddressResolutionStatementBuilder.
        */
        public static  AddressResolutionStatementBuilder Create(short version, ReceiptTypeDto type, UnresolvedAddressDto unresolved, List<AddressResolutionEntryBuilder> resolutionEntries) {
            return new AddressResolutionStatementBuilder(version, type, unresolved, resolutionEntries);
        }

        /*
        * Gets Unresolved address..
        *
        * @return Unresolved address..
        */
        public UnresolvedAddressDto GetUnresolved() {
            return unresolved;
        }

        /*
        * Gets Resolution entries..
        *
        * @return Resolution entries..
        */
        public List<AddressResolutionEntryBuilder> GetResolutionEntries() {
            return resolutionEntries;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += unresolved.GetSize();
            size +=  GeneratorUtils.GetSumSize(resolutionEntries, 0);
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
            var unresolvedEntityBytes = (unresolved).Serialize();
            bw.Write(unresolvedEntityBytes, 0, unresolvedEntityBytes.Length);
            GeneratorUtils.WriteList(bw, resolutionEntries, 0);
            var result = ms.ToArray();
            return result;
        }
    }
}
