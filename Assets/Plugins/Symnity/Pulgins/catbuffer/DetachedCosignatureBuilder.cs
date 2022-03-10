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
    * Cosignature detached from an AggregateCompleteTransaction or AggregateBondedTransaction.
    */
    [Serializable]
    public class DetachedCosignatureBuilder: CosignatureBuilder {

        /* Hash of the AggregateBondedTransaction that is signed by this cosignature.. */
        public Hash256Dto parentHash;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal DetachedCosignatureBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                parentHash = Hash256Dto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of DetachedCosignatureBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of DetachedCosignatureBuilder.
        */
        public new static DetachedCosignatureBuilder LoadFromBinary(BinaryReader stream) {
            return new DetachedCosignatureBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Version..
        * @param signerPublicKey Cosigner public key..
        * @param signature Transaction signature..
        * @param parentHash Hash of the AggregateBondedTransaction that is signed by this cosignature..
        */
        internal DetachedCosignatureBuilder(long version, PublicKeyDto signerPublicKey, SignatureDto signature, Hash256Dto parentHash)
            : base(version, signerPublicKey, signature)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(signature, "signature is null");
            GeneratorUtils.NotNull(parentHash, "parentHash is null");
            this.parentHash = parentHash;
        }
        
        /*
        * Creates an instance of DetachedCosignatureBuilder.
        *
        * @param version Version..
        * @param signerPublicKey Cosigner public key..
        * @param signature Transaction signature..
        * @param parentHash Hash of the AggregateBondedTransaction that is signed by this cosignature..
        * @return Instance of DetachedCosignatureBuilder.
        */
        public static  DetachedCosignatureBuilder Create(long version, PublicKeyDto signerPublicKey, SignatureDto signature, Hash256Dto parentHash) {
            return new DetachedCosignatureBuilder(version, signerPublicKey, signature, parentHash);
        }

        /*
        * Gets Hash of the AggregateBondedTransaction that is signed by this cosignature..
        *
        * @return Hash of the AggregateBondedTransaction that is signed by this cosignature..
        */
        public Hash256Dto GetParentHash() {
            return parentHash;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += parentHash.GetSize();
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
            var parentHashEntityBytes = (parentHash).Serialize();
            bw.Write(parentHashEntityBytes, 0, parentHashEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
