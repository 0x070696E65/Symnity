using System;

namespace Symnity.Core.Format
{
    [Serializable]
    public class RawArray
    {
        /*
         * Copies elements from a source array to a destination array.
         * @param {Array} dest The destination array.
         * @param {Array} src The source array.
         * @param {number} [numElementsToCopy=undefined] The number of elements to copy.
         * @param {number} [destOffset=0] The first index of the destination to write.
         * @param {number} [srcOffset=0] The first index of the source to read.
         */
        public static void Copy(byte[] dest, byte[] src, int? numElementsToCopy, int destOffset = 0, int srcOffset = 0)
        {
            int? length;
            if (numElementsToCopy == null)
            {
                length = dest.Length;
            }
            else
            {
                length = numElementsToCopy;
            }
            for (var i = 0; i < length; ++i) {
                dest[destOffset + i] = src[srcOffset + i];
            }
        }
    }
}