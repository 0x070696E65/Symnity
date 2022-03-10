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
using Newtonsoft.Json.Linq;
using Symnity.Core.Crypto;
using Symnity.Http;
using Symnity.Model.Lock;
using UnityEngine;
using UnityEngine.Networking;

namespace Symnity.UnityScript
{
    // 交換所にて注文を依頼する時のトランザクション（署名前）
    // Transaction when requesting an order at the exchange (before signature)
    // ->> CreateExchangeBuyTransaction <<-
    
    // 交換所にて注文を依頼する時のトランザクション（署名後）
    // Transaction when requesting an order at the exchange (after signing)
    // ->> CreateExchangeBuySignedTransaction <<-
    
    // 交換所にて依頼を受ける時のトランザクション
    // Transactions when receiving requests at the exchange
    // ->> CreateExchangeSellTransaction <<-
    
    // ゲーム開始時にプレイヤーアカウントにキャラクターアカウント付与（メタデータ初期）
    // Character account assigned to the player account at the start of the game (initial metadata)
    // ->> MultisigAndMetadataAggregateTransaction <<-
    
    // Revocableモザイク没収トランザクション
    // Revocable Mosaic Confiscation Transaction
    // ->> MosaicRevocationTransaction <<-
    
    // ダンジョン脱出時のトランザクション
    // Transactions during dungeon escape
    // ->> FloorEscapeTransaction <<-
    
    // コイン送信トランザクション（ただのTransferTransaction）
    // Coin Transfer Transaction (just TransferTransaction)
    // ->> AddCoinTransaction <<-
    
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
        
        
        private static SignedTransaction CreateExchangeBuySignedTransaction(AggregateTransaction aggTx,
            Account buyAccount)
        {
            var signedTransactionComplete = buyAccount.SignTransactionWithCosignatories(
                aggTx,
                new List<Account>{_adminAccount},
                GenerationHash
            );
            return signedTransactionComplete;
        }

        public class ExchangeSellTransaction
        {
            private long Fee;
            private SignedTransaction SignedTransaction;

            public ExchangeSellTransaction(long fee, SignedTransaction signedTransaction)
            {
                Fee = fee;
                SignedTransaction = signedTransaction;
            }
        }

        // 疑似取引所購入意思表示トランザクション
        public static ExchangeSellTransaction CreateExchangeSellTransaction(string sellPrivateKey,
            string exchangePublicKey, string buyMosaicId, int buyAmount, string sellMosaicId, int sellAmount,
            int durationBlock)
        {
            var deadLine = Deadline.Create(EpochAdjustment);
            var sellAccount = Account.CreateFromPrivateKey(sellPrivateKey, _networkType);
            var exchangePublicAccount = PublicAccount.CreateFromPublicKey(exchangePublicKey, _networkType);

            var random = Crypto.RandomBytes(20);
            var proof = ConvertUtils.ToHex(random);
            var hasher = SHA3Hasher.CreateHasher(32);
            var array = new byte[32];
            hasher.Hasher.BlockUpdate(random, 0, random.Length);
            hasher.Hasher.DoFinal(array, 0);
            var secret = ConvertUtils.ToHex(array).ToUpper();

            var order = new OrderMosaic(buyMosaicId, buyAmount);
            var json = JsonUtility.ToJson(order);

            var coinLockTx = SecretLockTransaction.Create(
                deadLine,
                new Mosaic(new MosaicId(sellMosaicId), sellAmount),
                new BlockDuration(durationBlock),
                LockHashAlgorithm.Op_Sha3_256,
                secret,
                exchangePublicAccount.Address,
                _networkType
            );

            var proofTx = TransferTransaction.Create(
                deadLine,
                exchangePublicAccount.Address,
                new List<Mosaic>(),
                sellAccount.EncryptMessage(proof, exchangePublicAccount),
                _networkType
            );
            var infoTx = TransferTransaction.Create(
                deadLine,
                exchangePublicAccount.Address,
                new List<Mosaic>(),
                PlainMessage.Create(json),
                _networkType
            );

            var aggTx = AggregateTransaction.CreateComplete(
                deadLine,
                new List<Transaction>
                {
                    coinLockTx.ToAggregate(sellAccount.GetPublicAccount()),
                    proofTx.ToAggregate(sellAccount.GetPublicAccount()),
                    infoTx.ToAggregate(sellAccount.GetPublicAccount())
                },
                _networkType,
                new List<AggregateTransactionCosignature>()
            ).SetMaxFeeForAggregate(100, 0);

            var aggTxSigned = sellAccount.Sign(
                aggTx,
                GenerationHash
            );
            Debug.Log(aggTxSigned.Hash);
            return new ExchangeSellTransaction(aggTx.MaxFee, aggTxSigned);
        }

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
        
