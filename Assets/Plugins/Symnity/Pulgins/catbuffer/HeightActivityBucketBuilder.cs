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
    * Account activity bucket
    */
    [Serializable]
    public class HeightActivityBucketBuilder: ISerializer {

        /* Activity start height. */
        public ImportanceHeightDto startHeight;
        /* Total fees paid by account. */
        public AmountDto totalFeesPaid;
        /* Number of times account has been used as a beneficiary. */
        public int beneficiaryCount;
        /* Raw importance score. */
        public long rawScore;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal HeightActivityBucketBuilder(BinaryReader stream)
        {
            try {
                startHeight = ImportanceHeightDto.LoadFromBinary(stream);
                totalFeesPaid = AmountDto.LoadFromBinary(stream);
                beneficiaryCount = stream.ReadInt32();
                rawScore = stream.ReadInt64();
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of HeightActivityBucketBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of HeightActivityBucketBuilder.
        */
        public static HeightActivityBucketBuilder LoadFromBinary(BinaryReader stream) {
            return new HeightActivityBucketBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param startHeight Activity start height.
        * @param totalFeesPaid Total fees paid by account.
        * @param beneficiaryCount Number of times account has been used as a beneficiary.
        * @param rawScore Raw importance score.
        */
        internal HeightActivityBucketBuilder(ImportanceHeightDto startHeight, AmountDto totalFeesPaid, int beneficiaryCount, long rawScore)
        {
            GeneratorUtils.NotNull(startHeight, "startHeight is null");
            GeneratorUtils.NotNull(totalFeesPaid, "totalFeesPaid is null");
            GeneratorUtils.NotNull(beneficiaryCount, "beneficiaryCount is null");
            GeneratorUtils.NotNull(rawScore, "rawScore is null");
            this.startHeight = startHeight;
            this.totalFeesPaid = totalFeesPaid;
            this.beneficiaryCount = beneficiaryCount;
            this.rawScore = rawScore;
        }
        
        /*
        * Creates an instance of HeightActivityBucketBuilder.
        *
        * @param startHeight Activity start height.
        * @param totalFeesPaid Total fees paid by account.
        * @param beneficiaryCount Number of times account has been used as a beneficiary.
        * @param rawScore Raw importance score.
        * @return Instance of HeightActivityBucketBuilder.
        */
        public static  HeightActivityBucketBuilder Create(ImportanceHeightDto startHeight, AmountDto totalFeesPaid, int beneficiaryCount, long rawScore) {
            return new HeightActivityBucketBuilder(startHeight, totalFeesPaid, beneficiaryCount, rawScore);
        }

        /*
        * Gets activity start height.
        *
        * @return Activity start height.
        */
        public ImportanceHeightDto GetStartHeight() {
            return startHeight;
        }

        /*
        * Gets total fees paid by account.
        *
        * @return Total fees paid by account.
        */
        public AmountDto GetTotalFeesPaid() {
            return totalFeesPaid;
        }

        /*
        * Gets number of times account has been used as a beneficiary.
        *
        * @return Number of times account has been used as a beneficiary.
        */
        public int GetBeneficiaryCount() {
            return beneficiaryCount;
        }

        /*
        * Gets raw importance score.
        *
        * @return Raw importance score.
        */
        public long GetRawScore() {
            return rawScore;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += startHeight.GetSize();
            size += totalFeesPaid.GetSize();
            size += 4; // beneficiaryCount
            size += 8; // rawScore
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
            var startHeightEntityBytes = (startHeight).Serialize();
            bw.Write(startHeightEntityBytes, 0, startHeightEntityBytes.Length);
            var totalFeesPaidEntityBytes = (totalFeesPaid).Serialize();
            bw.Write(totalFeesPaidEntityBytes, 0, totalFeesPaidEntityBytes.Length);
            bw.Write(GetBeneficiaryCount());
            bw.Write(GetRawScore());
            var result = ms.ToArray();
            return result;
        }
    }
}
