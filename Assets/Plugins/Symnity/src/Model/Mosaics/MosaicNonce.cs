using System;
using Symnity.Core.Crypto;

namespace Symnity.Model.Mosaics
{
    public class MosaicNonce
    {
        /**
         * Mosaic nonce
         */
        public readonly int nonce;

        /**
         * Create MosaicNonce from int
         *
         * @param nonce nonce
         */
        public MosaicNonce(int nonce) {
            this.nonce = nonce;
        }
        
        
        /** @return nonce int */
        public int GetNonceAsInt() {
            return nonce;
        }
        
        /**
         * Create a random MosaicNonce
         *
         * @return  {MosaicNonce}
         */
        public static MosaicNonce CreateRandom(byte length) {
            var nonce = Crypto.RandomBytes(length);
            return new MosaicNonce(BitConverter.ToInt32(nonce, 0));
        }
    }
}