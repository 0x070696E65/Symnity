using System;
using System.Numerics;

namespace Symnity.Model.Mosaics
{
    /**
 * A mosaic describes an instance of a mosaic definition.
 * Mosaics can be transferred by means of a transfer transaction.
 */
    [Serializable]
    public class Mosaic
    {
        /**
         * The mosaic id
         */
        // とりあえずNameSpaceを無視
        public readonly UnresolvedMosaicId Id;

        /**
         * The mosaic amount. The quantity is always given in smallest units for the mosaic
         * i.e. if it has a divisibility of 3 the quantity is given in millis.
         */
        public readonly long Amount;
        /*
         * Constructor
         * @param id
         * @param amount
         */
        public Mosaic(UnresolvedMosaicId id, long amount)
        {
            Id = id;
            Amount = amount;
        }
        
        /**
        * Returns the mosaic identifier
        *
        * @return mosaic identifier
        */
        public UnresolvedMosaicId GetId() {
            return Id;
        }
    }
}