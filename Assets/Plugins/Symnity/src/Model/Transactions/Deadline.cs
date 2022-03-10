using System;

namespace Symnity.Model.Transactions
{
    /**
     * The deadline of the transaction. The deadline is given as the number of seconds elapsed since the creation of the nemesis block.
     * If a transaction does not get included in a block before the deadline is reached, it is deleted.
     */
    [Serializable]
    public class Deadline
    {
        const byte defaultDeadline = 2;

        /**
         * Deadline value (without Nemesis epoch adjustment)
         */
        public long AdjustedValue;
        
        /**
         * Create deadline model. Default to 2 chrono hours in advance.
         * @param {number} epochAdjustment the network's epoch adjustment (seconds). Defined in the network/properties. e.g. 1573430400;
         * @param {number} deadline the deadline unit value.
         * @param {ChronoUnit} chronoUnit the crhono unit. e.g ChronoUnit.HOURS
         * @returns {Deadline}
         */
        public static Deadline Create(int epochAdjustment, byte deadline = defaultDeadline){ //, chronoUnit: ChronoUnit = defaultChronoUnit) {
            var deadlineDateTime = DateTime.Now.ToUniversalTime().AddHours(defaultDeadline).AddSeconds(-epochAdjustment);
            var unixOriginTime = new DateTime(1970, 1, 1, 0, 0, 0);
 
            if (deadline <= 0) {
                throw new Exception("deadline should be greater than 0");
            }
            
            return CreateFromAdjustedValue((long)Math.Floor((deadlineDateTime - unixOriginTime).TotalMilliseconds));
        }
        
        /**
         *
         * Create a Deadline where the adjusted values was externally calculated.
         *
         * @returns {Deadline}
        */
        public static Deadline CreateFromAdjustedValue(long adjustedValue) {
            return new Deadline(adjustedValue);
        }
        
        /*
         * @param value
         * @returns {Deadline}
         */
        /*public static Deadline CreateFromDTO<T>(T value) {
            var uint64Value = typeof(T) == typeof(string) ? UInt64.fromNumericString(value) : new UInt64(value);
            return new Deadline(uint64Value.compact());
        }*/
        
        /**
         * Constructor
         * @param adjustedValue Adjusted value. (Local datetime minus nemesis epoch adjustment)
         */
        private Deadline(long adjustedValue) {
            AdjustedValue = adjustedValue;
        }
    }
}