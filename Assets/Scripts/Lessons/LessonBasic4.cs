using System.Collections.Generic;
using Symnity.Http;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using Symnity.UnityScript;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonBasic4 : MonoBehaviour
{
    [SerializeField] private TMP_InputField addressInputField;
    [SerializeField] private TMP_InputField mosaicIdInputField;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private TMP_InputField mosaicAmountInputField;
    [SerializeField] private Button lesson2Button;

    private SssExtension sss;
    private string node;
    
    private async void Start()
    {
        node = await NodeUtilities.GetNode(NetworkType.TEST_NET);
        Debug.Log(node);
        lesson2Button.onClick.AddListener(SendMosaic);
        
#if UNITY_WEBGL && !UNITY_EDITOR
        sss = GetComponent<SssExtension>();
        var activeAddress = sss.GetActiveAddress();
        var activePublicKey = sss.GetActivePublicKey();
        var activeNetworkType = sss.GetActiveNetworkType();
        Debug.Log(activeAddress);
        Debug.Log(activePublicKey);
        Debug.Log(activeNetworkType);
#else
        Debug.Log("Only WebGL works");
#endif
    }
    
    private async void SendMosaic()
    {
        var address = Address.CreateFromRawAddress(addressInputField.text);
        var mosaicId = mosaicIdInputField.text;
        var mosaicList = new List<Mosaic>() {new Mosaic(new MosaicId(mosaicId), long.Parse(mosaicAmountInputField.text))};
        var epocAdjustment = await HttpUtilities.GetEpochAdjustment(node);

        var transferTransaction = TransferTransaction.Create(
            Deadline.Create(epocAdjustment),
            address,
            mosaicList,
            PlainMessage.Create(messageInputField.text),
            NetworkType.TEST_NET
        ).SetMaxFee(100);
        sss.AnnounceTransaction(transferTransaction, node, SssMethodType.Announce);
    }
}