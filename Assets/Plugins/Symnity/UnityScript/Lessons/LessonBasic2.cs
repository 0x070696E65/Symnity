using System.Collections.Generic;
using System.Linq;
using Plugins.Symnity.src.Repositories;
using Symnity.Http;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LessonBasic2 : MonoBehaviour
{
    [SerializeField] private Button lesson2Button;
    [SerializeField] private TMP_InputField privateKeyInputField;
    [SerializeField] private TMP_InputField addressInputField;
    [SerializeField] private TMP_InputField mosaicIdInputField;
    [SerializeField] private TMP_InputField mosaicAmountInputField;

    private TransactionRepository transactionRepository;

    private void Start()
    {
        transactionRepository = new TransactionRepository("https://hideyoshi.mydns.jp:3001");
        lesson2Button.onClick.AddListener(SendMosaic);
    }
    
    private async void SendMosaic()
    {
        var signerAccount = Account.CreateFromPrivateKey(privateKeyInputField.text, NetworkType.TEST_NET);
        var address = Address.CreateFromRawAddress(addressInputField.text);
        var mosaicId = mosaicIdInputField.text;
        var mosaicList = new List<Mosaic>() {new Mosaic(new MosaicId(mosaicId), long.Parse(mosaicAmountInputField.text))};

        var transferTransaction = TransferTransaction.Create(
            Deadline.Create(1637848847),
            address,
            mosaicList,
            PlainMessage.Create("hello symnity!!"),
            NetworkType.TEST_NET
        ).SetMaxFee(100);

        var signedTransaction = signerAccount.Sign(transferTransaction,
            "7FCCD304802016BEBBCD342A332F91FF1F3BB5E902988B352697BE245F48E836");
        var result = await transactionRepository.Announce(signedTransaction);
        Debug.Log(result);
    }
}