using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NativeWebSocket;
using Symnity.Http;
using Symnity.Infrastructure.SearchCriteria;
using Symnity.Model.Network;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace Symnity.UnityScript
{
    public class NodeUtilities
    {
        public static async UniTask<string> GetNode(NetworkType networkType)
        {
            while (true)
            {
                var node = await GetRandomNode(networkType);
                var str = await GetDataFromApi(node + "/node/health");
                var nodeHealthRoot = JsonUtility.FromJson<NodeHealthRoot>(str);
                if (nodeHealthRoot.status.db == "up" && nodeHealthRoot.status.apiNode == "up")
                {
                    return node;
                }
            }
        }

        private static async UniTask<string> GetDataFromApi(string url)
        {
            try
            {
                var webRequest = UnityWebRequest.Get(url);
                await webRequest.SendWebRequest();
                
                if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception(webRequest.error);
                }
                {
                    return webRequest.downloadHandler.text;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error From GetDataFromApi" + e.Message);
            }
        }

        private static async void ListenerKeepOpening(string wsEndpoint)
        {
            var webSocket = new WebSocket(wsEndpoint);
            webSocket.OnOpen += () => { Debug.Log(wsEndpoint); };
            webSocket.OnClose += code => Debug.Log("WS closed with code: " + code);
            webSocket.OnError += errMsg => Debug.Log($"WebSocket Error Message: {errMsg}");
            await webSocket.Connect();
            await webSocket.Close();
        }
        
        public static async UniTask<string> GetRandomNode(NetworkType networkType)
        {
            var url = networkType switch
            {
                NetworkType.MAIN_NET => "https://symbol.services/nodes/",
                NetworkType.TEST_NET => "https://testnet.symbol.services/nodes/",
                _ => throw new ArgumentOutOfRangeException(nameof(networkType), networkType, null)
            };
            var param = "?&limit=1&ssl=true&order=random";
            var str = await GetDataFromApi(url + param);
            var nodeRoot = JsonUtility.FromJson<NodeRoot>(str.Replace("[", "").Replace("]", ""));
            return nodeRoot.apiStatus.restGatewayUrl;
        }

        /*
        public static async UniTask<List<NodeRoot>> GetNodeList(NetworkType networkType, NodeSearchCriteria searchCriteria)
        {
            var param = "?";
            if (searchCriteria.Limit != 0) param += "&limit=" + searchCriteria.Limit;
            param += "&ssl=" + searchCriteria.Ssl;
            param += searchCriteria.NOrder == NodeSearchCriteria.Order.Natural ? "&order=natural" : "&order=random";
            var url = networkType switch
            {
                NetworkType.MAIN_NET => "https://symbol.services/nodes/",
                NetworkType.TEST_NET => "https://testnet.symbol.services/nodes/",
                _ => throw new ArgumentOutOfRangeException(nameof(networkType), networkType, null)
            };
            var str = await GetDataFromApi(url + param);
            Debug.Log(str);
            return JsonUtility.FromJson<List<NodeRoot>>(str);
        }
        */
    }
    
    
    [Serializable]
    public class NodeHealthStatus
    {
        public string apiNode;
        public string db;
    }

    [Serializable]
    public class NodeHealthRoot
    {
        public NodeHealthStatus status;
    }
    
    [Serializable]
    public class Coordinates
    {
        public double latitude;
        public double longitude;
    }
        
    [Serializable]
    public class HostDetail
    {
        public string host;
        public Coordinates coordinates;
        public string location;
        public string ip;
        public string organization;
        public string @as;
        public string continent;
        public string country;
        public string region;
        public string city;
        public string district;
        public string zip;
    }
    
    [Serializable]
    public class PeerStatus
    {
        public bool isAvailable;
        public long lastStatusCheck;
    }

    [Serializable]
    public class WebSocketNode
    {
        public bool isAvailable;
        public bool wss;
        public string url;
    }

    [Serializable]
    public class Finalization
    {
        public int height;
        public int epoch;
        public int point;
        public string hash;
    }

    [Serializable]
    public class NodeStatus
    {
        public string apiNode;
        public string db;
    }

    [Serializable]
    public class ApiStatus
    {
        public string restGatewayUrl;
        public bool isAvailable;
        public bool isHttpsEnabled;
        public long lastStatusCheck;
        public WebSocketNode webSocket;
        public string nodePublicKey;
        public int chainHeight;
        public Finalization finalization;
        public NodeStatus nodeStatus;
        public string restVersion;
    }

    [Serializable]
    public class NodeRoot
    {
        public string _id;
        public int version;
        public string publicKey;
        public string networkGenerationHashSeed;
        public int roles;
        public int port;
        public int networkIdentifier;
        public string host;
        public string friendlyName;
        public DateTime lastAvailable;
        public HostDetail hostDetail;
        public PeerStatus peerStatus;
        public ApiStatus apiStatus;
        public int __v;
    }
}