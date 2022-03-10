using System;
using System.Numerics;
using System.Globalization;
using Symnity.Core.Format;
using Symnity.Model.Accounts;

namespace Symnity.Model.Mosaics
{
    /**
     * The mosaic id structure describes mosaic id
     *
     * @since 1.0
     */
    [Serializable]
    public class MosaicId : UnresolvedMosaicId
    {
        /**
         * Mosaic id
         */
        public readonly BigInteger　Id;
        
        public MosaicId(MosaicNonce mosaicNonce, Address owner)
        {
            Id =
                IdGenerator.GenerateMosaicId(
                    mosaicNonce.GetNonceAsInt(), Base32.FromBase32String(owner.Plain()));
        }
        
        /**
         * Create a MosaicId for given `nonce` MosaicNonce and `owner` PublicAccount.
         *
         * @param   nonce   {MosaicNonce}
         * @param   ownerAddress   {Address}
         * @return  {MosaicId}
         */
        public static MosaicId CreateFromNonce(MosaicNonce nonce, Address ownerAddress) {
            return new MosaicId(nonce, ownerAddress);
        }
        
        /**
         * Create MosaicId from mosaic id in form of array of number (ex: [3646934825, 3576016193])
         * or the hexadecimal notation thereof in form of a string.
         *
         * @param id
         */
        public MosaicId(string id) {
            if (id == null) {
                throw new Exception("MosaicId undefined");
            }
            // hexadecimal formatted MosaicId
            Id = BigInteger.Parse(id, NumberStyles.HexNumber);
        }
        
        public MosaicId(long id) {
            Id = id;
        }
        
        /**
        * Returns mosaic BigInteger id
        *
        * @return mosaic BigInteger id
        */
        public BigInteger GetId() {
            return Id;
        }

        /**
        * Returns mosaic id as a long
        *
        * @return id long
        */
        public long GetIdAsLong()
        {
            return (long) Id;
        }
        
        /**
        * Gets the id as a hexadecimal string.
        *
        * @return Hex id.
        */
        public string GetIdAsHex() {
            var bytes = GetId().ToByteArray();
            return ConvertUtils.ToHex(bytes);
        }
    }
}