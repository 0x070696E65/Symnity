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
    * Cosignature attached to an AggregateCompleteTransaction or AggregateBondedTransaction.
    */
    [Serializable]
    public class CosignatureBuilder: ISerializer {

        /* Version.. */
        public long version;
        /* Cosigner public key.. */
        public PublicKeyDto signerPublicKey;
        /* Transaction signature.. */
        public SignatureDto signature;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal CosignatureBuilder(BinaryReader stream)
        {
            try {
                version = stream.ReadInt64();
                signerPublicKey = PublicKeyDto.LoadFromBinary(stream);
                signature = SignatureDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of CosignatureBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of CosignatureBuilder.
        */
        public static CosignatureBuilder LoadFromBinary(BinaryReader stream) {
            return new CosignatureBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Version..
        * @param signerPublicKey Cosigner public key..
        * @param signature Transaction signature..
        */
        internal CosignatureBuilder(long version, PublicKeyDto signerPublicKey, SignatureDto signature)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(signerPublicKey, "signerPublicKey is null");
            GeneratorUtils.NotNull(signature, "signature is null");
            this.version = version;
            this.signerPublicKey = signerPublicKey;
            this.signature = signature;
        }
        
        /*
        * Creates an instance of CosignatureBuilder.
        *
        * @param version Version..
        * @param signerPublicKey Cosigner public key..
        * @param signature Transaction signature..
        * @return Instance of CosignatureBuilder.
        */
        public static  CosignatureBuilder Create(long version, PublicKeyDto signerPublicKey, SignatureDto signature) {
            return new CosignatureBuilder(version, signerPublicKey, signature);
        }

        /*
        * Gets Version..
        *
        * @return Version..
        */
        public long GetVersion() {
            return version;
        }

        /*
        * Gets Cosigner public key..
        *
        * @return Cosigner public key..
        */
        public PublicKeyDto GetSignerPublicKey() {
            return signerPublicKey;
        }

        /*
        * Gets Transaction signature..
        *
        * @return Transaction signature..
        */
        public SignatureDto GetSignature() {
            return signature;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 8; // version
            size += signerPublicKey.GetSize();
            size += signature.GetSize();
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
            bw.Write(GetVersion());
            var signerPublicKeyEntityBytes = (signerPublicKey).Serialize();
            bw.Write(signerPublicKeyEntityBytes, 0, signerPublicKeyEntityBytes.Length);
            var signatureEntityBytes = (signature).Serialize();
            bw.Write(signatureEntityBytes, 0, signatureEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
