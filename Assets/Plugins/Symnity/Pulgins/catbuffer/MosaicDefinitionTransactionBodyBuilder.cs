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
    * Shared content between MosaicDefinitionTransaction and Embedded MosaicDefinitionTransaction.
    */
    [Serializable]
    public class MosaicDefinitionTransactionBodyBuilder: ISerializer {

        /* Unique mosaic identifier obtained from the generator account's public key and the `nonce`.
The SDK's can take care of generating this ID for you.. */
        public MosaicIdDto id;
        /* Mosaic duration expressed in blocks. If set to 0, the mosaic never expires.. */
        public BlockDurationDto duration;
        /* Random nonce used to generate the mosaic id.. */
        public MosaicNonceDto nonce;
        /* Mosaic flags.. */
        public List<MosaicFlagsDto> flags;
        /* Mosaic divisibility.. */
        public byte divisibility;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal MosaicDefinitionTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                id = MosaicIdDto.LoadFromBinary(stream);
                duration = BlockDurationDto.LoadFromBinary(stream);
                nonce = MosaicNonceDto.LoadFromBinary(stream);
                flags = GeneratorUtils.ToSet<MosaicFlagsDto>(stream.ReadByte());
                divisibility = stream.ReadByte();
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of MosaicDefinitionTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of MosaicDefinitionTransactionBodyBuilder.
        */
        public static MosaicDefinitionTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new MosaicDefinitionTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param id Unique mosaic identifier obtained from the generator account's public key and the `nonce`.
The SDK's can take care of generating this ID for you..
        * @param duration Mosaic duration expressed in blocks. If set to 0, the mosaic never expires..
        * @param nonce Random nonce used to generate the mosaic id..
        * @param flags Mosaic flags..
        * @param divisibility Mosaic divisibility..
        */
        internal MosaicDefinitionTransactionBodyBuilder(MosaicIdDto id, BlockDurationDto duration, MosaicNonceDto nonce, List<MosaicFlagsDto> flags, byte divisibility)
        {
            GeneratorUtils.NotNull(id, "id is null");
            GeneratorUtils.NotNull(duration, "duration is null");
            GeneratorUtils.NotNull(nonce, "nonce is null");
            GeneratorUtils.NotNull(flags, "flags is null");
            GeneratorUtils.NotNull(divisibility, "divisibility is null");
            this.id = id;
            this.duration = duration;
            this.nonce = nonce;
            this.flags = flags;
            this.divisibility = divisibility;
        }
        
        /*
        * Creates an instance of MosaicDefinitionTransactionBodyBuilder.
        *
        * @param id Unique mosaic identifier obtained from the generator account's public key and the `nonce`.
The SDK's can take care of generating this ID for you..
        * @param duration Mosaic duration expressed in blocks. If set to 0, the mosaic never expires..
        * @param nonce Random nonce used to generate the mosaic id..
        * @param flags Mosaic flags..
        * @param divisibility Mosaic divisibility..
        * @return Instance of MosaicDefinitionTransactionBodyBuilder.
        */
        public static  MosaicDefinitionTransactionBodyBuilder Create(MosaicIdDto id, BlockDurationDto duration, MosaicNonceDto nonce, List<MosaicFlagsDto> flags, byte divisibility) {
            return new MosaicDefinitionTransactionBodyBuilder(id, duration, nonce, flags, divisibility);
        }

        /*
        * Gets Unique mosaic identifier obtained from the generator account's public key and the `nonce`.
The SDK's can take care of generating this ID for you..
        *
        * @return Unique mosaic identifier obtained from the generator account's public key and the `nonce`.
The SDK's can take care of generating this ID for you..
        */
        public MosaicIdDto GetId() {
            return id;
        }

        /*
        * Gets Mosaic duration expressed in blocks. If set to 0, the mosaic never expires..
        *
        * @return Mosaic duration expressed in blocks. If set to 0, the mosaic never expires..
        */
        public BlockDurationDto GetDuration() {
            return duration;
        }

        /*
        * Gets Random nonce used to generate the mosaic id..
        *
        * @return Random nonce used to generate the mosaic id..
        */
        public MosaicNonceDto GetNonce() {
            return nonce;
        }

        /*
        * Gets Mosaic flags..
        *
        * @return Mosaic flags..
        */
        public List<MosaicFlagsDto> GetFlags() {
            return flags;
        }

        /*
        * Gets Mosaic divisibility..
        *
        * @return Mosaic divisibility..
        */
        public byte GetDivisibility() {
            return divisibility;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += id.GetSize();
            size += duration.GetSize();
            size += nonce.GetSize();
            size += 1; // flags
            size += 1; // divisibility
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
            var idEntityBytes = (id).Serialize();
            bw.Write(idEntityBytes, 0, idEntityBytes.Length);
            var durationEntityBytes = (duration).Serialize();
            bw.Write(durationEntityBytes, 0, durationEntityBytes.Length);
            var nonceEntityBytes = (nonce).Serialize();
            bw.Write(nonceEntityBytes, 0, nonceEntityBytes.Length);
            bw.Write((byte)GeneratorUtils.ToLong(flags));
            bw.Write(GetDivisibility());
            var result = ms.ToArray();
            return result;
        }
    }
}
