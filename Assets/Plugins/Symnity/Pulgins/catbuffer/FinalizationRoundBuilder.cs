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
    * Binary layout for finalization round
    */
    [Serializable]
    public class FinalizationRoundBuilder: ISerializer {

        /* Finalization epoch. */
        public FinalizationEpochDto epoch;
        /* Finalization point. */
        public FinalizationPointDto point;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal FinalizationRoundBuilder(BinaryReader stream)
        {
            try {
                epoch = FinalizationEpochDto.LoadFromBinary(stream);
                point = FinalizationPointDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of FinalizationRoundBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of FinalizationRoundBuilder.
        */
        public static FinalizationRoundBuilder LoadFromBinary(BinaryReader stream) {
            return new FinalizationRoundBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param epoch Finalization epoch.
        * @param point Finalization point.
        */
        internal FinalizationRoundBuilder(FinalizationEpochDto epoch, FinalizationPointDto point)
        {
            GeneratorUtils.NotNull(epoch, "epoch is null");
            GeneratorUtils.NotNull(point, "point is null");
            this.epoch = epoch;
            this.point = point;
        }
        
        /*
        * Creates an instance of FinalizationRoundBuilder.
        *
        * @param epoch Finalization epoch.
        * @param point Finalization point.
        * @return Instance of FinalizationRoundBuilder.
        */
        public static  FinalizationRoundBuilder Create(FinalizationEpochDto epoch, FinalizationPointDto point) {
            return new FinalizationRoundBuilder(epoch, point);
        }

        /*
        * Gets finalization epoch.
        *
        * @return Finalization epoch.
        */
        public FinalizationEpochDto GetEpoch() {
            return epoch;
        }

        /*
        * Gets finalization point.
        *
        * @return Finalization point.
        */
        public FinalizationPointDto GetPoint() {
            return point;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += epoch.GetSize();
            size += point.GetSize();
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
            var epochEntityBytes = (epoch).Serialize();
            bw.Write(epochEntityBytes, 0, epochEntityBytes.Length);
            var pointEntityBytes = (point).Serialize();
            bw.Write(pointEntityBytes, 0, pointEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
