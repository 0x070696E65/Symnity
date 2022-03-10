using Symnity.Model.Metadatas;

namespace Symnity.Http.Model
{
    public struct MetadataSearchCriteria
    {
        public readonly MetadataType MetadataType;
        public readonly string SourceAddress;
        public readonly string ScopedMetadataKey;
        public readonly string Id;
        public readonly int PageSize;
        public readonly int PageNumber;
        public MetadataSearchCriteria(
            MetadataType metadataType,
            string sourceAddress,
            string scopedMetadataKey = "",
            string id = "",
            int pageSize = 20,
            int pageNumber = 1
            
        )
        {
            MetadataType = metadataType;
            SourceAddress = sourceAddress;
            ScopedMetadataKey = scopedMetadataKey;
            Id = id;
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}