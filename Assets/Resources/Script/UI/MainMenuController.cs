//http://gametukurikata.com/program/uifocus

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    //シングルトン実装
    private static MainMenuController mInstance;

    // 唯一のインスタンスを取得します。
    public static MainMenuController Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new MainMenuController();
            }

            return mInstance;
        }

    }

    private MainMenuController()
    {

    }

    static private GameObject prefab_Panel_Button = null;

    //ボタンの各状態
    public enum eMainButton
    {
        eButton_Attack = 0,
        eButton_Skill = 1,
        eButton_Item = 2,
        eButton_Escape = 3,
    };

    //各ボタンの名前
    public string[] sButtonName = {
        "Button_Attack" ,
        "Button_Skill",
        "Button_Item",
        "Button_Escape"
    };

    private static GameObject[] gButton;

    //ボタンの最大数
    private readonly int iButton_MaxNumber = 4;

    //選択フォーカス初期値
    private readonly int iButton_Initialfocus = 0;

    //選択肢フォーカスの初期設定
    public void InitialSelectButton()
    {
        ShowPanel_Button();
 
        gButton[iButton_Initialfocus].GetComponent<Button>().Select();
    }

    //ボタンのフォーカス指定
    public void SetFocus_Button(eMainButton FocusButton)
    {
        gButton[(int)FocusButton].GetComponent<Button>().Select();
    }

    //ボタンを非表示/表示にする
    public void ShowHide_Button(bool IsShow)
    {
        if(IsShow == true)
        {
            ShowPanel_Button();
        }
        else
        {
            HidePanel_Button();
        }

    }

    //ボタングループ生成
    private void ShowPanel_Button()
    {
        if (prefab_Panel_Button == null)
        {
            //ローカル変数定義
            GameObject parentObject = null;

            //親オブジェクトの指定
            parentObject = GameObject.Find("Canvas");

            //プレハブ指定
            prefab_Panel_Button = Instantiate(
                (GameObject)Resources.Load("Prefabs/Panel_Button"));
            prefab_Panel_Button.transform.SetParent(parentObject.transform,false);

            //ボタン定義
            gButton = new GameObject[iButton_MaxNumber];

            for (int i = 0; i < iButton_MaxNumber; i++)
            {
                gButton[i] = GameObject.Find(sButtonName[i]);
            }
        }
    }

    //ボタングループ消去
    private void HidePanel_Button()
    {
        Destroy(prefab_Panel_Button);
        prefab_Panel_Button = null;

        for (int i = 0; i < iButton_MaxNumber; i++)
        {
            Destroy(gButton[i]);
            gButton[i] = null;
        }
    }

    //ボタンを有効/無効にする
    public void EnableDisable_Button(bool IsEnable)
    {
        for (int i = 0; i < iButton_MaxNumber; i++)
        {
            gButton[i].GetComponent<Button>().interactable = IsEnable;
        }
    }

}