using System.Collections.Generic;
using Symnity.Model.Accounts;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace Symnity.UnityScript
{
    public class SssController : MonoBehaviour
    {
        [SerializeField] private Button excuseButton;
        [SerializeField] private Button excuseButton2;
        
        private void Start()
        {
            excuseButton.onClick.AddListener(Excuse);
            excuseButton2.onClick.AddListener(Excuse2);
        }

        private void Excuse()
        {
            var sss = GetComponent<SssExtension>();
            var activeAddress = sss.GetActiveAddress();
            var activePublicKey = sss.GetActivePublicKey();
            var activeNetworkType = sss.GetActiveNetworkType();
            Debug.Log(activeAddress);
            Debug.Log(activePublicKey);
            Debug.Log(activeNetworkType);
            
            var publicAccountAlice = PublicAccount.CreateFromPublicKey("A890D229FEBDADEDD5B7D1DBDF2B4BECD21CCDCD15C420FC986CE8BBC2C972E4", NetworkType.TEST_NET);
            var publicAccountBob = PublicAccount.CreateFromPublicKey("B055C6F655CD3101A04567F9499F24BE7AB970C879887BD3C6644AB7CAA22D22", NetworkType.TEST_NET);
            var publicAccountClare = PublicAccount.CreateFromPublicKey("43CC385CF37318D022336624C8A56CBEB60360712D70163B554BA23EABF2D10E", NetworkType.TEST_NET);
            var deadLine = Deadline.Create(1637848847);
            var tx1 = TransferTransaction.Create(
                deadLine,
                publicAccountBob.Address,
                new List<Mosaic>() { },
                PlainMessage.Create("TEST"), 
                NetworkType.TEST_NET
            );
            var tx2 = TransferTransaction.Create(
                deadLine,
                publicAccountClare.Address,
                new List<Mosaic>() { },
                PlainMessage.Create("TEST"),
                NetworkType.TEST_NET
            );
            var aggTx = AggregateTransaction.CreateComplete(
                deadLine,
                new List<Transaction>()
                {
                    tx1.ToAggregate(publicAccountAlice),
                    tx2.ToAggregate(publicAccountAlice)
                },
                NetworkType.TEST_NET,
                new List<AggregateTransactionCosignature>() { }
            ).SetMaxFeeForAggregate(100, 0);
            sss.AnnounceTransaction(aggTx, "https://hideyoshi.mydns.jp:3001", SssMethodType.Announce);
        }
        
        private void Excuse2()
        {
            var sss = GetComponent<SssExtension>();
            var activeAddress = sss.GetActiveAddress();
            var activePublicKey = sss.GetActivePublicKey();
            var activeNetworkType = sss.GetActiveNetworkType();
            Debug.Log(activeAddress);
            Debug.Log(activePublicKey);
            Debug.Log(activeNetworkType);
            
            var publicAccountBob = PublicAccount.CreateFromPublicKey("B055C6F655CD3101A04567F9499F24BE7AB970C879887BD3C6644AB7CAA22D22", NetworkType.TEST_NET);
            var deadLine = Deadline.Create(1637848847);
            var tx1 = TransferTransaction.Create(
                deadLine,
                publicAccountBob.Address,
                new List<Mosaic>() { },
                PlainMessage.Create("TEST"),
                NetworkType.TEST_NET
            ).SetMaxFee(100);
            sss.AnnounceTransaction(tx1, "https://hideyoshi.mydns.jp:3001", SssMethodType.Announce);
        }
    }
}