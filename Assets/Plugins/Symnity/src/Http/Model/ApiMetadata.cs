using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Symnity.Infrastructure.SearchCriteria;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiMetadata : MonoBehaviour
    {
        public static async UniTask<MetadataDatum> GetMetadataInformation(string node, string compositeHash, bool log = false)
        {
            var url = "/metadata/" + compositeHash;
            if(log) Debug.Log($@"<a href=""{node}{url}"">{node}{url}</a>");
            var metadataDatumStr = await HttpUtilities.GetDataFromApiString(node, url);
            var metadataDatum = JsonUtility.FromJson<MetadataDatum>(metadataDatumStr);
            return metadataDatum;
        }
        
        public static async UniTask<MetadataRoot> SearchMetadata(string node, MetadataSearchCriteria query)
        {
            var param = "?";
            if (query.SourceAddress != null) param += "&sourceAddress=" + query.SourceAddress.Plain();
            if (query.TargetAddress != null) param += "&targetAddress=" + query.TargetAddress.Plain();
            if (query.ScopedMetadataKey != null) param += "&scopedMetadataKey=" + query.ScopedMetadataKey;
            if (query.TargetId != null) param += "&targetId=" + query.TargetId.GetIdAsHex();
            if (query.MetadataType != 0) param += "&metadataType=" + query.MetadataType;
            if (query.PageSize != 20) param += "&pageSize=" + query.PageSize;
            if (query.PageNumber != 1) param += "&pageNumber=" + query.PageNumber;
            if (query.Offset != null) param += "&offset=" + query.Offset;
            var order = query.Order == Order.Asc ? "asc" : "desc";
            param += "&order=" + order;
            var url = "/metadata" + param;
            Debug.Log($@"<a href=""{node}{url}"">{node}{url}</a>");
            var accountRootData = await HttpUtilities.GetDataFromApiString(node, url); ;
            var root = JsonUtility.FromJson<MetadataRoot>(accountRootData);
            return root;
        }
        
        [Serializable]
        public class MetadataEntry
        {
            public int version;
            public string compositeHash;
            public string sourceAddress;
            public string targetAddress;
            public string scopedMetadataKey;
            public string targetId;
            public int metadataType;
            public int valueSize;
            public string value;
        }

        [Serializable]
        public class MetadataDatum
        {
            public MetadataEntry metadataEntry;
            public string id;
        }

        [Serializable]
        public class MetadataPagination
        {
            public int pageNumber;
            public int pageSize;
        }

        [Serializable]
        public class MetadataRoot
        {
            public List<MetadataDatum> data;
            public MetadataPagination pagination;
        }
    }
}