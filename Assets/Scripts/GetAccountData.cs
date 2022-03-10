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
using UnityEngine.SceneManagement;

public class GetAccountData : MonoBehaviour
{
    private string node = "https://test.hideyoshi-node.net:3001";
    private string mosaicID;
    private string senderAccountAddress;
    private string receiverAccountAddress;
    public TextMeshProUGUI senderAccountAddressText;
    public TextMeshProUGUI senderAccountMosaicIDText;
    public TextMeshProUGUI senderAccountQuantityText;
    public TextMeshProUGUI receiverAccountAddressText;
    public TextMeshProUGUI receiverAccountMosaicIDText;
    public TextMeshProUGUI receiverAccountQuantityText;
    public TextMeshProUGUI alertText;

    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;
    
    private static WebSocket _websocket;

    private async void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        var sendMosaic = GetComponent<SendMosaic>();
        sendMosaic.OnSendMosaic += SentMosaic;
        
        node = SendMosaic.Node;
        mosaicID = SendMosaic.MosaicID;
        senderAccountAddress = Account.CreateFromPrivateKey(SendMosaic.SignerPrivateKey, SendMosaic.NetworkType).Address
            .Plain();
        receiverAccountAddress = SendMosaic.ReceiverAddress;
        senderAccountAddressText.text = senderAccountAddress;
        senderAccountMosaicIDText.text = mosaicID;
        
        receiverAccountAddressText.text = receiverAccountAddress;
        receiverAccountMosaicIDText.text = mosaicID;
        GetMosaicQuantity();
        
        _websocket = new WebSocket(node.Replace("https://", "wss://")+"/ws");
        _websocket.OnOpen += () => { Debug.Log("WebSocket opened."); };
        _websocket.OnError += errMsg => Debug.Log($"WebSocket Error Message: {errMsg}");
        _websocket.OnClose += code => Debug.Log("WS closed with code: " + code);

        _websocket.OnMessage += async (msg) =>
        {
            var data = Encoding.UTF8.GetString(msg);
            var rootData = JsonUtility.FromJson<RootData>(data);
            if (rootData.uid != null)
            {
                var body = "{\"uid\":\"" + rootData.uid + "\", \"subscribe\":\"block\"}";
                await _websocket.SendText(body);
                var confirmed = "{\"uid\":\"" + rootData.uid + "\", \"subscribe\":\"confirmedAdded/" +
                                receiverAccountAddress + "\"}";
                await _websocket.SendText(confirmed);
            }
            else
            {
                var root = JsonUtility.FromJson<Root>(data);
                if (!root.topic.Contains("confirmed")) return;
                GetMosaicQuantity();
                audioSource.PlayOneShot(sound2);
                alertText.text = "Receive Mosaic !!!!";
            }
        };
        await _websocket.Connect();
    }

    private void SentMosaic()
    {
        audioSource.PlayOneShot(sound1);
        alertText.text = "Send Mosaic !!!!";
    }

    private async void GetMosaicQuantity()
    {
        var senderAccountInformation = await ApiAccount.GetAccountInformation(node, senderAccountAddress);
        senderAccountInformation.account.mosaics.ForEach(mosaic =>
        {
            if (mosaic.id == mosaicID)
            {
                senderAccountQuantityText.text = mosaic.amount;
            }
        });
        
        var receiverAccountInformation = await ApiAccount.GetAccountInformation(node, receiverAccountAddress);
        receiverAccountInformation.account.mosaics.ForEach(mosaic =>
        {
            if (mosaic.id == mosaicID)
            {
                receiverAccountQuantityText.text = mosaic.amount;
            }
        });
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
        if(_websocket.State != WebSocketState.Closed && _websocket.State != WebSocketState.Closing) await _websocket.Close();
    }


    private async void OnApplicationQuit()
    {
        if (_websocket == null) return;
        if(_websocket.State != WebSocketState.Closed && _websocket.State != WebSocketState.Closing) await _websocket.Close();
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
