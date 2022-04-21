using System;
using System.Collections.Generic;
using System.Numerics;
using Cysharp.Threading.Tasks;
using Symnity.Infrastructure.SearchCriteria;
using Symnity.Model.Accounts;
using UnityEngine;

namespace Symnity.Http.Model
{
    public class ApiTransaction : MonoBehaviour
    {
        public static async UniTask<Datum> GetConfirmedTransaction(string node, string hash)
        {
            var url = "/transactions/confirmed/" + hash;
            var transactionRootData = await HttpUtilities.GetDataFromApiString(node, url);
            var root = JsonUtility.FromJson<Datum>(transactionRootData);
            return root;
        }
        
        public static async UniTask<Datum> GetUnconfirmedTransaction(string node, string id)
        {
            var url = "/transactions/unconfirmed/" + id;
            var datumString = await HttpUtilities.GetDataFromApiString(node, url);
            return JsonUtility.FromJson<Datum>(datumString);
        }
        
        public static async UniTask<Datum> GetPartialTransaction(string node, string id)
        {
            var url = "/transactions/partial/" + id;
            var datumString = await HttpUtilities.GetDataFromApiString(node, url);
            return JsonUtility.FromJson<Datum>(datumString);
        }


        public static async UniTask<Root> SearchConfirmedTransactions(string node, TransactionSearchCriteria query)
        {
            var param = "?";
            if (query.Address != null) param += "&address=" + query.Address.Plain();
            if (query.RecipientAddress != null) param += "&recipientAddress=" + query.RecipientAddress.Plain();
            if (query.SignerPublicKey != null) param += "&signerPublicKey=" + query.SignerPublicKey;
            if (query.Height != null) param += "&height=" + query.Height;
            if (query.FromHeight != null) param += "&fromHeight=" + query.FromHeight;
            if (query.ToHeight != null) param += "&toHeight=" + query.ToHeight;
            if (query.FromTransferAmount != null) param += "&fromTransferAmount=" + query.FromTransferAmount;
            if (query.ToTransferAmount != null) param += "&toTransferAmount=" + query.ToTransferAmount;
            if (query.Type != null) param += "&type=" + query.Type;
            if (query.Embedded) param += "&embedded=" + query.Embedded;
            if (query.TransferMosaicId != null) param += "&transferMosaicId=" + query.TransferMosaicId.GetIdAsHex();
            if (query.PageSize != 10) param += "&pageSize=" + query.PageSize;
            if (query.PageNumber != 1) param += "&pageNumber=" + query.PageNumber;
            if (query.Offset != null) param += "&offset=" + query.Offset;
            var order = query.Order == Order.Asc ? "asc" : "desc";
            param += "&order=" + order;

            var url = "/transactions/confirmed" + param;
            Debug.Log(url);
            var transactionRootData = await HttpUtilities.GetDataFromApiString(node, url);
            var root = JsonUtility.FromJson<Root>(transactionRootData);
            return root;
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
            public string aggregateHash;
            public string aggregateId;
        }
        
        [Serializable]
        public class InnerTransactionDatum
        {
            public Meta meta;
            public string id;
            public InnerTransaction transaction;
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
            public List<Cosignatures> cosignatures;
            public List<Mosaic> mosaics;
            public int nonce;
            public string id;
            public int flags;
            public byte divisibility;
            public string duration;
            public string mosaicId;
            public int action;
            public string delta;
            public string amount;
            public int hashAlgorithm;
            public string secret;
            public string proof;
            public string targetAddress;
            public string scopedMetadataKey;
            public int valueSizeDelta;
            public int valueSize;
            public string value;
            public string targetMosaicId;
            public int minRemovalDelta;
            public int minApprovalDelta;
            public List<string> addressAdditions;
            public List<string> addressDeletions;
            public List<InnerTransactionDatum> transactions;
        }
        
        [Serializable]
        public class InnerTransaction
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
            public List<Cosignatures> cosignatures;
            public List<Mosaic> mosaics;
            public int nonce;
            public string id;
            public int flags;
            public byte divisibility;
            public string duration;
            public string mosaicId;
            public int action;
            public string delta;
            public string amount;
            public int hashAlgorithm;
            public string secret;
            public string proof;
            public string targetAddress;
            public string scopedMetadataKey;
            public int valueSizeDelta;
            public int valueSize;
            public string value;
            public string targetMosaicId;
            public int minRemovalDelta;
            public int minApprovalDelta;
            public List<string> addressAdditions;
            public List<string> addressDeletions;
        }

        [Serializable]
        public class Cosignatures
        {
            public string version;
            public string signerPublicKey;
            public string signature;
        }
        
        [Serializable]
        public class Mosaic
        {
            public string id;
            public int amount;
        }

        [Serializable]
        public class Datum
        {
            public Meta meta;
            public string id;
            public Transaction transaction;
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
