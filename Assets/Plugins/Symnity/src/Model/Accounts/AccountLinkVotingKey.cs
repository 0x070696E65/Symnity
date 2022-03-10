namespace Symnity.Model.Accounts
{
    public class AccountLinkVotingKey
    {
        /**
         * public key
         */
        public string publicKey;

        /**
         * Start epoch
         */
        public int startEpoch;

        /**
         * End epoch
         */
        public int endEpoch;

        AccountLinkVotingKey(
            string publicKey,
            int startEpoch,
            int endEpoch
        )
        {
            this.publicKey = publicKey;
            this.startEpoch = startEpoch;
            this.endEpoch = endEpoch;
        }
    }
}