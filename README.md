# HowOld@chomado について

HowOld@chomadoは Microsoft Azure の Cognitive Services の Face API を Xamarin.Forms 製のアプリケーションから呼び出すためのサンプルプログラムです。

## アプリケーションの動作

人の顔写真と名前と年齢を登録しておくことで遊ぶことができます。
アプリケーションに登録されている人同士で写真を撮ることで登録された写真から誰が写っているのか認識して、Face API で認識された年齢と実年齢の差が一番大きい人が勝者として黄色い矩形と文字で表示されます。

## 使用しているテクノロジ

主に以下のライブラリを使用して作成しています。

- Prism.Forms
    - MVVM をするためのライブラリ
- Autofac
    - DIコンテナ
- SkiaSharp
    - 2D描画ライブラリ
-sqlite-net-pcl
    - SQLiteを使うためのライブラリ
- Media Plugin for Xamarin and Windows
    - クロスプラットフォームでカメラなどを使うためのライブラリ
- Xam.Plugins.Forms.ImageCircle
    - 画像を丸く表示するためのライブラリ
- Microsoft Cognitive Services Face API Client Library
    - Microsoft AzureのCognitive ServicesのFace APIをC#から呼ぶためのライブラリ

## デモでの使用方法

- 自分の顔を登録した状態のアプリケーションを準備しておく
- デモ用に対戦相手の人の顔と年齢と名前を登録する
- 二人で並んだ写真を撮影する
- より実年齢より若く判定された人が黄色の矩形で囲まれて表示される。その他の人は、水色の矩形で囲まれて表示される

