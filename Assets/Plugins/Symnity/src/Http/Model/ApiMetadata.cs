using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json.Linq;
using Symnity.Core.Format;
using Symnity.Model;
using Symnity.Model.Accounts;
using Symnity.Model.Metadatas;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiMetadata : MonoBehaviour
    {
        public struct MetadataQueryParameters
        {
            public readonly string sourceAddress;
            public readonly string targetAddress;
            public readonly string scopedMetadataKey;
            public readonly string targetId;
            public readonly int metadataType;
            public readonly int pageSize;
            public readonly int pageNumber;
            public readonly string offset;
            public readonly string order;
            public MetadataQueryParameters(
                string sourceAddress = "",
                string targetAddress = "",
                string scopedMetadataKey = "",
                string targetId = "",
                int metadataType = 0,
                int pageSize = 10,
                int pageNumber = 1,
                string offset = null,
                string order = null
            )
            {
                this.sourceAddress = sourceAddress;
                this.targetAddress = targetAddress;
                this.scopedMetadataKey = scopedMetadataKey;
                this.targetId = targetId;
                this.metadataType = metadataType;
                this.pageSize = pageSize;
                this.pageNumber = pageNumber;
                this.offset = offset;
                this.order = order;
            }
        }
        
        public static async UniTask<MetadataRoot> SearchMetadata(string node, MetadataQueryParameters query)
        {
            var param = "?";
            if (query.sourceAddress != null) param += "&sourceAddress=" + query.sourceAddress;
            if (query.targetAddress != null) param += "&sourceAddress=" + query.targetAddress;
            if (query.scopedMetadataKey != null) param += "&sourceAddress=" + query.scopedMetadataKey;
            if (query.targetId != null) param += "&sourceAddress=" + query.targetId;
            if (query.metadataType != 0) param += "&sourceAddress=" + query.metadataType;
            if (query.pageSize != 10) param += "&pageSize=" + query.pageSize;
            if (query.pageNumber != 1) param += "&pageNumber=" + query.pageNumber;
            if (query.offset != null) param += "&offset=" + query.offset;
            if (query.order != null) param += "&order=" + query.order;

            var url = "/metadata" + param;
            var accountRootData = await HttpUtiles.GetDataFromApiString(node, url);
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