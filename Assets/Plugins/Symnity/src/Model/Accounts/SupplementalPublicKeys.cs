using System.Collections.Generic;

namespace Symnity.Model.Accounts
{
    public class SupplementalPublicKeys
    {
        /**
         * Linked keys
         */
        public AccountLinkPublicKey linked;

        /**
         * Node linked keys
         */
        public AccountLinkPublicKey node;

        /**
         * VRF linked keys
         */
        public AccountLinkPublicKey vrf;

        /**
         * Voting linked keys
         */
        public List<AccountLinkVotingKey> voting;

        public SupplementalPublicKeys(
            AccountLinkPublicKey linked = null,
            AccountLinkPublicKey node = null,
            AccountLinkPublicKey vrf = null,
            List<AccountLinkVotingKey> voting = null
        )
        {
            this.linked = linked;
            this.node = node;
            this.vrf = vrf;
            this.voting = voting;
        }
    }
}