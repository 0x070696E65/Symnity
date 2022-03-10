using System.Numerics;

namespace Symnity.Model.BlockChain
{
    public class BlockDuration
    {
        /**
        * The duration in blocks a mosaic will be available. After the duration finishes mosaic is
        * inactive and can be renewed. Duration is required when defining the mosaic
        */
        private long duration;

        /**
        * Constructor.
        *
        * @param duration long
        */
        public BlockDuration(long duration)
        {
            this.duration = duration;
        }

        /**
        * Constructor.
        *
        * @param duration BigInteger
        */
        public BlockDuration(BigInteger duration)
        {
            this.duration = (long) duration;
        }

        /**
        * Returns the number of blocks from height it will be active
        *
        * @return long duration
        */
        public long GetDuration()
        {
            return duration;
        }

        /**
        * Returns the duration as string.
        *
        * @return String duration
        */
        public override string ToString()
        {
            return duration.ToString();
        }
    }
}