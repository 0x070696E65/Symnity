namespace Symnity.Model.Accounts
{
    public class AccountLinkPublicKey
    {
        /**
         * public key
         */
        public string PublicKey;
        
        public AccountLinkPublicKey(
            string publicKey
        )
        {
            PublicKey = publicKey;
        }
    }
}