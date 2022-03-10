using System;

namespace Symnity.Model.Transactions
{
    /**
     * Static class containing transaction version constants.
     *
     * Transaction format versions are defined in catapult-server in
     * each transaction's plugin source code.
     *
     * In [catapult-server](https://github.com/nemtech/catapult-server), the `DEFINE_TRANSACTION_CONSTANTS` macro
     * is used to define the `TYPE` and `VERSION` of the transaction format.
     *
     * @see https://github.com/nemtech/catapult-server/blob/main/plugins/txes/transfer/src/model/TransferTransaction.h#L37
     */
    [Serializable]
    public class TransactionVersion
    {
        /**
        * Transfer Transaction transaction version.
        * @type {number}
        */
        public static readonly byte TRANSFER = 1;

        /**
        * Register namespace transaction version.
        * @type {number}
        */
        public static readonly byte NAMESPACE_REGISTRATION = 1;

        /**
        * Mosaic definition transaction version.
        * @type {number}
        */
        public static readonly byte MOSAIC_DEFINITION = 1;

        /**
        * Mosaic supply change transaction.
        * @type {number}
        */
        public static readonly byte MOSAIC_SUPPLY_CHANGE = 1;
        
        /**
      * Mosaic supply revocation transaction.
      * @type {number}
      */
        public static readonly byte MOSAIC_SUPPLY_REVOCATION = 1;

        /**
        * Modify multisig account transaction version.
        * @type {number}
        */
        public static readonly byte MULTISIG_ACCOUNT_MODIFICATION = 1;

        /**
        * Aggregate complete transaction version.
        * @type {number}
        */
        public static readonly byte AGGREGATE_COMPLETE = 1;

        /**
        * Aggregate bonded transaction version
        */
        public static readonly byte AGGREGATE_BONDED = 1;

        /**
        * Lock transaction version
        * @type {number}
        */
        public static readonly byte HASH_LOCK = 1;

        /**
        * Secret Lock transaction version
        * @type {number}
        */
        public static readonly byte SECRET_LOCK = 1;

        /**
        * Secret Proof transaction version
        * @type {number}
        */
        public static readonly byte SECRET_PROOF = 1;

        /**
        * Address Alias transaction version
        * @type {number}
        */
        public static readonly byte ADDRESS_ALIAS = 1;

        /**
        * Mosaic Alias transaction version
        * @type {number}
        */
        public static readonly byte MOSAIC_ALIAS = 1;

        /**
        * Mosaic global restriction transaction version
        * @type {number}
        */
        public static readonly byte MOSAIC_GLOBAL_RESTRICTION = 1;

        /**
        * Mosaic address restriction transaction version
        * @type {number}
        */
        public static readonly byte MOSAIC_ADDRESS_RESTRICTION = 1;

        /**
        * Account Restriction address transaction version
        * @type {number}
        */
        public static readonly byte ACCOUNT_ADDRESS_RESTRICTION = 1;

        /**
        * Account Restriction mosaic transaction version
        * @type {number}
        */
        public static readonly byte ACCOUNT_MOSAIC_RESTRICTION = 1;

        /**
        * Account Restriction operation transaction version
        * @type {number}
        */
        public static readonly byte MODIFY_ACCOUNT_RESTRICTION_ENTITY_TYPE = 1;

        /**
        * Link account transaction version
        * @type {number}
        */
        public static readonly byte ACCOUNT_KEY_LINK = 1;

        /**
        * Account metadata transaction version
        * @type {number}
        */
        public static readonly byte ACCOUNT_METADATA = 1;

        /**
        * Mosaic metadata transaction version
        * @type {number}
        */
        public static readonly byte MOSAIC_METADATA = 1;

        /**
        * Namespace metadata transaction version
        * @type {number}
        */
        public static readonly byte NAMESPACE_METADATA = 1;

        /**
         * Vrf key link transaction version.
         * @type {number}
         */
        public static readonly byte VRF_KEY_LINK = 1;

        /**
         * Voting key link transaction version.
         * @type {number}
         */
        public static readonly byte VOTING_KEY_LINK = 1;

        /**
         * Node key link transaction version.
         * @type {number}
         */
        public static readonly byte NODE_KEY_LINK = 1;
    }
}