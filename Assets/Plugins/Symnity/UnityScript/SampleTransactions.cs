using System;
using System.Collections.Generic;
using System.Text;
using Symnity.Core.Format;
using Symnity.Model.Accounts;
using Symnity.Model.BlockChain;
using Symnity.Model.Messages;
using Symnity.Model.Mosaics;
using Symnity.Model.Network;
using Symnity.Model.Transactions;
using Cysharp.Threading.Tasks;
using Symnity.Core.Crypto;
using Symnity.Model.Lock;
using UnityEngine;
using UnityEngine.Networking;

namespace Symnity.UnityScript
{
    [Serializable]
    public class SampleTransactions : MonoBehaviour
    {
        private static NetworkType _networkType = NetworkType.TEST_NET;

        private const string Node = "";
        private const int EpochAdjustment = 1637848847;
        private const string GenerationHash = "7FCCD304802016BEBBCD342A332F91FF1F3BB5E902988B352697BE245F48E836";
        private const string AdminAccountPrivateKey = "";
        private static Account _adminAccount = Account.CreateFromPrivateKey(AdminAccountPrivateKey, _networkType);
        private const string PlayerAccountPrivateKey = "";
        private static Account _playerAccount = Account.CreateFromPrivateKey(PlayerAccountPrivateKey, _networkType);
        private const string CharacterAccountPrivateKey = "";
        private static Account _characterAccount = Account.CreateFromPrivateKey(CharacterAccountPrivateKey, _networkType);
        
        public class Order
        {
            private string Id;
            private int Amount;
            public Order(string buyMosaicId, int buyMosaicAmount)
            {
                Id = buyMosaicId;
                Amount = buyMosaicAmount;
            }
        }
        
        // Revocableモザイク発行トランザクション
        public static SignedTransaction DefineRevocableMosaicTransaction()
        {
            //revocable モザイク発行
            const bool isSupplyMutable = true;
            const bool isTransferable = true;
            const bool isRestrictable = true;
            const bool isRevocable = true;

            var deadLine = Deadline.Create(EpochAdjustment);
            var nonce = MosaicNonce.CreateRandom(4);

            var mosaicDefTx = MosaicDefinitionTransaction.Create(
                deadLine,
                nonce,
                MosaicId.CreateFromNonce(nonce, _adminAccount.Address),
                MosaicFlags.Create(isSupplyMutable, isTransferable, isRestrictable, isRevocable),
                0,
                new BlockDuration(0),
                _networkType
            );

            //モザイク変更
            var mosaicChangeTx = MosaicSupplyChangeTransaction.Create(
                deadLine,
                mosaicDefTx.MosaicId,
                MosaicSupplyChangeAction.Increase,
                1000000,
                _networkType
            );

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                deadLine,
                new List<Transaction>
                {
                    mosaicDefTx.ToAggregate(_adminAccount.GetPublicAccount()),
                    mosaicChangeTx.ToAggregate(_adminAccount.GetPublicAccount())
                },
                _networkType,
                new List<AggregateTransactionCosignature>())
                .SetMaxFeeForAggregate(100, 0);

            return _adminAccount.Sign(
                aggregateTransaction,
                GenerationHash
            );
        }

        // ゲーム開始時にプレイヤーアカウントにキャラクターアカウント付与（メタデータ初期）
        public static SignedTransaction MultisigAndMetadataAggregateTransaction()
        {
            var deadLine = Deadline.Create(EpochAdjustment);
            var keyLife = KeyGenerator.GenerateUInt64Key("Life");
            var keyAttack = KeyGenerator.GenerateUInt64Key("Attack");

            const string metaDataLife = "100";
            const string metaDataPower = "10";

            var playerChildAccountModificationTransaction = MultisigAccountModificationTransaction.Create(
                deadLine,
                1,
                1,
                new List<UnresolvedAddress>
                {
                    _playerAccount.Address
                },
                new List<UnresolvedAddress>(),
                _networkType
            );

            var playerChildAccountMetadataLifeTransactionL = AccountMetadataTransaction.Create(
                deadLine,
                _characterAccount.Address,
                keyLife,
                (short) metaDataLife.Length,
                metaDataLife,
                _networkType
            );

            var playerChildAccountMetadataPowerTransactionL = AccountMetadataTransaction.Create(
                deadLine,
                _characterAccount.Address,
                keyAttack,
                (short) metaDataPower.Length,
                metaDataPower,
                _networkType
            );


            var aggregateTransaction = AggregateTransaction.CreateComplete(
                deadLine,
                new List<Transaction>
                {
                    playerChildAccountModificationTransaction.ToAggregate(_characterAccount.GetPublicAccount()),
                    playerChildAccountMetadataLifeTransactionL.ToAggregate(_adminAccount.GetPublicAccount()),
                    playerChildAccountMetadataPowerTransactionL.ToAggregate(_adminAccount.GetPublicAccount()),
                },
                _networkType,
                new List<AggregateTransactionCosignature>()
                ).SetMaxFeeForAggregate(100, 2);

            var signedTransactionComplete = _adminAccount.SignTransactionWithCosignatories(
                aggregateTransaction,
                new List<Account>{_playerAccount, _characterAccount},
                GenerationHash
            );
            
            return signedTransactionComplete;
        }

