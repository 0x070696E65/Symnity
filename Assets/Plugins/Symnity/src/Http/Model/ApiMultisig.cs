using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiMultisig : MonoBehaviour
    {
        public static async UniTask<MultisigInfo> GetMultisigAccountInfomation(string node, string address)
        {
            var url = "/account/" + address + "/multisig";
            var multisigInfoStr = await HttpUtiles.GetDataFromApiString(node, url);
            var multisigInfo = JsonUtility.FromJson<MultisigInfo>(multisigInfoStr);
            return multisigInfo;
        }

        [Serializable]
        public class MultisigInfo
        {
            public int version;
            public string accountAddress;
            public int minApproval;
            public int minRemoval;
            public List<string> cosignatoryAddresses;
            public List<string> multisigAddresses;
        }
    }
}