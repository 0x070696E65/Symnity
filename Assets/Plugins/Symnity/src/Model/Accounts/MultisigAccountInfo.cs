using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Symbol.Builders;
using Symnity.Model.Mosaics;

namespace Symnity.Model.Accounts
{
    [Serializable]
    public class MultisigAccountInfo
    {
        /**
         * Version
         */
        public int version;

        /**
         * The account multisig address.
         */
        public Address accountAddress;

        /**
         * The number of signatures needed to approve a transaction.
         */
        public int minApproval;

        /**
         * The number of signatures needed to remove a cosignatory.
         */
        public int minRemoval;

        /**
         * The multisig account cosignatories.
         */
        public List<Address> cosignatoryAddresses;

        /**
         * The multisig accounts this account is cosigner of.
         */
        public List<Address> multisigAddresses;

        public MultisigAccountInfo(
            int version,
            Address accountAddress,
            int minApproval,
            int minRemoval,
            List<Address> cosignatoryAddresses,
            List<Address> multisigAddresses
            )
        {
            this.version = version;
            this.accountAddress = accountAddress;
            this.minApproval = minApproval;
            this.minRemoval = minRemoval;
            this.cosignatoryAddresses = cosignatoryAddresses;
            this.multisigAddresses = multisigAddresses;
        }
        
        /**
         * Checks if the account is a multisig account.
         * @returns {boolean}
         */
        public bool IsMultisig() {
         return minRemoval != 0 && minApproval != 0;
        }
        
        /**
         * Checks if an account is cosignatory of the multisig account.
         * @param address
         * @returns {boolean}
         */
        public bool HasCosigner(Address address) { 
            return cosignatoryAddresses.Find(cosigner => cosigner.Equals(address)) != null;
        }
        
        /**
         * Checks if the multisig account is cosignatory of an account.
         * @param address
         * @returns {boolean}
         */
        public bool IsCosignerOfMultisigAccount(Address address) {
            return multisigAddresses.Find(multisig => multisig.Equals(address)) != null;
        }
    }
}