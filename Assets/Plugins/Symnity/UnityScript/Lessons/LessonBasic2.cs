using System.Collections.Generic;
using Symnity.Infrastructure;
using Symnity.Infrastructure.SearchCriteria;
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

    private async void Start()
    {
        var bbb = new TransactionSearchCriteria(
            group: TransactionGroup.Confirmed,
            order:Order.Asc,
            pageSize:1,
            pageNumber:2,
            address: Address.CreateFromPublicKey("A890D229FEBDADEDD5B7D1DBDF2B4BECD21CCDCD15C420FC986CE8BBC2C972E4", NetworkType.TEST_NET)
            );
        transactionRepository = new TransactionRepository("https://hideyoshi.mydns.jp:3001");
        lesson2Button.onClick.AddListener(SendMosaic);
        var page = await transactionRepository.Search(bbb);
        Debug.Log(page.Data[0].TransactionInfo?.Hash);
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
            MessageFactory.EmptyMessage(),
            NetworkType.TEST_NET
        ).SetMaxFee(100);

        var signedTransaction = signerAccount.Sign(transferTransaction,
            "7FCCD304802016BEBBCD342A332F91FF1F3BB5E902988B352697BE245F48E836");
        var result = await transactionRepository.Announce(signedTransaction);
        Debug.Log(result);
    }
}