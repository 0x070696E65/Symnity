using System;
using Cysharp.Threading.Tasks;
using Symnity.Http;
using Symnity.Model.Network;
using UnityEngine;

public class SendMosaic : MonoBehaviour
{
    public string node;
    public NetworkType networkType;
    public string receiverAddress;
    public string signerPrivateKey;
    public string mosaicID;
    public long sendMosaicQuantity;
    public string message;
    public int maxFee;

    [NonSerialized]
    public static string Node;
    [NonSerialized]
    public static NetworkType NetworkType;
    [NonSerialized]
    public static string ReceiverAddress;
    [NonSerialized]
    public static string SignerPrivateKey;
    [NonSerialized]
    public static string MosaicID;
    
    public delegate void OnSendMosaicDelegate();
    public OnSendMosaicDelegate OnSendMosaic;

    private void Awake()
    {
        Node = node;
        NetworkType = networkType;
        ReceiverAddress = receiverAddress;
        SignerPrivateKey = signerPrivateKey;
        MosaicID = mosaicID;
    }
    private async void OnCollisionEnter(Collision collision)
    {
        var signedTransferTransaction = await TransactionManager.CreateSignedTransferTransaction(node, networkType, receiverAddress, signerPrivateKey,
            mosaicID, sendMosaicQuantity, message, maxFee);
        HttpUtiles.Announce(node, signedTransferTransaction.Payload).Forget();
        OnSendMosaic();
    }
}
