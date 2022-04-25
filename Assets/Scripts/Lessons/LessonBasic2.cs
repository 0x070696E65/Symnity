using System.Collections.Generic;
using Symnity.Http;
using Symnity.Infrastructure;
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
    [SerializeField] private TMP_InputField messageInputField;
    [SerializeField] private TMP_InputField mosaicAmountInputField;
    [SerializeField] private string node;
    private TransactionRepository transactionRepository;
    
    private void Start()
    {
        transactionRepository = new TransactionRepository(node);
        lesson2Button.onClick.AddListener(SendMosaic);
    }
    
    private async void SendMosaic()
    {
        var signerAccount = Account.CreateFromPrivateKey(privateKeyInputField.text, NetworkType.TEST_NET);
        var address = Address.CreateFromRawAddress(addressInputField.text);
        var mosaicId = mosaicIdInputField.text;
        var mosaicList = new List<Mosaic>() {new Mosaic(new MosaicId(mosaicId), long.Parse(mosaicAmountInputField.text))};
        
        var epocAdjustment = await HttpUtilities.GetEpochAdjustment(node);
        var generationHash = await HttpUtilities.GetGenerationHash(node);

        var transferTransaction = TransferTransaction.Create(
            Deadline.Create(epocAdjustment),
            address,
            mosaicList,
            PlainMessage.Create(messageInputField.text),
            NetworkType.TEST_NET
        ).SetMaxFee(100);
        
        var signedTransaction = signerAccount.Sign(transferTransaction, generationHash);
        Debug.Log($@"<a href=""https://testnet.symbol.fyi/transactions/{signedTransaction.Hash}"">https://testnet.symbol.fyi/transactions/{signedTransaction.Hash}</a>");
        var result = await transactionRepository.Announce(signedTransaction);
        Debug.Log(result);
    }
}