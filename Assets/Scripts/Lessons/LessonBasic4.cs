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
    [SerializeField] private Button lesson2Button;
    [SerializeField] private TMP_InputField addressInputField;
    [SerializeField] private TMP_InputField mosaicIdInputField;
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private TMP_InputField mosaicAmountInputField;
    private SssExtension sss;
    
    private void Start()
    {
        sss = GetComponent<SssExtension>();
        lesson2Button.onClick.AddListener(SendMosaic);
    }
    
    private async void SendMosaic()
    {
        var node = await NodeUtilities.GetNode(NetworkType.TEST_NET);
        Debug.Log(node);
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