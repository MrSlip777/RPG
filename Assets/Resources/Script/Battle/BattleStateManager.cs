/*
*参考URL http://fantastic-works.com/archives/148
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour {

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

    static CharacterDataSingleton mCharacterDataSingleton;

    //UIの状態
    private enum eUIStatus
    {
        eUIStatus_Main = 0,
        eUIStatus_focusEnemy = 10,
        eUIStatus_Skill = 1,
        eUIStatus_Item = 2,
    };

    private static eUIStatus mUIstate;

    // Use this for initialization
    void Start () {

        //インスタンス取得
        mCharacterDataSingleton = CharacterDataSingleton.Instance;

        mMainMenuController.InitialSelectButton();
        mCharacterStatusController.InitialSelectCharacter();
       
        //UI状態　選択肢表示がデフォルト
        mUIstate = eUIStatus.eUIStatus_Main;

        mCharacterDataSingleton.SetBattleCharacterObject();
    }

    // Update is called once per frame
    void Update () {

        //キャンセル動作
        if (Input.GetKey(KeyCode.Escape) == true
            || Input.GetMouseButton(1) == true) {

            switch (mUIstate)
            {
                case eUIStatus.eUIStatus_focusEnemy:
                    UIState(true, false);
                    //フォーカスを攻撃ボタンにする
                    mMainMenuController.
                    SetFocus_Button(MainMenuController.eMainButton.eButton_Attack);
                    break;

                case eUIStatus.eUIStatus_Skill:

                    //スクロールビューを表示させる
                    mSubMenuController.HideScrollView();
                    mMainMenuController.EnableDisable_Button(true);
                    //フォーカスをスキルボタンにする
                    mMainMenuController.
                        SetFocus_Button(MainMenuController.eMainButton.eButton_Skill);
                    break;

                case eUIStatus.eUIStatus_Item:

                    //スクロールビューを表示させる
                    mSubMenuController.HideScrollView();
                    mMainMenuController.EnableDisable_Button(true);
                    //フォーカスをスキルボタンにする
                    mMainMenuController.
                        SetFocus_Button(MainMenuController.eMainButton.eButton_Item);
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
        //UI状態を敵ターゲット選択状態にする
        mUIstate = eUIStatus.eUIStatus_focusEnemy;
    }

    //スキルボタンを押下時の処理
    public void Implement_Button_Skill()
    {
        //スクロールビューを表示させる
        mSubMenuController
            .ShowScrollView(mCharacterDataSingleton.GetSkillName(1));
        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(false);

        //UI状態をスキル選択状態にする
        mUIstate = eUIStatus.eUIStatus_Skill;
    }

    //アイテムボタンを押下時の処理
    public void Implement_Button_Item()
    {
        //コンテンツ
        string[] ContentName = { "アイテム１", "アイテム２" };

        //スクロールビューを表示させる
        mSubMenuController
            .ShowScrollView(ContentName);
        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(false);

        //UI状態をアイテム選択状態にする
        mUIstate = eUIStatus.eUIStatus_Item;
    }

    //逃げるボタンを押下時の処理
    public void Implement_Button_Escape()
    {
        //シーン終了処理？
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
        GameObject terget_image
                = GameObject.Find("terget");
        terget_image.GetComponent<SpriteRenderer>()
            .enabled = IsShowEnemyTerget;

    }
}
