using System;

namespace Symnity.Core.Format
{
    public class Utilities
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        public const byte Decoded_Block_Size = 5;
        public const byte Encoded_Block_Size = 8;
        
        public static void EncodeBlock(byte[] input, int inputOffset, byte[] output, int outputOffset)
        {
            output[outputOffset + 0] = (byte)Alphabet[input[inputOffset + 0] >> 3];
            output[outputOffset + 1] = (byte)Alphabet[((input[inputOffset + 0] & 0x07) << 2) | (input[inputOffset + 1] >> 6)];
            output[outputOffset + 2] = (byte)Alphabet[(input[inputOffset + 1] & 0x3e) >> 1];
            output[outputOffset + 3] = (byte)Alphabet[((input[inputOffset + 1] & 0x01) << 4) | (input[inputOffset + 2] >> 4)];
            output[outputOffset + 4] = (byte)Alphabet[((input[inputOffset + 2] & 0x0f) << 1) | (input[inputOffset + 3] >> 7)];
            output[outputOffset + 5] = (byte)Alphabet[(input[inputOffset + 3] & 0x7f) >> 2];
            output[outputOffset + 6] = (byte)Alphabet[((input[inputOffset + 3] & 0x03) << 3) | (input[inputOffset + 4] >> 5)];
            output[outputOffset + 7] = (byte)Alphabet[input[inputOffset + 4] & 0x1f];
        }
        
        /*
        
        public static Char_To_Decoded_Char_Map(){
            var builder = createBuilder();
            builder.addRange('A', 'Z', 0);
            builder.addRange('2', '7', 26);
            return builder.map;
        }
    
        public static byte DecodeChar(byte c){
            var charMap = Char_To_Decoded_Char_Map();
            var decodedChar = charMap[c];
            if (null != decodedChar) {
                return decodedChar;
            }
            throw new Exception("illegal base32 character " + c);
        };

        public static void DecodeBlock(byte[] input, int inputOffset, byte[] output, int outputOffset)
        {
            var bytes = new byte[Encoded_Block_Size];
        for (var i = 0; i < Encoded_Block_Size; ++i) {
                bytes[i] = DecodeChar(input[inputOffset + i]);
            }

            output[outputOffset + 0] = (byte)((bytes[0] << 3) | (bytes[1] >> 2));
            output[outputOffset + 1] = (byte)(((bytes[1] & 0x03) << 6) | (bytes[2] << 1) | (bytes[3] >> 4));
            output[outputOffset + 2] = (byte)(((bytes[3] & 0x0f) << 4) | (bytes[4] >> 1));
            output[outputOffset + 3] = (byte)(((bytes[4] & 0x01) << 7) | (bytes[5] << 2) | (bytes[6] >> 3));
            output[outputOffset + 4] = (byte)(((bytes[6] & 0x07) << 5) | bytes[7]);
        }
        */
    }
}