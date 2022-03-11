using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NativeWebSocket;
using Symnity.Core.Format;
using Symnity.Http.Model;
using Symnity.Model.Accounts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Symnity.UnityScript
{
    public class WebSocketManager : MonoBehaviour
    {
        private WebSocket _websocket;

        public enum WebSocketType
        {
            Confirmed = 0,
            UnConfirmed = 1
        }

        public async void Connect(string node, WebSocketType webSocketType, string address, UnityEvent action, bool isOneTIme = true)
        {
            _websocket = new WebSocket(node.Replace("https://", "wss://") + "/ws");
            _websocket.OnOpen += () => { Debug.Log("WebSocket opened."); };
            _websocket.OnError += errMsg => Debug.Log($"WebSocket Error Message: {errMsg}");
            _websocket.OnClose += code => Debug.Log("WS closed with code: " + code);

            _websocket.OnMessage += async (msg) =>
            {
                var data = Encoding.UTF8.GetString(msg);
                var rootData = JsonUtility.FromJson<RootData>(data);
                string t1;
                string t2;
                switch (webSocketType)
                {
                    case WebSocketType.Confirmed:
                        t1 = "{\"uid\":\"" + rootData.uid + "\", \"subscribe\":\"confirmedAdded/" +
                                   address + "\"}";
                        t2 = "confirmed";
                        break;
                    case WebSocketType.UnConfirmed:
                        t1 = "{\"uid\":\"" + rootData.uid + "\", \"subscribe\":\"unconfirmedAdded/" +
                                   address + "\"}";
                        t2 = "unconfirmed";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(webSocketType), webSocketType, null);
                }
                if (rootData.uid != null)
                {
                    var body = "{\"uid\":\"" + rootData.uid + "\", \"subscribe\":\"block\"}";
                    await _websocket.SendText(body);
                    await _websocket.SendText(t1);
                }
                else
                {
                    var root = JsonUtility.FromJson<Root>(data);
                    if (!root.topic.Contains(t2)) return;
                    action?.Invoke();
                    if (isOneTIme) await _websocket.Close();
                }
            };
            await _websocket.Connect();
        }
        
        private void Update()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _websocket?.DispatchMessageQueue();
#endif
        }

        private async void OnDestroy()
        {
            if (_websocket == null) return;
            if (_websocket.State != WebSocketState.Closed && _websocket.State != WebSocketState.Closing)
                await _websocket.Close();
        }


        private async void OnApplicationQuit()
        {
            if (_websocket == null) return;
            if (_websocket.State != WebSocketState.Closed && _websocket.State != WebSocketState.Closing)
                await _websocket.Close();
        }

        [Serializable]
        public class Mosaic
        {
            public string id;
            public string amount;
        }

        [Serializable]
        public class WsTransaction
        {
            public string signature;
            public string signerPublicKey;
            public int version;
            public int network;
            public int type;
            public string maxFee;
            public string deadline;
            public string recipientAddress;
            public List<Mosaic> mosaics;
        }

        [Serializable]
        public class Meta
        {
            public string hash;
            public string merkleComponentHash;
            public string height;
        }

        [Serializable]
        public class WaTransactionData
        {
            public WsTransaction transaction;
            public Meta meta;
        }

        [Serializable]
        public class Root
        {
            public string topic;
            public WaTransactionData data;
        }

        [Serializable]
        public class RootData
        {
            public string uid;
        }
    }
}