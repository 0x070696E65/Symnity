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
    * Receipt generated when a [namespace](/concepts/namespace.html) expires.
    */
    [Serializable]
    public class NamespaceExpiryReceiptBuilder: ReceiptBuilder {

        /* Expiring namespace identifier.. */
        public NamespaceIdDto artifactId;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal NamespaceExpiryReceiptBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                artifactId = NamespaceIdDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of NamespaceExpiryReceiptBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of NamespaceExpiryReceiptBuilder.
        */
        public new static NamespaceExpiryReceiptBuilder LoadFromBinary(BinaryReader stream) {
            return new NamespaceExpiryReceiptBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param artifactId Expiring namespace identifier..
        */
        internal NamespaceExpiryReceiptBuilder(short version, ReceiptTypeDto type, NamespaceIdDto artifactId)
            : base(version, type)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(artifactId, "artifactId is null");
            this.artifactId = artifactId;
        }
        
        /*
        * Creates an instance of NamespaceExpiryReceiptBuilder.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param artifactId Expiring namespace identifier..
        * @return Instance of NamespaceExpiryReceiptBuilder.
        */
        public static  NamespaceExpiryReceiptBuilder Create(short version, ReceiptTypeDto type, NamespaceIdDto artifactId) {
            return new NamespaceExpiryReceiptBuilder(version, type, artifactId);
        }

        /*
        * Gets Expiring namespace identifier..
        *
        * @return Expiring namespace identifier..
        */
        public NamespaceIdDto GetArtifactId() {
            return artifactId;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += artifactId.GetSize();
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
            var artifactIdEntityBytes = (artifactId).Serialize();
            bw.Write(artifactIdEntityBytes, 0, artifactIdEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
