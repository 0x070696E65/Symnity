using System;
using Symbol.Builders;
using Symnity.Core.Format;
using Symnity.Model.Network;

namespace Symnity.Model.Accounts
{
    [Serializable]
    public class Address : UnresolvedAddress
    {
        /**
         * @internal
         * @param address
         * @param networkType
         */

        /**
         * The address value.
         */
        private readonly string _address;
        /**
         * The NEM network type.
         */
        public readonly NetworkType NetworkType;

        private Address(string address, NetworkType networkType)
        {
            _address = address;
            NetworkType = networkType;
        }
        
        /**
         * Create from private key
         * @param publicKey - The account public key.
         * @param networkType - The NEM network type.
         * @returns {Address}
         */
        public static Address CreateFromPublicKey(string publicKey, NetworkType networkType) {
            var publicKeyBytes = ConvertUtils.GetBytes(publicKey);
            var address = RawAddress.AddressToString(RawAddress.PublicKeyToAddress(publicKeyBytes, networkType));
            return new Address(address, networkType);
        }
        
        /**
         * Create an Address from a given raw address.
         * @param rawAddress - Address in string format.
         *                  ex: SB3KUBHATFCPV7UZQLWAQ2EUR6SIHBSBEOEDDDF3 or SB3KUB-HATFCP-V7UZQL-WAQ2EU-R6SIHB-SBEOED-DDF3
         * @returns {Address}
         */
        public static Address CreateFromRawAddress(string rawAddress) {
            NetworkType networkType;
            var addressTrimAndUpperCase = rawAddress.Trim().ToUpper().Replace("/-/g", "");
            if (addressTrimAndUpperCase.Length != 39) {
                throw new Exception("Address " + addressTrimAndUpperCase + " has to be 39 characters long");
            }
            if (addressTrimAndUpperCase[0] == 'T') {
                networkType = NetworkType.TEST_NET;
            } else if (addressTrimAndUpperCase[0] == 'N') {
                networkType = NetworkType.MAIN_NET;
            } else {
                throw new Exception("Address Network unsupported");
            }
            return new Address(addressTrimAndUpperCase, networkType);
        }
        
        /**
         * Get address in the encoded format ex: NAR3W7B4BCOZSZMFIZRYB3N5YGOUSWIYJCJ6HDFH.
         * @returns {string}
         */
        public string Encoded() {
            return ConvertUtils.ToHex(RawAddress.StringToAddress(_address));
        }
        
        /**
         * Encoded address or namespace id. Note that namespace id get the hex reversed and
         * zero padded.
         * @returns {Uint8Array}
         */
        public byte[] EncodeUnresolvedAddress() {
            return ConvertUtils.GetBytes(Encoded());
        }
        
        /**
         * Get address in plain format ex: SB3KUBHATFCPV7UZQLWAQ2EUR6SIHBSBEOEDDDF3.
         * @returns {string}
         */
        public string Plain() {
            return _address;
        }
    }
}