# Symnity
SymnityはUnityでブロックチェーンであるSymbolを利用するためのアセットです。

※TypescriptやJavaのSDKを参考に作成していますが、まだ完成していません。（2022年1月11日）
ひとまず最低限の機能はありますので利用はできるかと思います。

# ノーコードでSymbolを使う！
[![](https://img.youtube.com/vi/hcj6HXw9-OQ/0.jpg)](https://www.youtube.com/watch?v=hcj6HXw9-OQ)

上の画像クリックでYouTubeに飛びます。この動画ではUnityをインストールし、
プロジェクトを作成した空の状態から、Prefabを使ってノーコードでSymbolを使っている参考動画です。

* トランスファートランザクション
* モザイク残高の取得
* WebSocketを使ってトランザクション承認を監視

などを解説しています。
コードを書いて使われる場合は以下を参考にしてください。

# Installation
<a href="https://github.com/0x070696E65/Symnity/releases">Release</a>よりSymnity.unitypackageをダウンロードし、プロジェクトのAssetフォルダにて展開してください。

# Usage
基本的な構文はTypeScript版Symbol-SDKと同じようにしています。ただし、メソッドの最初の一文字は大文字です。
こちらを参考にしていただければだいたい使えるかと思います。<br>
https://symbol.github.io/symbol-sdk-typescript-javascript/1.0.3/

今の所以下トランザクションは使えます。
* （メッセージ付）送金トランザクション
* メタデータ割当・更新トランザクション
* マルチシグトランザクション
* リボーカブルモザイク没収トランザクション
* それらのアグリゲートトランザクションコンプリート（ボンデッド未対応）
* シークレットロック＆プルーフトランザクション

例）
```c#
using Symnity.Http;
using Symnity.Http.Model;
using Symnity.Model.Accounts;
using Symnity.Model.Network;
using Symnity.UnityScript;

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

HttpUtiles.Announce(Node, signedTx.Payload).Forget();
```

SymbolのAPIからデータ取得も以下は可能です。
* アカウントデータ
* トランザクションデータ
* メタデータ
* マルチシグデータ

src/Http/Model<br>
以下にあるクラスを利用します。<br>
（参考）

https://symbol.github.io/symbol-openapi/v1.0.3/


```c#
var Node = "NODE_URL";
Debug.Log("--アカウントデータ取得--");
var accountData = await ApiAccount.GetAccountInformation(node, "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ");
Debug.Log(RawAddress.AddressToString(ConvertUtils.GetBytes(accountData.account.address)));

var mosaicId = "3A8416DB2D53B6C8";
var mosaic = accountData.account.mosaics.Where(mosaic=>mosaic.id == mosaicId);
Debug.Log(mosaic.ToList()[0].amount);

Debug.Log("--アカウントデータ検索--");
var acountQueryParameters = new ApiAccount.AccountQueryParameters
{
    mosaicId = "3A8416DB2D53B6C8"
};
var accounts = await ApiAccount.SearchAccounts(node, acountQueryParameters);
Debug.Log(RawAddress.AddressToString(ConvertUtils.GetBytes(accounts.data[0].account.address)));

Debug.Log("--トランザクションデータ取得--");
var transactionData =
    await ApiTransaction.GetConfirmedTransaction(
        Node, "97E74C42E4DB83684011B4D29ADA6A5EDF03A87173D6635A8EA7B97CA6988088");
Debug.Log(RawAddress.AddressToString(ConvertUtils.GetBytes(
    transactionData.transaction.recipientAddress)));
Debug.Log(ConvertUtils.HexToChar(transactionData.transaction.message));

Debug.Log("--トランザクションデータ検索--");
var transactionQueryParameters = new ApiTransaction.TransactionQueryParameters
{
    recipientAddress = "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ",
    signerPublicKey = "DABACD828039B0F625D7A0F77AF7C08CD343AA94067B07D7C9A8DE7AA99BDEB2"
};
var transactions = await ApiTransaction.SearchConfirmedTransactions(node, transactionQueryParameters);
Debug.Log(ConvertUtils.HexToChar(transactions.data[0].transaction.message));

Debug.Log("--メタデータ取得--");
var metadataQueryParameters = new ApiMetadata.MetadataQueryParameters
{
    sourceAddress = "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ",
    scopedMetadataKey = "19670280EC3E4E7D",
    targetAddress = "TCOHSBNTWYNFUWP2PLGSGDK6EWE4BC5TFZNQBLI"
};

var metadataData = await ApiMetadata.SearchMetadata(node, metadataQueryParameters);
Debug.Log(ConvertUtils.HexToChar(metadataData.data[0].metadataEntry.value));

Debug.Log("--マルチシグデータ取得--");
var address = "TAIVS4GFLTZQVJGHCQD232Y3L5BSP2F27XRDBFQ";

var multisigRoot = await ApiMultisig.GetMultisigAccountInfomation(node, address);
Debug.Log(RawAddress.AddressToString(ConvertUtils.GetBytes(multisigRoot.multisig.multisigAddresses[0])));

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
