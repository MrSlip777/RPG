using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour {

    MainMenuController mMainMenuController = new MainMenuController();
    SubMenuController mSubMenuController = new SubMenuController();
    CharacterStatusController mCharacterStatusController
        = new CharacterStatusController();

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
        mMainMenuController.InitialSelectButton();
        //mMainMenuController.ShowHide_Button(true);
        mCharacterStatusController.InitialSelectCharacter();
        mCharacterStatusController.ActiveCharacter();
        
        //UI状態　選択肢表示がデフォルト
        mUIstate = eUIStatus.eUIStatus_Main;
    }
	
	// Update is called once per frame
	void Update () {

        //キャンセル動作
        if (Input.GetKey(KeyCode.Escape) == true
            || Input.GetMouseButton(1) == true) {

            
            if (mUIstate == eUIStatus.eUIStatus_focusEnemy)
            {
                UIState(true, false);
                //フォーカスを攻撃ボタンにする
                mMainMenuController.
                    SetFocus_Button(MainMenuController.eMainButton.eButton_Attack);
            }

            if (mUIstate == eUIStatus.eUIStatus_Skill)
            {
                //スクロールビューを表示させる
                mSubMenuController.HideScrollView();
                mMainMenuController.EnableDisable_Button(true);
                //フォーカスをスキルボタンにする
                mMainMenuController.
                    SetFocus_Button(MainMenuController.eMainButton.eButton_Skill);
            }

            if (mUIstate == eUIStatus.eUIStatus_Item)
            {
                //スクロールビューを表示させる
                mSubMenuController.HideScrollView();
                mMainMenuController.EnableDisable_Button(true);
                //フォーカスをスキルボタンにする
                mMainMenuController.
                    SetFocus_Button(MainMenuController.eMainButton.eButton_Item);
            }
        }
    }

    //攻撃ボタンを押下時の処理
    public void OnClick_Button_Attack()
    {
        //敵ターゲット表示させる
        UIState(false,true);
        //UI状態を敵ターゲット選択状態にする
        mUIstate = eUIStatus.eUIStatus_focusEnemy;
    }

    //スキルボタンを押下時の処理
    public void OnClick_Button_Skill()
    {
        //スクロールビューを表示させる
        mSubMenuController.ShowScrollView(MainMenuController.eMainButton.eButton_Skill);
        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(false);

        //UI状態をスキル選択状態にする
        mUIstate = eUIStatus.eUIStatus_Skill;
    }

    //スキルボタンを押下時の処理
    public void OnClick_Button_Item()
    {
        //スクロールビューを表示させる
        mSubMenuController.ShowScrollView(MainMenuController.eMainButton.eButton_Item);
        //メインメニューを有効／無効にする
        mMainMenuController.EnableDisable_Button(false);

        //UI状態をアイテム選択状態にする
        mUIstate = eUIStatus.eUIStatus_Item;
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
