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
    MainMenuController mMainMenuController = null;
    SubMenuController mSubMenuController = null;
    TergetController mTergetController = null;

    //戦闘状態データ
    static BattleStateDataSinglton mBattleStateDataSingleton;

    //キャラクターデータシングルトン
    static　CharacterDataSingleton mCharacterDataSingleton;

    //キャラクターステータス表示ウインドウ
    static CharacterStatusController mCharacterStatusController;

    //キャラクター行動マネージャ
    static BattleCharacterManager mBattleCharacterManager;

    //敵データ（シングルトン）
    static EnemiesDataSingleton mEnemiesDataSingleton;

    //対象種類（Autoへ渡る）
    private eTergetScope mTergetScope;

    //行動者の選択スキルID
    private static int mActorSkillID;


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
        //ローカル変数定義
        GameObject parentObject = null;

        //インスタンス取得
        mMainMenuController = gameObject.GetComponent<MainMenuController>();
        mSubMenuController = gameObject.GetComponent<SubMenuController>();
        mTergetController = gameObject.GetComponent<TergetController>();

        parentObject = GameObject.Find("DataSingleton");
        mCharacterDataSingleton = parentObject.GetComponent<CharacterDataSingleton>();
        mEnemiesDataSingleton = parentObject.GetComponent<EnemiesDataSingleton>();
        mBattleStateDataSingleton = parentObject.GetComponent<BattleStateDataSinglton>();

        parentObject = GameObject.Find("Panel_CharacterStatus");
        mCharacterStatusController 
        = parentObject.GetComponent<CharacterStatusController>();
        mBattleCharacterManager
        = parentObject.GetComponent<BattleCharacterManager>();

        //データ初期化
        mCharacterDataSingleton.SetBattleCharacterObject();

        //UI表示
        mCharacterStatusController.MakeUI();
        mMainMenuController.MakeUI();
        mTergetController.MakeUI();
        mSubMenuController.MakeUI();    

        TurnStart();
    }

    //ターン開始時の初期化
    public void TurnStart()
    {
        //UI状態　選択肢表示がデフォルト
        mUIstate = eUIStatus.eUIStatus_Main;
        mUIpreviousstate = mUIstate;

        //UI表示状態設定
        mMainMenuController.InitialSelectButton();
        mSubMenuController.HideSubMenu();
        mTergetController.ShowHide_Terget(eTergetScope.Hide);
        mCharacterStatusController.InitialSelectCharacter();
        mBattleCharacterManager.TurnStartCharacter();

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
            mSubMenuController.ChangeContentScrollView(gEvent.name);
        }

        //キャンセル処理
        Implement_Cancel();
    }

    //UIの初期設定
    private void SetUIDefault()
    {
        UIState(true,eTergetScope.Hide);
        //フォーカスを攻撃ボタンにする
        mMainMenuController.
        SetFocus_Button(MainMenuController.eMainButton.eButton_Attack);

        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(true);

        //UI状態をメインの項目選択状態にする
        mUIstate = eUIStatus.eUIStatus_Main;

        //前の状態を更新する
        mUIpreviousstate = mUIstate;
    }

    //UI表示をフラグで切り替える
    private void UIState(bool IsShowMain, eTergetScope Scope)
    {
        //変数を更新する
        mTergetScope = Scope;

        //選択肢ボタンを表示/非表示にする
        mMainMenuController.ShowHide_Button(IsShowMain);
        //ターゲット画像を表示/非表示する
        mTergetController.ShowHide_Terget(Scope);
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
                        UIState(true, eTergetScope.Hide);
                        Implement_Button_Skill();
                    }
                    else if (mUIpreviousstate == eUIStatus.eUIStatus_Item)
                    {
                        UIState(true, eTergetScope.Hide);
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
                    if (1 < mBattleCharacterManager.GetSelectingCharacter())
                    {
                        //次のキャラを行動可能状態にする
                        mCharacterStatusController
                            .SetFocus_Character(mBattleCharacterManager.BeforeSelectingCharacter());
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
    //ターゲット表示の位置を渡す（単体用でエフェクト表示に使用する）
    public void Implement_DecideAct(int tergetNum,Vector3[] tergetPos)
    {
        ActorObject actorObject = new ActorObject();
        actorObject.actorNum = mBattleCharacterManager.GetSelectingCharacter();
        actorObject.speed = mCharacterDataSingleton.CharaSpeed(actorObject.actorNum);
        actorObject.belong = eActorScope.Friend;

        //保持している行動者のスキルIDを渡す（キャンセル操作に注意）
        actorObject.skillID = mActorSkillID;

        //ターゲットの渡す
        actorObject.terget = mTergetScope;
        actorObject.tergetNum = tergetNum;
        actorObject.tergetPos = tergetPos;

        mBattleStateDataSingleton.ActorObject = actorObject;


        //パーティ最大人数は4であり、4以上である場合は行動選択画面を終了する
        if (4 > mBattleCharacterManager.GetSelectingCharacter())
        {

            //次のキャラを行動可能状態にする
            mCharacterStatusController
                .SetFocus_Character(mBattleCharacterManager.NextSelectingCharacter());
            //行動をデフォルトに戻す
            SetUIDefault();
        }
        else
        {
            //敵の行動を自動選択により設定する
            for (int i = 0; i < mEnemiesDataSingleton.EnemiesNum; i++)
            {
                mBattleStateDataSingleton.ActorObject = mEnemiesDataSingleton.getAutoActorData(i+1);
            }

            //ターゲットを非表示にする
            UIState(false, eTergetScope.Hide);

            //暫定的に行動順
            mCharacterStatusController.SetNoFocus();
            //前の状態を更新する
            mUIpreviousstate = mUIstate;

            //戦闘画面状態を敵ターゲット選択状態にする
            mBattleStateDataSingleton.BattleStateMode
                = BattleStateDataSinglton.eBattleState.eBattleState_SelectEnd;

        }

    }

    //攻撃ボタンを押下時の処理
    public void Implement_Button_Attack()
    {
        //攻撃のスキルIDを設定
        mActorSkillID = 1;

        //敵ターゲット表示させる
        UIState(false, eTergetScope.forOne);
        
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
    public void Implement_Button_Content(int number)
    {
        int[] SkillId = mCharacterDataSingleton.GetSkillIndex(1);

        //行動者のスキルIDを保持する
        mActorSkillID = SkillId[number];

        //敵ターゲット表示させる
        UIState(false, 
            mCharacterDataSingleton.GetSkillScope(SkillId[number]));
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
