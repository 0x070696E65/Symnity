using System;
using System.Collections.Generic;
using Symnity.Core.Crypto;
using Symnity.Core.Format;
using Symnity.Model.Messages;
using Symnity.Model.Network;
using Symnity.Model.Transactions;

namespace Symnity.Model.Accounts
{
    /**
    * The account structure describes an account private key, public key, address and allows signing transactions.
    */
    [Serializable]
    public class Account
    {
        /*
         * @internal
         * @param address
         * @param keyPair
         */
        /**
         * The account address.
         */
        public readonly Address Address;

        /**
         * The account keyPair, public and private key.
         */
        public readonly KeyPair KeyPair;

        /**
         * Create an Account from a given private key
         * @param privateKey - Private key from an account
         * @param networkType - Network type
         * @return {Account}
         */
        public Account(Address address, KeyPair keyPair)
        {
            Address = address;
            KeyPair = keyPair;
        }
        
        /**
         * Create an Account from a given private key
         * @param privateKey - Private key from an account
         * @param networkType - Network type
         * @return {Account}
         */
        public static Account CreateFromPrivateKey(string privateKey, NetworkType networkType ) {
            var keyPair = KeyPair.CreateKeyPairFromPrivateKeyString(privateKey);
            var address = RawAddress.AddressToString(RawAddress.PublicKeyToAddress(keyPair.publicKey, networkType));
            return new Account(Address.CreateFromRawAddress(address), keyPair);
        }
        
        /**
         * Generate a new account
         * @param networkType - Network type
         */
        public static Account GenerateNewAccount(NetworkType networkType) {
            // Create random bytes
            var randomBytesArray = Crypto.RandomBytes(32);
            // Hash random bytes with entropy seed
            // Finalize and keep only 32 bytes
            var hashKey = ConvertUtils.ToHex(randomBytesArray);

            // Create KeyPair from hash key
            var keyPair = KeyPair.CreateKeyPairFromPrivateKeyString(hashKey);

            var address = Address.CreateFromPublicKey(ConvertUtils.ToHex(keyPair.publicKey), networkType);
            return new Account(address, keyPair);
        }
        
        /**
         * Create a new encrypted Message
         * @param message - Plain message to be encrypted
         * @param recipientPublicAccount - Recipient public account
         * @returns {EncryptedMessage}
         */
        public EncryptedMessage EncryptMessage(string message, PublicAccount recipientPublicAccount) {
            return EncryptedMessage.Create(message, recipientPublicAccount, KeyPair);
        }

        /**
         * Decrypts an encrypted message
         * @param encryptedMessage - Encrypted message
         * @param publicAccount - The public account originally encrypted the message
         * @returns {PlainMessage}
         */
        public PlainMessage DecryptMessage(EncryptedMessage encryptedMessage, PublicAccount publicAccount) {
            return EncryptedMessage.Decrypt(encryptedMessage, KeyPair, publicAccount);
        }
        
        /**
         * Account public key.
         * @return {string}
         */
        public string GetPublicKey() {
            return ConvertUtils.ToHex(KeyPair.publicKey);
        }
        
        /**
        * Public account.
        * @return {PublicAccount}
        */
        public PublicAccount GetPublicAccount() {
            return PublicAccount.CreateFromPublicKey(GetPublicKey(), Address.NetworkType);
        }
        
        /**
         * Account private key.
         * @return {string}
         */
        public string GetPrivateKey() {
            return ConvertUtils.ToHex(KeyPair.privateKey);
        }
        
        /**
         * Sign a transaction
         * @param transaction - The transaction to be signed.
         * @param generationHash - Network generation hash hex
         * @return {SignedTransaction}
         */
        public SignedTransaction Sign(Transaction transaction, string generationHash) {
            return transaction.SignWith(this, generationHash);
        }
        
        /**
         * Sign transaction with cosignatories creating a new SignedTransaction
         * @param transaction - The aggregate transaction to be signed.
         * @param cosignatories - The array of accounts that will cosign the transaction
         * @param generationHash - Network generation hash hex
         * @return {SignedTransaction}
         */
        public SignedTransaction SignTransactionWithCosignatories(
            AggregateTransaction transaction, 
            List<Account> cosignatories,
            string generationHash
        ) {
            return transaction.SignTransactionWithCosignatories(this, cosignatories, generationHash);
        }
        
        /**
         * Sign transaction with cosignatories collected from cosigned transactions and creating a new SignedTransaction
         * For off chain Aggregated Complete Transaction co-signing.
         * @param initiatorAccount - Initiator account
         * @param {CosignatureSignedTransaction[]} cosignatureSignedTransactions - Array of cosigned transaction
         * @param generationHash - Network generation hash hex
         * @return {SignedTransaction}
         */
        public SignedTransaction SignTransactionGivenSignatures(
            AggregateTransaction transaction,
            CosignatureSignedTransaction[] cosignatureSignedTransactions,
            string generationHash
        ) {
            return transaction.SignTransactionGivenSignatures(this, cosignatureSignedTransactions, generationHash);
        }
    }
}