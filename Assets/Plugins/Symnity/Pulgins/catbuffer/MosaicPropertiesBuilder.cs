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
    * Binary layout for mosaic properties
    */
    [Serializable]
    public class MosaicPropertiesBuilder: ISerializer {

        /* Mosaic flags. */
        public List<MosaicFlagsDto> flags;
        /* Mosaic divisibility. */
        public byte divisibility;
        /* Mosaic duration. */
        public BlockDurationDto duration;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicPropertiesBuilder(BinaryReader stream)
        {
            try {
                flags = GeneratorUtils.ToSet<MosaicFlagsDto>(stream.ReadByte());
                divisibility = stream.ReadByte();
                duration = BlockDurationDto.LoadFromBinary(stream);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicPropertiesBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicPropertiesBuilder.
        */
        public static MosaicPropertiesBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicPropertiesBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param flags Mosaic flags.
        * @param divisibility Mosaic divisibility.
        * @param duration Mosaic duration.
        */
        internal MosaicPropertiesBuilder(List<MosaicFlagsDto> flags, byte divisibility, BlockDurationDto duration)
        {
            GeneratorUtils.NotNull(flags, "flags is null");
            GeneratorUtils.NotNull(divisibility, "divisibility is null");
            GeneratorUtils.NotNull(duration, "duration is null");
            this.flags = flags;
            this.divisibility = divisibility;
            this.duration = duration;
        }
        
        /*
        * Creates an instance of MosaicPropertiesBuilder.
        *
        * @param flags Mosaic flags.
        * @param divisibility Mosaic divisibility.
        * @param duration Mosaic duration.
        * @return Instance of MosaicPropertiesBuilder.
        */
        public static  MosaicPropertiesBuilder Create(List<MosaicFlagsDto> flags, byte divisibility, BlockDurationDto duration) {
            return new MosaicPropertiesBuilder(flags, divisibility, duration);
        }

        /*
        * Gets mosaic flags.
        *
        * @return Mosaic flags.
        */
        public List<MosaicFlagsDto> GetFlags() {
            return flags;
        }

        /*
        * Gets mosaic divisibility.
        *
        * @return Mosaic divisibility.
        */
        public byte GetDivisibility() {
            return divisibility;
        }

        /*
        * Gets mosaic duration.
        *
        * @return Mosaic duration.
        */
        public BlockDurationDto GetDuration() {
            return duration;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += 1; // flags
            size += 1; // divisibility
            size += duration.GetSize();
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
            bw.Write((byte)GeneratorUtils.ToLong(flags));
            bw.Write(GetDivisibility());
            var durationEntityBytes = (duration).Serialize();
            bw.Write(durationEntityBytes, 0, durationEntityBytes.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
