using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Symnity.Core.Format;
using Symnity.Http.Model;
using Symnity.Infrastructure.SearchCriteria;
using Symnity.Model.Accounts;
using Symnity.Model.Metadatas;
using Symnity.Model.Mosaics;

namespace Symnity.Infrastructure
{
    public class MetadataRepository
    {
        private string Node;
        public MetadataRepository(string node)
        {
            Node = node;
        }

        public async UniTask<Metadata> GetMetadata(string compositeHash)
        {
            var info = await ApiMetadata.GetMetadataInformation(Node, compositeHash);
            return GetMetadataInformation(info);
        }

        private static Metadata GetMetadataInformation(ApiMetadata.MetadataDatum info)
        {
            var metadataEntry = new MetadataEntry(
                info.metadataEntry.version,
                info.metadataEntry.compositeHash,
                Address.CreateFromEncoded(info.metadataEntry.sourceAddress),
                Address.CreateFromEncoded(info.metadataEntry.targetAddress),
                BigInteger.Parse(info.metadataEntry.scopedMetadataKey, NumberStyles.AllowHexSpecifier),
                (MetadataType) Enum.ToObject(typeof(MetadataType), info.metadataEntry.metadataType),
                ConvertUtils.HexToChar(info.metadataEntry.value),
                new MosaicId(info.metadataEntry.targetId)
            );
            return new Metadata(
                info.id,
                metadataEntry);
        }
        
        public async UniTask<Page<Metadata>> Search(MetadataSearchCriteria searchCriteria)
        {
            var result = await ApiMetadata.SearchMetadata(Node, searchCriteria);
            var list = result.data.Select(GetMetadataInformation).ToList();
            return new Page<Metadata>(
                list,
                result.pagination.pageNumber,
                result.pagination.pageSize
            );
        }
    }
}