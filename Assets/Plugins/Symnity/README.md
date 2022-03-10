# Symnity
SymnityはUnityでブロックチェーンであるSymbolを利用するためのアセットです。
全てC#でコーディングしているのでUnity以外でも利用は可能です。
将来的に誰でも簡単にSymbolを使うことができるようにPrefabをいくつか作成する予定です。そのためUnity用アセットと呼びます。<br>
※TypescriptやJavaのSDKを参考に作成していますが、まだ完成していません。（2022年1月11日）
ひとまず最低限の機能はありますので利用はできるかと思います。

# Requirement
* UniTask 2.2.5
* NativeWebSocket

# Installation
ファイルをダウンロード＆解凍してUnityのAssetフォルダに配置してください。

また、トランザクションをアナウンスするためにUniTaskを使用していますので以下のリンクからインストールしてください。
<br>
https://github.com/Cysharp/UniTask/releases
<br>使用方法は上記を読んでください。
<br>
同じようにWebSocketでの接続はこちらを利用しています。(WebGL対応のため）<br>
https://github.com/endel/NativeWebSocket
<br>ダウンロード＆解凍しAssetフォルダに配置してください。<br>

# Usage

今の所、UnityScript/SampleTransaction.csにかかれているトランザクションは対応しています。<br>
例）

```c#
var networkType = NetworkType.TEST_NET;
var deadLine = Deadline.Create(EpochAdjustment);
var senderAccount = Account.CreateFromPrivateKey(PrivateKey1, networkType);
var receiverAccount = Account.CreateFromPrivateKey(PrivateKey2, networkType);
var message = PlainMessage.Create("Hello Symbol from NEM!");
var mosaicId = new MosaicId("3A8416DB2D53B6C8");
var mosaic = new Mosaic(mosaicId, 1000000);
var transferTransaction = TransferTransaction.Create(
    deadLine,
    receiverAccount.Address,
    new List<Mosaic> {mosaic},
    message,
    networkType,
    100000
);
var signedTx = senderAccount.Sign(transferTransaction, GenerationHash);
Debug.Log(signedTx.Payload);
Debug.Log(signedTx.Hash);

HttpConection.Announce(Node, signedTx.Payload).Forget();
```
* （メッセージ付）送金トランザクション
* メタデータ割当・更新トランザクション
* マルチシグトランザクション
* リボーカブルモザイク没収トランザクション
* それらのアグリゲートトランザクションコンプリート（ボンデッド未対応）
* シークレットロック＆プルーフトランザクション

SymbolのAPIからデータ取得も以下は可能です。
* アカウントデータ
* トランザクションデータ
* メタデータ
* マルチシグデータ

src/Http/Model<br>
以下にあるクラスを利用します。<br>
（参考）

```c#
var Node = "NODE_URL";
Debug.Log("--アカウントデータ取得--");
var accountData = await ApiAccount.CreateAccountFromApi(Node, "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ");
var mosaicId = new MosaicId("65DBB4CC472A5734");
Debug.Log(accountData.address.Plain());
var mosaic = accountData.mosaics.Where(mosaic=>mosaic.Id.GetId() == mosaicId.GetId());
Debug.Log(mosaic.ToList()[0].Amount);

Debug.Log("--トランザクションデータ取得--");
var transactionData =
    await ApiTransaction.CreateTransferTransactionFromApi(
        Node, "97E74C42E4DB83684011B4D29ADA6A5EDF03A87173D6635A8EA7B97CA6988088");
Debug.Log(transactionData.RecipientAddress.Plain());
Debug.Log(transactionData.Message);
Debug.Log(transactionData.Message.Payload);
Debug.Log(transactionData.MaxFee);

Debug.Log("--メタデータ取得--");
var metadataSearchCriteria = new MetadataSearchCriteria(
    MetadataType.Account,
    "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ",
    "19670280EC3E4E7D",
    "TCOHSBNTWYNFUWP2PLGSGDK6EWE4BC5TFZNQBLI"
);
var metadataData = await ApiMetadata.CreateMetadataFromApi(Node, metadataSearchCriteria);
Debug.Log(metadataData.metadataEntry.value);
Debug.Log(ConvertUtils.HexToChar(metadataData.metadataEntry.value));

Debug.Log("--マルチシグデータ取得--");
var multisigData = await ApiMultisig.CreateAccountFromApi(Node, "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ");
multisigData.multisigAddresses.ForEach(multisigAddress =>
{
    Debug.Log(RawAddress.AddressToString(ConvertUtils.GetBytes(multisigAddress)));
});
```

Symbolに関してはこちらを参考にしてください。<br>
https://docs.symbolplatform.com/ja/getting-started/ <br>
※TypeScriptのSDKを主に参考にしていますので書き方は似ていると思います。

また、ネットワークプロパティなどはこちらをご参考ください。<br>
https://qiita.com/nem_takanobu/items/4f50e5740318d92d7dcb


# Note

現在は自分で使うために作成していますのでバグやエラー処理などは完全ではありません。ご理解いただいた上でご利用ください。
希望するトランザクションなどあればTwitterなどでリクエストしていただいても構いません。可能な限り対応します。

アセット内で暗号化等のために<a href="https://www.bouncycastle.org/">BouncyCastle</a>を使用しています。ただWebGLでビルドしたときにDLLだとうまくいかなかったので、ファイルをそのまま使用しています。そのためかいくつかWarningが出ますがご理解いただける方のみご利用ください。いつかなんとかしたいとは思っています。

# Author

* Toshi
* https://twitter.com/toshiya_ma
