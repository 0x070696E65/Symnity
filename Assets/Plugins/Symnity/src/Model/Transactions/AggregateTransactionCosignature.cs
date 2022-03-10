using Symnity.Model.Accounts;

namespace Symnity.Model.Transactions
{
    public class AggregateTransactionCosignature
    {
        /*
        * The signature of aggregate transaction done by the cosigner.
        */
        public readonly string Signature;

        /*
        * The cosigner public account.
        */
        public readonly PublicAccount Signer;

        /*
        * Version
        */
        public readonly long Version = 0;

        /*
         * @param signature
         * @param signer
         * @param version
         */
        public AggregateTransactionCosignature(
            string signature,
            PublicAccount signer,
            long version
        )
        {
            Signature = signature;
            Signer = signer;
            Version = version;
        }
    }
}