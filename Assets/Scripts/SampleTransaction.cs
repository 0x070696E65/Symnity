using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using NativeWebSocket;
using Symnity.Core.Format;
using Symnity.Http;
using Symnity.Http.Model;
using Symnity.Model.Accounts;
using Symnity.Model.Network;
using Symnity.UnityScript;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SampleTransaction : MonoBehaviour
{
    public string node = "https://test.hideyoshi-node.net:3001";
    public NetworkType networkType;
    public string receiverAddress;
    public string signerPrivateKey;
    public string mosaicID = "3A8416DB2D53B6C8";
    public long sendMosaicQuantity;
    public string message;
    public int maxFee;

    private string senderAccountAddress;
    public TextMeshProUGUI senderAccountAddressText;
    public TextMeshProUGUI senderAccountMosaicIDText;
    public TextMeshProUGUI senderAccountQuantityText;
    public TextMeshProUGUI receiverAccountAddressText;
    public TextMeshProUGUI receiverAccountMosaicIDText;
    public TextMeshProUGUI receiverAccountQuantityText;
    
    public UnityEvent sentAction;
    public UnityEvent receivedAction;

    public WebSocketManager webSocketManager;

    
    private void Start()
    {
        senderAccountAddress = Account.CreateFromPrivateKey(signerPrivateKey, networkType).Address.Plain();
        senderAccountAddressText.text = senderAccountAddress;
        senderAccountMosaicIDText.text = mosaicID;
        
        receivedAction.AddListener(GetMosaicQuantity);

        receiverAccountAddressText.text = receiverAddress;
        receiverAccountMosaicIDText.text = mosaicID;
        GetMosaicQuantity();
    }
    
    private async void OnCollisionEnter(Collision collision)
    {
        webSocketManager.Connect(node, WebSocketManager.WebSocketType.Confirmed, receiverAddress, receivedAction);
        var signedTransferTransaction = await TransactionManager.CreateSignedTransferTransaction(node, networkType, receiverAddress, signerPrivateKey,
            mosaicID, sendMosaicQuantity, message, maxFee);
        HttpUtiles.Announce(node, signedTransferTransaction.Payload).Forget();
        sentAction.Invoke();
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
        
        var receiverAccountInformation = await ApiAccount.GetAccountInformation(node, receiverAddress);
        receiverAccountInformation.account.mosaics.ForEach(mosaic =>
        {
            if (mosaic.id == mosaicID)
            {
                receiverAccountQuantityText.text = mosaic.amount;
            }
        });
    }
}
