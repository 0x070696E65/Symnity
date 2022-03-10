namespace Symnity.Model.Metadatas
{
    public class Metadata
    {
        /**
         * The metadata id
         */
        public string id;

        /**
         * The metadata entry
         */
        public MetadataEntry metadataEntry;

        /**
         * Constructor
         * @param id - The metadata id
         * @param metadataEntry - The metadata entry
         */
        public Metadata(
            string id,
            MetadataEntry metadataEntry
        )
        {
            this.id = id;
            this.metadataEntry = metadataEntry;
        }
    }
}