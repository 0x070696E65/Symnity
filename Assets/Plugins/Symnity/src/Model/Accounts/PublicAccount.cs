using System;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Network;

namespace Symnity.Model.Accounts
{
    public class PublicAccount
    {
        /**
         * The account's public key.
         */
        public readonly string PublicKey;

        /**
         * The account's address.
         */
        public readonly Address Address;

        /**
         * @internal
         * @param publicKey
         * @param address
         */
        public PublicAccount(string publicKey, Address address)
        {
            PublicKey = publicKey;
            Address = address;
        }
        
        /**
        * Create a PublicAccount from a public key and network type.
        * @param publicKey Public key
        * @param networkType Network type
        * @returns {PublicAccount}
        */
        public static PublicAccount CreateFromPublicKey(string publicKey, NetworkType networkType) {
            if (publicKey == null || (publicKey.Length != 64 && publicKey.Length != 66)) {
                throw new Exception("Not a valid public key");
            }
            var address = Address.CreateFromPublicKey(publicKey, networkType);
            return new PublicAccount(publicKey, address);
        }
        
        /**
         * Create Builder object
         */
        public PublicKeyDto ToBuilder() {
            return new PublicKeyDto(ConvertUtils.GetBytes(PublicKey));
        }
    }
}