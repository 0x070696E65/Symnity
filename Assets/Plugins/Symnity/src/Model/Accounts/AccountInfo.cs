using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Symnity.Model.Mosaics;

namespace Symnity.Model.Accounts
{
    [Serializable]
    public class AccountInfo
    {
        /**
         * Version
         */
        public int version;

        /**
         * The database record id;
         */
        public string recordId;

        /**
         * Address of the account.
         */
        public Address address;

        /**
         * Height when the address was published.
         */
        public BigInteger addressHeight;

        /**
         * Public key of the account.
         */
        public string publicKey;

        /**
         * Height when the public key was published.
         */
        public BigInteger publicKeyHeight;

        /**
         * Account type
         */
        public AccountType accountType;

        /**
         * Account keys
         */
        public SupplementalPublicKeys supplementalAccountKeys;

        /**
         * Account activity bucket
         */
        public List<ActivityBucket> activityBuckets;

        /**
         * Mosaics held by the account.
         */
        public List<Mosaic> mosaics;

        /**
         * Importance of the account.
         */
        public Importance importance;

        /**
         * Importance height of the account.
         */
        public BigInteger importanceHeight;

        public AccountInfo(
            string recordId,
            int version,
            Address address,
            BigInteger addressHeight,
            string publicKey,
            BigInteger publicKeyHeight,
            BigInteger importance,
            BigInteger importanceHeight,
            List<Mosaic> mosaics,
            AccountType accountType,
            SupplementalPublicKeys supplementalAccountKeys = null,
            List<ActivityBucket> activityBuckets = null)
        {
            this.version = version;
            this.recordId = recordId;
            this.address = address;
            this.addressHeight = addressHeight;
            this.publicKey = publicKey;
            this.publicKeyHeight = publicKeyHeight;
            this.accountType = accountType;
            this.supplementalAccountKeys = supplementalAccountKeys;
            this.activityBuckets = activityBuckets;
            this.importance = new Importance(importance, importanceHeight);
            this.mosaics = mosaics;
        }
    }
}