using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Symnity.Infrastructure.SearchCriteria;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiAccount : MonoBehaviour
    {
        public static async UniTask<AccountDatum> GetAccountInformation(string node, string accountId, bool log = false)
        {
            var url = "/accounts/" + accountId;
            if(log) Debug.Log($@"<a href=""{node}{url}"">{node}{url}</a>");
            var accountDatumStr = await HttpUtilities.GetDataFromApiString(node, url);
            var accountDatum = JsonUtility.FromJson<AccountDatum>(accountDatumStr);
            return accountDatum;
        }
        
        public static async UniTask<AccountRoot> SearchAccounts(string node, AccountSearchCriteria query)
        {
            var param = "?";
            if (query.PageSize != 20) param += "&pageSize=" + query.PageSize;
            if (query.PageNumber != 1) param += "&pageNumber=" + query.PageNumber;
            if (query.Offset != null) param += "&offset=" + query.Offset;
            if (query.MosaicId != null) param += "&mosaicId=" + query.MosaicId.GetIdAsHex();
            var orderBy = query.OrderBy == AccountOrderBy.Id ? "id" : "balance";
            param += "&orderBy=" + orderBy;
            var order = query.Order == Order.Asc ? "asc" : "desc";
            param += "&order=" + order;
            var url = "/accounts" + param;
            Debug.Log($@"<a href=""{node}{url}"">{node}{url}</a>");
            var accountRootData = await HttpUtilities.GetDataFromApiString(node, url);
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
        public class Voting
        {
            public List<string> keys;
        }

        [Serializable]
        public class SupplementalPublicKeys
        {
            public Linked linked;
            public Node node;
            public Vrf vrf;
            public List<Voting> voting;
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