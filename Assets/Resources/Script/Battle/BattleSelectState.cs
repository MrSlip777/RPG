/*
 *戦闘の選択状態
 * 
 *参考URL http://fantastic-works.com/archives/148
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleSelectState : MonoBehaviour {

    //シングルトン実装
    private static BattleSelectState mInstance;

    // 唯一のインスタンスを取得します。
    public static BattleSelectState Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new BattleSelectState();
            }

            return mInstance;
        }

    }

    private BattleSelectState()
    {

    }

    //UIの状態
    private enum eUIStatus
    {
        eUIStatus_Main = 0,
        eUIStatus_focusEnemy = 10,
        eUIStatus_Skill = 1,
        eUIStatus_Item = 2,
    };

    //UIの状態
    private static eUIStatus mUIstate;
    private static eUIStatus mUIpreviousstate;

    //各種インスタンス定義
        //UI関係
    MainMenuController mMainMenuController = MainMenuController.Instance;
    SubMenuController mSubMenuController = SubMenuController.Instance;

    //戦闘状態データ
    BattleStateDataSinglton mBattleStateDataSingleton = BattleStateDataSinglton.Instance;

    //ターゲット表示
    TergetController mTergetController = new TergetController();

    //キャラクターステータス表示ウインドウ
    CharacterStatusController mCharacterStatusController
        = CharacterStatusController.Instance;

    //キャラクターのデータ（シングルトン）
    static CharacterDataSingleton mCharacterDataSingleton
         = CharacterDataSingleton.Instance;

    //キー判定フラグ
    private bool IsPush = false;

    //キーが押されたか判定
    private void JudgePushDown()
    {
        if (Input.anyKeyDown)
        {
            IsPush = true;
        }
        else
        {
            IsPush = false;
        }

    }

    // Use this for initialization
    public void _Start()
    {
        //データ初期化
        mCharacterDataSingleton.SetBattleCharacterObject();

        //UI表示
        mCharacterStatusController.ShowCharacterStatus();

        TurnStart();
    }

    //ターン開始時の初期化
    public void TurnStart()
    {
        mMainMenuController.InitialSelectButton();

        //UI状態　選択肢表示がデフォルト
        mUIstate = eUIStatus.eUIStatus_Main;
        mUIpreviousstate = mUIstate;

        //ターゲットの初期化
        mTergetController.ShowHide_Terget(false);

        //キャラクターステータス表示ウインドウの初期化
        mCharacterStatusController.InitialSelectCharacter();

        //行動選択者の初期化
        mCharacterDataSingleton.TurnStartCharacter();
    }

    // Update is called once per frame
    public void _Update() {
        //キーが押されたか判定
        JudgePushDown();

        //選択中のオブジェクトを取得し、スキル、アイテムであれば、説明ウインドウを更新する
        GameObject gEvent = EventSystem.current.currentSelectedGameObject;
        if (gEvent != null &&
            (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            mSubMenuController.ChangeDescription(gEvent.name);
        }

        //キャンセル処理
        Implement_Cancel();

        //行動決定時の処理
        Implement_DecideAct();
    }

    //UIの初期設定
    private void SetUIDefault()
    {
        UIState(true, false);
        //フォーカスを攻撃ボタンにする
        mMainMenuController.
        SetFocus_Button(MainMenuController.eMainButton.eButton_Attack);

        //UI状態をメインの項目選択状態にする
        mUIstate = eUIStatus.eUIStatus_Main;

        //前の状態を更新する
        mUIpreviousstate = mUIstate;
    }

    //UI表示をフラグで切り替える
    private void UIState(bool IsShowMain, bool IsShowEnemyTerget)
    {
        //選択肢ボタンを表示/非表示にする
        mMainMenuController.ShowHide_Button(IsShowMain);
        //ターゲット画像を表示/非表示する
        mTergetController.ShowHide_Terget(IsShowEnemyTerget);
    }

    //キャンセル動作
    private void Implement_Cancel()
    {
        if (IsPush == true
            && (Input.GetKey(KeyCode.Escape) == true
            || Input.GetMouseButton(1) == true))
        {

            switch (mUIstate)
            {
                case eUIStatus.eUIStatus_focusEnemy:
                    if (mUIpreviousstate == eUIStatus.eUIStatus_Main)
                    {
                        //フォーカスを攻撃にして、メイン選択ウインドウを表示
                        SetUIDefault();
                    }
                    else if (mUIpreviousstate == eUIStatus.eUIStatus_Skill)
                    {
                        UIState(true, false);
                        Implement_Button_Skill();
                    }
                    else if (mUIpreviousstate == eUIStatus.eUIStatus_Item)
                    {
                        UIState(true, false);
                        Implement_Button_Item();
                    }

                    break;

                case eUIStatus.eUIStatus_Skill:

                    //スクロールビューを非表示にする
                    mSubMenuController.HideSubMenu();
                    mMainMenuController.EnableDisable_Button(true);
                    //フォーカスをスキルボタンにする
                    mMainMenuController.
                        SetFocus_Button(MainMenuController.eMainButton.eButton_Skill);

                    //UI状態をメインの項目選択状態にする
                    mUIstate = eUIStatus.eUIStatus_Main;

                    //前の状態を更新する
                    mUIpreviousstate = mUIstate;
                    break;

                case eUIStatus.eUIStatus_Item:

                    //スクロールビューを非表示にする
                    mSubMenuController.HideSubMenu();
                    mMainMenuController.EnableDisable_Button(true);
                    //フォーカスをアイテムボタンにする
                    mMainMenuController.
                        SetFocus_Button(MainMenuController.eMainButton.eButton_Item);

                    //UI状態をメインの項目選択状態にする
                    mUIstate = eUIStatus.eUIStatus_Main;

                    //前の状態を更新する
                    mUIpreviousstate = mUIstate;
                    break;
                case eUIStatus.eUIStatus_Main:
                    //パーティ最小人数は1であり、1以下である場合はなにも処理を実行しない
                    if (1 < mCharacterDataSingleton.GetSelectingCharacter())
                    {
                        //次のキャラを行動可能状態にする
                        mCharacterStatusController
                            .SetFocus_Character(mCharacterDataSingleton.BeforeSelectingCharacter());
                        //行動をデフォルトに戻す
                        SetUIDefault();
                    }
                    break;

                default:
                    break;
            }
        }
    }

    //行動決定時の処理
    private void Implement_DecideAct()
    {
        if (IsPush == true && mUIstate == eUIStatus.eUIStatus_focusEnemy
            && (Input.GetKey(KeyCode.KeypadEnter) == true
            || Input.GetMouseButton(0) == true))
        {
            //パーティ最大人数は4であり、4以上である場合は行動選択画面を終了する
            if (4 > mCharacterDataSingleton.GetSelectingCharacter())
            {

                //次のキャラを行動可能状態にする
                mCharacterStatusController
                    .SetFocus_Character(mCharacterDataSingleton.NextSelectingCharacter());
                //行動をデフォルトに戻す
                SetUIDefault();
            }
            else
            {
                
                UIState(false, false);

                //暫定的に行動順
                mCharacterStatusController.SetNoFocus();
                //前の状態を更新する
                mUIpreviousstate = mUIstate;

                //戦闘画面状態を敵ターゲット選択状態にする
                mBattleStateDataSingleton.BattleStateMode
                    = BattleStateDataSinglton.eBattleState.eBattleState_SelectEnd;

            }
        }
    }

    //攻撃ボタンを押下時の処理
    public void Implement_Button_Attack()
    {

        //敵ターゲット表示させる
        UIState(false, true);

        //前の状態を更新する
        mUIpreviousstate = mUIstate;

        //UI状態を敵ターゲット選択状態にする
        mUIstate = eUIStatus.eUIStatus_focusEnemy;
    }

    //スキルボタンを押下時の処理
    public void Implement_Button_Skill()
    {
        //設定
        mSubMenuController
            .SetContents(MainMenuController.eMainButton.eButton_Skill,
            mCharacterDataSingleton.GetSkillIndex(1));

        //サブメニュー表示
        mSubMenuController.ShowSubMenu();

        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(false);

        //前の状態を更新する
        mUIpreviousstate = mUIstate;

        //UI状態をスキル選択状態にする
        mUIstate = eUIStatus.eUIStatus_Skill;
    }

    //アイテムボタンを押下時の処理
    public void Implement_Button_Item()
    {
        mSubMenuController
            .SetContents(MainMenuController.eMainButton.eButton_Item, null);
        //サブメニュー表示
        mSubMenuController.ShowSubMenu();

        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(false);

        //前の状態を更新する
        mUIpreviousstate = mUIstate;

        //UI状態をアイテム選択状態にする
        mUIstate = eUIStatus.eUIStatus_Item;
    }

    //逃げるボタンを押下時の処理
    public void Implement_Button_Escape()
    {
        //シーン終了処理？
    }

    //サブメニュー内のボタンが押されたときの処理
    public void Implement_Button_Content()
    {
        //敵ターゲット表示させる
        UIState(false, true);
        //サブメニュー非表示
        mSubMenuController.HideSubMenu();

        //前の状態を更新する
        mUIpreviousstate = mUIstate;

        //UI状態を敵選択状態にする（暫定的）
        mUIstate = eUIStatus.eUIStatus_focusEnemy;
    }

    //ボタン名の取得
    public string getButtonName(MainMenuController.eMainButton tergetButton)
    {
        return mMainMenuController.sButtonName[(int)tergetButton];
    }
}
