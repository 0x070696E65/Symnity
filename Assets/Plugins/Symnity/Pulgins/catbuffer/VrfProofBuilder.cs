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
    * Verfiable random function proof
    */
    [Serializable]
    public class VrfProofBuilder: ISerializer {

        /* Gamma. */
        public ProofGammaDto gamma;
        /* Verification hash. */
        public ProofVerificationHashDto verificationHash;
        /* Scalar. */
        public ProofScalarDto scalar;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal VrfProofBuilder(BinaryReader stream)
        {
            try {
                gamma = ProofGammaDto.LoadFromBinary(stream);
                verificationHash = ProofVerificationHashDto.LoadFromBinary(stream);
                scalar = ProofScalarDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of VrfProofBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of VrfProofBuilder.
        */
        public static VrfProofBuilder LoadFromBinary(BinaryReader stream) {
            return new VrfProofBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param gamma Gamma.
        * @param verificationHash Verification hash.
        * @param scalar Scalar.
        */
        internal VrfProofBuilder(ProofGammaDto gamma, ProofVerificationHashDto verificationHash, ProofScalarDto scalar)
        {
            GeneratorUtils.NotNull(gamma, "gamma is null");
            GeneratorUtils.NotNull(verificationHash, "verificationHash is null");
            GeneratorUtils.NotNull(scalar, "scalar is null");
            this.gamma = gamma;
            this.verificationHash = verificationHash;
            this.scalar = scalar;
        }
        
        /*
        * Creates an instance of VrfProofBuilder.
        *
        * @param gamma Gamma.
        * @param verificationHash Verification hash.
        * @param scalar Scalar.
        * @return Instance of VrfProofBuilder.
        */
        public static  VrfProofBuilder Create(ProofGammaDto gamma, ProofVerificationHashDto verificationHash, ProofScalarDto scalar) {
            return new VrfProofBuilder(gamma, verificationHash, scalar);
        }

        /*
        * Gets gamma.
        *
        * @return Gamma.
        */
        public ProofGammaDto GetGamma() {
            return gamma;
        }

        /*
        * Gets verification hash.
        *
        * @return Verification hash.
        */
        public ProofVerificationHashDto GetVerificationHash() {
            return verificationHash;
        }

        /*
        * Gets scalar.
        *
        * @return Scalar.
        */
        public ProofScalarDto GetScalar() {
            return scalar;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += gamma.GetSize();
            size += verificationHash.GetSize();
            size += scalar.GetSize();
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
            var gammaEntityBytes = (gamma).Serialize();
            bw.Write(gammaEntityBytes, 0, gammaEntityBytes.Length);
            var verificationHashEntityBytes = (verificationHash).Serialize();
            bw.Write(verificationHashEntityBytes, 0, verificationHashEntityBytes.Length);
            var scalarEntityBytes = (scalar).Serialize();
            bw.Write(scalarEntityBytes, 0, scalarEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
