# Symnity
SymnityはUnityでブロックチェーンであるSymbolを利用するためのアセットです。

※TypescriptやJavaのSDKを参考に作成していますが、まだ完成していません。（2022年1月11日）
ひとまず最低限の機能はありますので利用はできるかと思います。

# Installation
インストール、使い方などはこちらを参考にしてください。
全ての機能は記事には出来ていませんが徐々に書いていこうと思います。<br>
https://symnity.dev/

# Usage
基本的な構文はTypeScript版Symbol-SDKと同じようにしています。ただし、メソッドの最初の一文字は大文字です。
こちらを参考にしていただければだいたい使えるかと思います。<br>
https://symbol.github.io/symbol-sdk-typescript-javascript/1.0.3/

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
