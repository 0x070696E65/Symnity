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
    * Shared content between TransferTransaction and EmbeddedTransferTransaction.
    */
    [Serializable]
    public class TransferTransactionBodyBuilder: ISerializer {

        /* Recipient address. */
        public UnresolvedAddressDto recipientAddress;
        /* Reserved padding to align mosaics on 8-byte boundary. */
        public int transferTransactionBodyReserved1;
        /* Reserved padding to align mosaics on 8-byte boundary. */
        public byte transferTransactionBodyReserved2;
        /* Attached mosaics. */
        public List<UnresolvedMosaicBuilder> mosaics;
        /* Attached message. */
        public byte[] message;
        
        /*
        * Constructor - Creates an object from stream.
        *
        * @param stream Byte stream to use to serialize the object.
        */
        internal TransferTransactionBodyBuilder(BinaryReader stream)
        {
            try {
                recipientAddress = UnresolvedAddressDto.LoadFromBinary(stream);
                var messageSize = stream.ReadInt16();
                var mosaicsCount = stream.ReadByte();
                transferTransactionBodyReserved1 = stream.ReadInt32();
                transferTransactionBodyReserved2 = stream.ReadByte();
                mosaics = GeneratorUtils.LoadFromBinaryArray(UnresolvedMosaicBuilder.LoadFromBinary, stream, mosaicsCount, 0);
                message = GeneratorUtils.ReadBytes(stream, messageSize);
            } catch (Exception e) {
                throw new Exception(e.ToString());
            }
        }

        /*
        * Creates an instance of TransferTransactionBodyBuilder from a stream.
        *
        * @param stream Byte stream to use to serialize the object.
        * @return Instance of TransferTransactionBodyBuilder.
        */
        public static TransferTransactionBodyBuilder LoadFromBinary(BinaryReader stream) {
            return new TransferTransactionBodyBuilder(stream);
        }

        
        /*
        * Constructor.
        *
        * @param recipientAddress Recipient address.
        * @param mosaics Attached mosaics.
        * @param message Attached message.
        */
        internal TransferTransactionBodyBuilder(UnresolvedAddressDto recipientAddress, List<UnresolvedMosaicBuilder> mosaics, byte[] message)
        {
            GeneratorUtils.NotNull(recipientAddress, "recipientAddress is null");
            GeneratorUtils.NotNull(mosaics, "mosaics is null");
            GeneratorUtils.NotNull(message, "message is null");
            this.recipientAddress = recipientAddress;
            this.transferTransactionBodyReserved1 = 0;
            this.transferTransactionBodyReserved2 = 0;
            this.mosaics = mosaics;
            this.message = message;
        }
        
        /*
        * Creates an instance of TransferTransactionBodyBuilder.
        *
        * @param recipientAddress Recipient address.
        * @param mosaics Attached mosaics.
        * @param message Attached message.
        * @return Instance of TransferTransactionBodyBuilder.
        */
        public static  TransferTransactionBodyBuilder Create(UnresolvedAddressDto recipientAddress, List<UnresolvedMosaicBuilder> mosaics, byte[] message) {
            return new TransferTransactionBodyBuilder(recipientAddress, mosaics, message);
        }

        /*
        * Gets recipient address.
        *
        * @return Recipient address.
        */
        public UnresolvedAddressDto GetRecipientAddress() {
            return recipientAddress;
        }

        /*
        * Gets reserved padding to align mosaics on 8-byte boundary.
        *
        * @return Reserved padding to align mosaics on 8-byte boundary.
        */
        private int GetTransferTransactionBodyReserved1() {
            return transferTransactionBodyReserved1;
        }

        /*
        * Gets reserved padding to align mosaics on 8-byte boundary.
        *
        * @return Reserved padding to align mosaics on 8-byte boundary.
        */
        private byte GetTransferTransactionBodyReserved2() {
            return transferTransactionBodyReserved2;
        }

        /*
        * Gets attached mosaics.
        *
        * @return Attached mosaics.
        */
        public List<UnresolvedMosaicBuilder> GetMosaics() {
            return mosaics;
        }

        /*
        * Gets attached message.
        *
        * @return Attached message.
        */
        public byte[] GetMessage() {
            return message;
        }

    
        /*
        * Gets the size of the object.
        *
        * @return Size in bytes.
        */
        public int GetSize() {
            var size = 0;
            size += recipientAddress.GetSize();
            size += 2; // messageSize
            size += 1; // mosaicsCount
            size += 4; // transferTransactionBodyReserved1
            size += 1; // transferTransactionBodyReserved2
            size +=  GeneratorUtils.GetSumSize(mosaics, 0);
            size += message.Length;
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
            var recipientAddressEntityBytes = (recipientAddress).Serialize();
            bw.Write(recipientAddressEntityBytes, 0, recipientAddressEntityBytes.Length);
            bw.Write((short)GeneratorUtils.GetSize(GetMessage()));
            bw.Write((byte)GeneratorUtils.GetSize(GetMosaics()));
            bw.Write(GetTransferTransactionBodyReserved1());
            bw.Write(GetTransferTransactionBodyReserved2());
            mosaics.ForEach(entity =>
            {
                var entityBytes = entity.Serialize();
                bw.Write(entityBytes, 0, entityBytes.Length);
                GeneratorUtils.AddPadding(entityBytes.Length, bw, 0);
            });
            bw.Write(message, 0, message.Length);
            var result = ms.ToArray();
            return result;
        }
    }
}