        // Revocableモザイク没収トランザクション
        public static SignedTransaction MosaicRevocationTransaction(int amount)
        {
            var deadLine = Deadline.Create(EpochAdjustment);
            var revTx = MosaicSupplyRevocationTransaction.Create(
                deadLine,
                _playerAccount.Address,
                new Mosaic(new MosaicId("65DBB4CC472A5734"), amount),
                _networkType
            ).SetMaxFee(100);

            return _adminAccount.Sign(revTx, GenerationHash);
        }
        
        [Serializable]
        public class ClearFloor
        {
            public string characterAddress;
            public int floor;
            public ClearFloor(string characterAddress, int floor)
            {
                this.characterAddress = characterAddress;
                this.floor = floor;
            }
        }
        
        public static SignedTransaction AddCoinTransaction(long pointsToAdd, string message = "")
        {
            const string coinMosaicId = "";
            var deadLine = Deadline.Create(EpochAdjustment);
            var transferTx = TransferTransaction.Create(
                deadLine,
                _characterAccount.Address,
                new List<Mosaic> {new Mosaic(new MosaicId(coinMosaicId), pointsToAdd)},
                new Message(MessageType.PlainMessage, message),
                _networkType
            ).SetMaxFee(100);
            return _adminAccount.Sign(transferTx, GenerationHash);
        }

        public static async UniTask<string> Announce(string payload)
        {
            try
            {
                const string url = Node + "/transactions";
                var myData = Encoding.UTF8.GetBytes("{ \"payload\" : \"" + payload + "\"}");
                var webRequest = UnityWebRequest.Put(url, myData);
                webRequest.SetRequestHeader("Content-Type", "application/json");
                await webRequest.SendWebRequest();
                if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    //エラー確認
                    throw new Exception(webRequest.error);
                }

                webRequest.Dispose();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }

            return "Upload complete!";
        }
        
        // シンプルなメッセージ送信トランザクション
        public static SignedTransaction SimpleTransferTransaction()
        {
            var deadLine = Deadline.Create(EpochAdjustment);
            var message = PlainMessage.Create("Hello Symbol from NEM!");
            var mosaicId = new MosaicId("3A8416DB2D53B6C8");
            var mosaic = new Mosaic(mosaicId, 1000000);
            var transferTransaction = TransferTransaction.Create(
                deadLine,
                _characterAccount.Address,
                new List<Mosaic> {mosaic},
                message,
                _networkType
            ).SetMaxFee(100);
            var signedTx = _adminAccount.Sign(transferTransaction, GenerationHash);
            return signedTx;
        }
    }
    
    public class Order
    {
        public string OrderMosaicId;
        public string OrderMosaicName;
        public int OrderAmount;
        public string SupplyMosaicId;
        public string SupplyMosaicName;
        public int SupplyAmount;
        public string SignerPublicKey;
        public string Hash;
        public int Duration;
        public string Secret;
            
        public Order(string orderMosaicId, string orderMosaicName, int orderAmount, string supplyMosaicId,
            string supplyMosaicName, int supplyAmount, int duration,
            string signerPublicKey = null, string hash = null, string secret = null)
        {
            OrderMosaicId = orderMosaicId;
            OrderMosaicName = orderMosaicName;
            OrderAmount = orderAmount;
            SupplyMosaicId = supplyMosaicId;
            SupplyMosaicName = supplyMosaicName;
            SupplyAmount = supplyAmount;
            Duration = duration;
            SignerPublicKey = signerPublicKey;
            Hash = hash;
            Secret = secret;
        }
    }
    
    public class OrderMosaic
    {
        public string Id;
        public int Amount;

        public OrderMosaic(string buyMosaicId, int buyMosaicAmount)
        {
            Id = buyMosaicId;
            Amount = buyMosaicAmount;
        }
    }
}