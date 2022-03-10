using System;
using Symnity.Core.Format;

namespace Symnity.Model
{
    public class Uint64
    {
        /**
         * uint64 lower part
         */
        private readonly ulong _lower;

        /**
         * uint64 higher part
         */
        private readonly ulong _higher;
        
        /**
         * Constructor
         * @param uintArray
         */
        public Uint64(ulong[] intArray) {
            if (intArray.Length != 2 || intArray[0] < 0 || intArray[1] < 0) {
                throw new Exception("uintArray must be be an array of two uint numbers");
            }
            _lower = intArray[0];
            _higher = intArray[1];
        }

        /**
         * Create from uint value
         * @param value
         * @returns {UInt64}
         */
        public static Uint64 FromUint(ulong value) {
            return new Uint64(RawUInt64.FromUint(value));
        }
        
        /**
         * Get DTO representation with format: `[lower, higher]`
         *
         * @returns {[number,number]}
         */
        public ulong[] ToDTO()
        {
            return new [] {_lower, _higher};
        }
    }
}