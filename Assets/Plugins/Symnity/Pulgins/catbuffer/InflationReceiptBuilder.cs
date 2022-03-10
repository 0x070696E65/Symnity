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
    * Network currency mosaics were created due to [inflation](/concepts/inflation).
    */
    [Serializable]
    public class InflationReceiptBuilder: ReceiptBuilder {

        /* Created mosaic.. */
        public MosaicBuilder mosaic;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal InflationReceiptBuilder(BinaryReader stream)
            : base(stream)
        {
            try {
                mosaic = MosaicBuilder.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of InflationReceiptBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of InflationReceiptBuilder.
        */
        public new static InflationReceiptBuilder LoadFromBinary(BinaryReader stream) {
            return new InflationReceiptBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param mosaic Created mosaic..
        */
        internal InflationReceiptBuilder(short version, ReceiptTypeDto type, MosaicBuilder mosaic)
            : base(version, type)
        {
            GeneratorUtils.NotNull(version, "version is null");
            GeneratorUtils.NotNull(type, "type is null");
            GeneratorUtils.NotNull(mosaic, "mosaic is null");
            this.mosaic = mosaic;
        }
        
        /*
        * Creates an instance of InflationReceiptBuilder.
        *
        * @param version Receipt version..
        * @param type Type of receipt..
        * @param mosaic Created mosaic..
        * @return Instance of InflationReceiptBuilder.
        */
        public static  InflationReceiptBuilder Create(short version, ReceiptTypeDto type, MosaicBuilder mosaic) {
            return new InflationReceiptBuilder(version, type, mosaic);
        }

        /*
        * Gets Created mosaic..
        *
        * @return Created mosaic..
        */
        public MosaicBuilder GetMosaic() {
            return mosaic;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public new int GetSize() {
            var size = base.GetSize();
            size += mosaic.GetSize();
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
            var mosaicEntityBytes = (mosaic).Serialize();
            bw.Write(mosaicEntityBytes, 0, mosaicEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
