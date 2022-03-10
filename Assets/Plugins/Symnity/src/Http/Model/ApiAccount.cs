using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Newtonsoft.Json.Linq;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.Mosaics;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiAccount : MonoBehaviour
    {
        public static async UniTask<AccountDatum> GetAccountInformation(string node, string accountId)
        {
            var url = "/accounts/" + accountId;
            var accountDatumStr = await HttpUtiles.GetDataFromApiString(node, url);
            var accountDatum = JsonUtility.FromJson<AccountDatum>(accountDatumStr);
            return accountDatum;
        }
        
        public class AccountQueryParameters
        {
            public readonly int pageSize;
            public readonly int pageNumber;
            public readonly string offset;
            public readonly string order;
            public readonly string orderBy;
            public readonly string mosaicId;

            public AccountQueryParameters(
                int pageSize = 10,
                int pageNumber = 1,
                string offset = null,
                string order = null,
                string orderBy = null,
                string mosaicId = null
            )
            {
                this.pageSize = pageSize;
                this.pageNumber = pageNumber;
                this.offset = offset;
                this.order = order;
                this.orderBy = orderBy;
                this.mosaicId = mosaicId;
            }
        }

        public static async UniTask<AccountRoot> SearchAccounts(string node, AccountQueryParameters query)
        {
            var param = "?";
            if (query.pageSize != 10) param += "&pageSize=" + query.pageSize;
            if (query.pageNumber != 1) param += "&pageNumber=" + query.pageNumber;
            if (query.offset != null) param += "&offset=" + query.offset;
            if (query.order != null) param += "&order=" + query.order;
            if (query.orderBy != null) param += "&orderBy=" + query.orderBy;
            if (query.mosaicId != null) param += "&mosaicId=" + query.mosaicId;

            var url = "/accounts" + param;
            var accountRootData = await HttpUtiles.GetDataFromApiString(node, url);
            var root = JsonUtility.FromJson<AccountRoot>(accountRootData);
            return root;
        }

        [Serializable]
        public class Linked
        {
            public string publicKey;
        }

        [Serializable]
        public class Node
        {
            public string publicKey;
        }

        [Serializable]
        public class Vrf
        {
            public string publicKey;
        }

        [Serializable]
        public class SupplementalPublicKeys
        {
            public Linked linked;
            public Node node;
            public Vrf vrf;
        }

        [Serializable]
        public class ActivityBucket
        {
            public string startHeight;
            public string totalFeesPaid;
            public int beneficiaryCount;
            public string rawScore;
        }

        [Serializable]
        public class Mosaic
        {
            public string id;
            public string amount;
        }

        [Serializable]
        public class Account
        {
            public int version;
            public string address;
            public string addressHeight;
            public string publicKey;
            public string publicKeyHeight;
            public int accountType;
            public SupplementalPublicKeys supplementalPublicKeys;
            public List<ActivityBucket> activityBuckets;
            public List<Mosaic> mosaics;
            public string importance;
            public string importanceHeight;
        }

        [Serializable]
        public class AccountDatum
        {
            public Account account;
            public string id;
        }

        [Serializable]
        public class AccountPagination
        {
            public int pageNumber;
            public int pageSize;
        }

        [Serializable]
        public class AccountRoot
        {
            public List<AccountDatum> data;
            public AccountPagination pagination;
        }
    }
}