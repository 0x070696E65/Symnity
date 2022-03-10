using System;
using System.Data;

namespace Symnity.Core.Format
{
    public class RawUInt64
    {
        /*
         * Parses a hex string into a uint64.
         * @param {string} input A hex encoded string.
         * @returns {module:coders/uint64~uint64} The uint64 representation of the input.
         */
        /*public static int[] FromHex(string input) {
            if (16 != input.Length) {
                throw new Exception("hex string has unexpected size " + input.Length);
            }
            var hexString = input;
            if (16 > hexString.Length) {
                hexString = new string('0', 16 - hexString.Length) + hexString;
            }
            var byteArray = ConvertUtils.GetBytes(hexString);
            var view = new DataView(byteArray.buffer);
            return [view.getUint32(4), view.getUint32(0)];
        }*/

        /**
         * Converts a numeric unsigned integer into a uint64.
         * @param {number} number The unsigned integer.
         * @returns {module:coders/uint64~uint64} The uint64 representation of the input.
         */
        public static ulong[] FromUint(ulong number)
        {
            var a = (number & 0xffffffff) >> 0;
            var b = (number / 0x100000000) >> 0;
            var value = new ulong[] {a, b};
        return value;
        }
    }
}