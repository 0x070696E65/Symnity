using System.Numerics;

namespace Symnity.Model.Accounts
{
    public class ActivityBucket
    {
        /** Start height. */
        private BigInteger startHeight;

        /** Total fees paid. */
        private BigInteger totalFeesPaid;

        /** Beneficiary count. */
        private long beneficiaryCount;

        /** Raw score. */
        private BigInteger rawScore;

        /**
        * Constructor
        *
        * @param startHeight Total fees paid.
        * @param totalFeesPaid Total fees paid.
        * @param beneficiaryCount Beneficiary count.
        * @param rawScore Raw score.
        */
        public ActivityBucket(
            BigInteger startHeight,
            BigInteger totalFeesPaid,
            long beneficiaryCount,
            BigInteger rawScore)
        {
            this.startHeight = startHeight;
            this.totalFeesPaid = totalFeesPaid;
            this.beneficiaryCount = beneficiaryCount;
            this.rawScore = rawScore;
        }
    }
}