        public static SignedTransaction FloorEscapeTransaction(int floorCount)
        {
            Debug.Log("Floor Escape: " + floorCount);
            var EscapeMosaicId = "";
            var message = JObject.FromObject(new ClearFloor(_characterAccount.Address.Plain(), floorCount)).ToString();
            var deadLine = Deadline.Create(EpochAdjustment);
            var transferTx = TransferTransaction.Create(
                deadLine,
                _playerAccount.Address,
                new List<Mosaic> {new Mosaic(new MosaicId(EscapeMosaicId), 1)},
                new Message(MessageType.PlainMessage, message),
                _networkType
            );
            
            var revokeTx = MosaicSupplyRevocationTransaction.Create(
                deadLine,
                _playerAccount.Address,
                new Mosaic(new MosaicId(EscapeMosaicId), 1),
                _networkType
            );

            var aggTx = AggregateTransaction.CreateComplete(
                deadLine,
                new List<Transaction>
                {
                    transferTx.ToAggregate(_adminAccount.GetPublicAccount()),
                    revokeTx.ToAggregate(_adminAccount.GetPublicAccount())
                },
                _networkType,
                new List<AggregateTransactionCosignature>()
            ).SetMaxFeeForAggregate(100, 0);
            return _adminAccount.Sign(aggTx, GenerationHash);
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

        // アカウントデータ取得
        /*var accountData = ApiAccount.CreateAccountFromApi("TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ");
        var mosaicId = new MosaicId("65DBB4CC472A5734");
        Console.WriteLine(accountData.address.Plain());
        var mosaic = accountData.mosaics.Where(mosaic=>mosaic.Id.GetId() == mosaicId.GetId());
        Console.WriteLine(mosaic.ToList()[0].Amount);*/

        // トランザクションデータ取得
        /*var transactionData = ApiTransaction.CreateTransferTransactionFromApi("97E74C42E4DB83684011B4D29ADA6A5EDF03A87173D6635A8EA7B97CA6988088");
        Console.WriteLine(transactionData.RecipientAddress.Plain());
        Console.WriteLine(transactionData.Message.Payload);
        Console.WriteLine(transactionData.MaxFee);*/

        // メタデータ取得
        /*var metadataSearchCriteria = new ApiMetadata.MetadataSearchCriteria(
            MetadataType.Account,
            "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ",
            "19670280EC3E4E7D",
            "TCOHSBNTWYNFUWP2PLGSGDK6EWE4BC5TFZNQBLI"
        );
        var metadataData = ApiMetadata.CreateMetadataFromApi(metadataSearchCriteria);
        Console.WriteLine(ConvertUtils.HexToChar(metadataData.metadataEntry.value));*/

        // マルチシグデータ取得
        /*var multisigData = ApiMultisig.CreateAccountFromApi("TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ");
        multisigData.multisigAddresses.ForEach(multisigAddress=>
        {
            Console.WriteLine(RawAddress.AddressToString(ConvertUtils.GetBytes(multisigAddress)));
        });*/
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