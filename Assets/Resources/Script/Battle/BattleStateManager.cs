﻿/*
*参考URL http://fantastic-works.com/archives/148
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.EventSystems;

public class BattleStateManager : MonoBehaviour
{

    //インスタンス定義
    private static BattleStateManager mInstance;

    // 唯一のインスタンスを取得します。
    public static BattleStateManager Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new BattleStateManager();
            }

            return mInstance;
        }

    }

    //シングルトン実装
    private BattleStateManager()
    {

    }

    MainMenuController mMainMenuController = new MainMenuController();
    SubMenuController mSubMenuController = new SubMenuController();
    CharacterStatusController mCharacterStatusController
        = new CharacterStatusController();
    EnemyGraphicController mEnemyGraphicController
        = new EnemyGraphicController();

    TergetController mTergetController = new TergetController();

    Game_Action action = new Game_Action();

    static CharacterDataSingleton mCharacterDataSingleton;

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
    void Start () {

        //インスタンス取得
        mCharacterDataSingleton = CharacterDataSingleton.Instance;

        mMainMenuController.InitialSelectButton();
        mCharacterStatusController.InitialSelectCharacter();

        mEnemyGraphicController.ShowEnemy();
        mTergetController.ShowHide_Terget(false);

        //UI状態　選択肢表示がデフォルト
        mUIstate = eUIStatus.eUIStatus_Main;
        mUIpreviousstate = mUIstate;

        mCharacterDataSingleton.SetBattleCharacterObject();
    }

    // Update is called once per frame
    void Update () {

        //キーが押されたか判定
        JudgePushDown();

        //選択中のオブジェクトを取得する
        GameObject gEvent = EventSystem.current.currentSelectedGameObject;
        if (gEvent != null && 
            (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            mSubMenuController.ChangeDescription(gEvent.name);
        }

        //キャンセル動作
        if (IsPush == true 
            &&(Input.GetKey(KeyCode.Escape) == true
            || Input.GetMouseButton(1) == true)) {

            switch (mUIstate)
            {
                case eUIStatus.eUIStatus_focusEnemy:
                    if (mUIpreviousstate == eUIStatus.eUIStatus_Main) {
                        UIState(true, false);
                        //フォーカスを攻撃ボタンにする
                        mMainMenuController.
                        SetFocus_Button(MainMenuController.eMainButton.eButton_Attack);

                        //UI状態をメインの項目選択状態にする
                        mUIstate = eUIStatus.eUIStatus_Main;

                        //前の状態を更新する
                        mUIpreviousstate = mUIstate;
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
                default:
                    break;
            }
        }
    }


    //攻撃ボタンを押下時の処理
    public void Implement_Button_Attack()
    {

        //敵ターゲット表示させる
        UIState(false,true);

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
            .SetContents(MainMenuController.eMainButton.eButton_Item,null);
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

    //UI表示をフラグで切り替える
    private void UIState(bool IsShowMain,bool IsShowEnemyTerget)
    {
        //選択肢ボタンを表示/非表示にする
        mMainMenuController.ShowHide_Button(IsShowMain);
        //ターゲット画像を表示/非表示する
        mTergetController.ShowHide_Terget(IsShowEnemyTerget);
    }
}
