using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NativeWebSocket;
using Symnity.Http;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace Symnity.UnityScript
{
    public class NodeUtilities
    {
        public static async UniTask<string> GetNode()
        {
            while (true)
            {
                var nodes = NodeList.GetNodes();
                var rand = new Random();
                var node = nodes[(int) Math.Floor((float) rand.NextDouble() * nodes.Count)];
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
}