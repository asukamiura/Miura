# Dragon Soul

---

## ジャンル
3Dバトルアクション

---

## 開発時期（期間）
2024年10月 ～ 2025年8月（約11カ月）

---

## 開発メンバー・担当箇所
1人（個人制作）

※サウンド実装については  
『Unityサウンドエキスパート養成講座』を参考にして実装しています。

### スクリプト配置場所
実装したスクリプトは全て以下フォルダに入っています。
Assets/DragonSoul/Scripts

---

## コンセプト
ジャスト回避やジャストガードで有利な状況を作り、  
タイミングよく攻撃を決める爽快感のあるバトルアクション。

---

## ゲーム概要
本作は、ジャスト回避・ジャストガードを駆使して敵の攻撃を見極めながら戦う  
3Dバトルアクションゲームです。

敵が攻撃する直前に発生するエフェクトの色によって、  
プレイヤーの有効な行動が変化します。

- 赤いエフェクト：回避が有効  
- 青いエフェクト：ガードが有効  

それぞれをタイミングよく成功させることで  
ジャスト回避 / ジャストガードとなり、強力な反撃が可能です。

また、ジャストアクション成功時に獲得できる  
ジャストポイントを消費することで、

- 回復
- パワーアップ

といった行動が可能になります。

---

## 操作方法（キーボード）

| 操作 | キー |
|---|---|
| 移動 | WASD |
| 視点移動 | マウス |
| 回避 | Space |
| ガード | 右クリック |
| 攻撃 | 左クリック |
| 必殺技 | Q |
| 回復 | E |
| パワーアップ | C |
| カメラロックオン | ホイールクリック |

---

## 操作方法（XBOXコントローラー）

| 操作 | ボタン |
|---|---|
| 移動 | 左スティック |
| 視点移動 | 右スティック |
| 回避 | A |
| ガード | RB |
| 攻撃 | X |
| 必殺技 | LB |
| 回復 | Y |
| パワーアップ | B |
| カメラロックオン | 右スティック押し込み |

---

## 開発環境 / 使用技術

- Unity 2022.3.62
- Visual Studio Community 2022（17.10.4）
- C#
- SourceTree
- GitHub

---

## 動作環境
PC

---

## 苦労した点・工夫点

- ジャスト回避・ジャストガード成功時に、  
  プレイヤーが達成感や爽快感を得られるよう  
  カメラワークやエフェクト演出を工夫しました。

- プレイヤーおよび敵の行動管理に  
  ステートパターンを採用し、  
  拡張性・保守性を意識した設計を行いました。  
  また、循環参照を避け、依存関係を最小限に抑えています。

- エフェクト管理には ObjectPool を使用し、  
  生成・破棄コストを抑えた設計としました。

- UIには MVPパターンを採用し、  
  表示とロジックを分離することで  
  保守性・拡張性の高い構造を構築しました。

---

## 使用アセット・サイト

（※各種アセット・音源・効果音サイトは権利表記のため記載）

Knight Warrior Mecanim Animation Pack  
https://assetstore.unity.com/packages/3d/animations/knight-warrior-mecanim-animation-pack-38814

RPG_Animations_Pack_FREE  
https://assetstore.unity.com/packages/3d/animations/rpg-animations-pack-free-288783

Runner Action Animation Pack  
https://assetstore.unity.com/packages/3d/animations/runner-action-animation-pack-153906

Frank climax's Katana (+ UE4 FBX)  
https://assetstore.unity.com/packages/3d/animations/frank-climax-s-katana-ue4-fbx-147166

TransformAnimation SXB  
https://assetstore.unity.com/packages/3d/animations/transformanimation-sxb-159419

Sword and Shield Pack  
https://www.mixamo.com/

MYFG - Weapon Pack  
https://assetstore.unity.com/packages/3d/props/weapons/myfg-weapon-pack-14192

Dragon for Boss Monster : PBR  
https://assetstore.unity.com/packages/3d/characters/creatures/dragon-for-boss-monster-pbr-78923

Magic Arsenal  
https://assetstore.unity.com/packages/vfx/particles/spells/magic-arsenal-20869

Sword slashes PRO  
https://assetstore.unity.com/packages/vfx/particles/sword-slashes-pro-173450

Hit Effects FREE  
https://assetstore.unity.com/packages/vfx/particles/hit-effects-free-284613

Status and Auras FREE  
https://assetstore.unity.com/packages/vfx/particles/spells/status-and-auras-free-289450

Ultimate 10+ Shaders  
https://assetstore.unity.com/packages/vfx/shaders/ultimate-10-shaders-168611

Zap VFX - URP  
https://assetstore.unity.com/packages/vfx/particles/spells/zap-vfx-urp-303479

Hyper Casual FX  
https://assetstore.unity.com/packages/vfx/particles/hyper-casual-fx-200333

Casual_Hit  
https://assetstore.unity.com/packages/vfx/particles/casual-hit-318002

Desert Rocks And Boulders  
https://assetstore.unity.com/packages/3d/environments/landscapes/desert-rocks-and-boulders-129874

UI SFX Free Pack  
https://assetstore.unity.com/packages/audio/sound-fx/ui-sfx-free-pack-245925

Deadly_Creatures_Pack1_v1  
https://assetstore.unity.com/packages/audio/sound-fx/creatures/deadly-creatures-pack1-v1-280813

Music-Note.jp  
https://www.music-note.jp/

Springin’ Sound Stock  
https://www.springin.org/sound-stock/

DOVA-SYNDROME  
https://dova-s.jp/

Modern RPG - Free icons pack  
https://assetstore.unity.com/packages/2d/gui/icons/modern-rpg-free-icons-pack-264706

Input Prompts  
https://kenney.nl/assets/input-prompts

---

## 外部コード・ライセンス表記

以下のスクリプトは MIT License のもとで使用しています。

- AudioMixerExtentions.cs  
- AudioSourceExtentions.cs  
- GameSePlayer.cs  
- SoundManager.cs  

Copyright (c) 2019 Takaaki Ichijo

MIT License
