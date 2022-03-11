using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiMetadata : MonoBehaviour
    {
        public class MetadataQueryParameters
        {
            public string sourceAddress;
            public string targetAddress;
            public string scopedMetadataKey;
            public string targetId;
            public int metadataType;
            public int pageSize;
            public int pageNumber;
            public string offset;
            public string order;
            public MetadataQueryParameters(
                string sourceAddress = null,
                string targetAddress = null,
                string scopedMetadataKey = null,
                string targetId = null,
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
            if (query.targetAddress != null) param += "&targetAddress=" + query.targetAddress;
            if (query.scopedMetadataKey != null) param += "&scopedMetadataKey=" + query.scopedMetadataKey;
            if (query.targetId != null) param += "&targetId=" + query.targetId;
            if (query.metadataType != 0) param += "&metadataType=" + query.metadataType;
            if (query.pageSize != 10) param += "&pageSize=" + query.pageSize;
            if (query.pageNumber != 1) param += "&pageNumber=" + query.pageNumber;
            if (query.offset != null) param += "&offset=" + query.offset;
            if (query.order != null) param += "&order=" + query.order;

            var url = "/metadata" + param;
            Debug.Log(url);
            
            var accountRootData = await HttpUtiles.GetDataFromApiString(node, url); ;
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