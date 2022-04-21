using Symnity.Model.Accounts;
using Symnity.Model.Metadatas;
using Symnity.Model.Mosaics;

namespace Symnity.Infrastructure.SearchCriteria
{
    /**
 * Defines the params used to search metadata. With this criteria, you can sort and filter
 * metadata queries using rest.
 */
    public class MetadataSearchCriteria : SearchCriteria
    {
        public Address SourceAddress;
        public Address TargetAddress;
        public string ScopedMetadataKey;
        public MosaicId TargetId;
        public MetadataType MetadataType;
        public MetadataSearchCriteria(
            MetadataType metadataType = MetadataType.Account,
            Address sourceAddress = null,
            Address targetAddress = null,
            string scopedMetadataKey = null,
            MosaicId targetId = null,
            Order order = Order.Asc,
            int pageSize = 20,
            int pageNumber = 1,
            string? offset = null
        ): base(order, pageSize, pageNumber, offset)
        {
            MetadataType = metadataType;
            SourceAddress = sourceAddress;
            TargetAddress = targetAddress;
            ScopedMetadataKey = scopedMetadataKey;
            TargetId = targetId;
        }
    }
}