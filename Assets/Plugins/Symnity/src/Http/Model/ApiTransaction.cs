using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Symnity.Http.Model
{
    public class ApiTransaction : MonoBehaviour
    {
        public static async UniTask<Datum> GetConfirmedTransaction(string node, string hash)
        {
            var url = "/transactions/confirmed/" + hash;
            var transactionRootData = await HttpUtiles.GetDataFromApiString(node, url);
            var root = JsonUtility.FromJson<Datum>(transactionRootData);
            return root;
        }

        public class TransactionQueryParameters
        {
            public readonly string address;
            public readonly string recipientAddress;
            public readonly string signerPublicKey;
            public readonly string height;
            public readonly string fromHeight;
            public readonly string toHeight;
            public readonly string fromTransferAmount;
            public readonly string toTransferAmount;
            public readonly List<int> type;
            public readonly bool embedded;
            public readonly string transferMosaicId;
            public readonly int pageSize;
            public readonly int pageNumber;
            public readonly string offset;
            public readonly string order;

            public TransactionQueryParameters(
                string address = null,
                string recipientAddress = null,
                string signerPublicKey = null,
                string height = null,
                string fromHeight = null,
                string toHeight = null,
                string fromTransferAmount = null,
                string toTransferAmount = null,
                List<int> type = null,
                bool embedded = false,
                string transferMosaicId = null,
                int pageSize = 10,
                int pageNumber = 1,
                string offset = null,
                string order = null
            )
            {
                this.address = address;
                this.recipientAddress = recipientAddress;
                this.signerPublicKey = signerPublicKey;
                this.height = height;
                this.fromHeight = fromHeight;
                this.toHeight = toHeight;
                this.fromTransferAmount = fromTransferAmount;
                this.toTransferAmount = toTransferAmount;
                this.type = type;
                this.embedded = embedded;
                this.transferMosaicId = transferMosaicId;
                this.pageSize = pageSize;
                this.pageNumber = pageNumber;
                this.offset = offset;
                this.order = order;
            }
        }

        public static async UniTask<Root> SearchConfirmedTransactions(string node, TransactionQueryParameters query)
        {
            var param = "?";
            if (query.address != null) param += "&address=" + query.address;
            if (query.recipientAddress != null) param += "&recipientAddress=" + query.recipientAddress;
            if (query.signerPublicKey != null) param += "&signerPublicKey=" + query.signerPublicKey;
            if (query.height != null) param += "&height=" + query.height;
            if (query.fromHeight != null) param += "&fromHeight=" + query.fromHeight;
            if (query.toHeight != null) param += "&toHeight=" + query.toHeight;
            if (query.fromTransferAmount != null) param += "&fromTransferAmount=" + query.fromTransferAmount;
            if (query.toTransferAmount != null) param += "&toTransferAmount=" + query.toTransferAmount;
            if (query.type != null) param += "&type=" + query.type;
            if (query.embedded) param += "&embedded=" + query.embedded;
            if (query.transferMosaicId != null) param += "&transferMosaicId=" + query.transferMosaicId;
            if (query.pageSize != 10) param += "&pageSize=" + query.pageSize;
            if (query.pageNumber != 1) param += "&pageNumber=" + query.pageNumber;
            if (query.offset != null) param += "&offset=" + query.offset;
            if (query.order != null) param += "&order=" + query.order;

            var url = "/transactions/confirmed" + param;
            Debug.Log(url);
            var transactionRootData = await HttpUtiles.GetDataFromApiString(node, url);
            var root = JsonUtility.FromJson<Root>(transactionRootData);
            return root;
        }
        
        public static async UniTask<Root> SearchUnConfirmedTransactions(string node, TransactionQueryParameters query)
        {
            var param = "?";
            if (query.address != null) param += "&address=" + query.address;
            if (query.recipientAddress != null) param += "&recipientAddress=" + query.recipientAddress;
            if (query.signerPublicKey != null) param += "&signerPublicKey=" + query.signerPublicKey;
            if (query.height != null) param += "&height=" + query.height;
            if (query.fromHeight != null) param += "&fromHeight=" + query.fromHeight;
            if (query.fromTransferAmount != null) param += "&fromTransferAmount=" + query.fromTransferAmount;
            if (query.toTransferAmount != null) param += "&toTransferAmount=" + query.toTransferAmount;
            if (query.type != null) param += "&type=" + query.type;
            if (query.embedded != null) param += "&embedded=" + query.embedded;
            if (query.transferMosaicId != null) param += "&transferMosaicId=" + query.transferMosaicId;
            if (query.pageSize != null) param += "&pageSize=" + query.pageSize;
            if (query.pageNumber != null) param += "&pageNumber=" + query.pageNumber;
            if (query.offset != null) param += "&offset=" + query.offset;
            if (query.order != null) param += "&order=" + query.order;

            var url = "/transactions/unconfirmed" + param;
            var transactionRootData = await HttpUtiles.GetDataFromApiString(node, url);
            var root = JsonUtility.FromJson<Root>(transactionRootData);
            return root;
        }

        public static async UniTask<Datum> GetUnConfirmedTransaction(string node, string id)
        {
            var url = "/transactions/unconfirmed/" + id;
            var datumString = await HttpUtiles.GetDataFromApiString(node, url);
            return JsonUtility.FromJson<Datum>(datumString);
        }

        [Serializable]
        public class Meta
        {
            public string height;
            public string hash;
            public string merkleComponentHash;
            public int index;
            public string timestamp;
            public int feeMultiplier;
        }

        [Serializable]
        public class Transaction
        {
            public int size;
            public string signature;
            public string signerPublicKey;
            public int version;
            public int network;
            public int type;
            public string maxFee;
            public string deadline;
            public string recipientAddress;
            public string message;
            public List<ApiMosaic> mosaics;
            public List<AggTransaction> transactions;
            public List<string> cosignatures;
        }
        
        [Serializable]
        public class AggTransaction
        {
            public Meta meta;
            public AggInnerTransaction transaction;
            public string id;
        }
        
        [Serializable]
        public class AggInnerTransaction
        {
            public int size;
            public string signature;
            public string signerPublicKey;
            public int version;
            public int network;
            public int type;
            public string maxFee;
            public string deadline;
            public string recipientAddress;
            public string message;
            public List<ApiMosaic> mosaics;
            public string targetAddress;
            public string scopedMetadataKey;
            public int valueSizeDelta;
            public int valueSize;
            public string value;
        }

        [Serializable]
        public class ApiMosaic
        {
            public string id;
            public int amount;

            public ApiMosaic(string id, int amount)
            {
                this.id = id;
                this.amount = amount;
            }
        }

        [Serializable]
        public class Datum
        {
            public Meta meta;
            public Transaction transaction;
            public string id;
        }

        [Serializable]
        public class Pagination
        {
            public int pageNumber;
            public int pageSize;
        }

        [Serializable]
        public class Root
        {
            public List<Datum> data;
            public Pagination pagination;
        }
    }
}
