using System;
using System.Numerics;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;

namespace Symnity.Model.Metadatas
{
    public class MetadataEntry
    {
        /**
         * Version
         */
        public int version;

        /**
         * The composite hash
         */
        public string compositeHash;

        /**
         * The metadata source address (provider)
         */
        public Address sourceAddress;

        /**
         * The metadata target address
         */
        public Address targetAddress;

        /**
         * The key scoped to source, target and type
         */
        public BigInteger scopedMetadataKey;

        /**
         * The metadata type
         */
        public MetadataType metadataType;

        /**
         * The metadata value
         */
        public string value;

        /**
         * The target mosaic or namespace identifier
         */
        public Id targetId;

        /**
        * Constructor
        * @param {number} version - The version
        * @param {string} compositeHash - The composite hash
        * @param {string} sourceAddress - The metadata source address (provider)
        * @param {string} targetAddress - The metadata target address
        * @param {UInt64} scopedMetadataKey - The key scoped to source, target and type
        * @param {MetadatType} metadataType - The metadata type (Account | Mosaic | Namespace)
        * @param {string} value - The metadata value
        * @param {UnresolvedMosaicId | undefined} targetId - The target mosaic or namespace identifier
        */
        public MetadataEntry(
            int version,
            string compositeHash,
            Address sourceAddress,
            Address targetAddress,
            BigInteger scopedMetadataKey,
            MetadataType metadataType,
            string value,
            Id targetId = null
        )
        {
            this.version = version;
            this.compositeHash = compositeHash;
            this.sourceAddress = sourceAddress;
            this.targetAddress = targetAddress;
            this.scopedMetadataKey = scopedMetadataKey;
            this.metadataType = metadataType;
            this.value = value;
            this.targetId = targetId;
        }
    }
}