using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Symnity.Http.Model
{
    [Serializable]
    public class ApiMultisig : MonoBehaviour
    {
        public static async UniTask<MultisigRoot> GetMultisigAccountInfomation(string node, string address)
        {
            var url = "/account/" + address + "/multisig";
            var multisigInfoStr = await HttpUtiles.GetDataFromApiString(node, url);
            var multisigInfo = JsonUtility.FromJson<MultisigRoot>(multisigInfoStr);
            return multisigInfo;
        }
        
        [Serializable]
        public class Multisig
        {
            public int version;
            public string accountAddress;
            public int minApproval;
            public int minRemoval;
            public List<object> cosignatoryAddresses;
            public List<string> multisigAddresses;
        }

        [Serializable]
        public class MultisigRoot
        {
            public Multisig multisig;
        }
    }
}