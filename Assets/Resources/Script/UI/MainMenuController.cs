//http://gametukurikata.com/program/uifocus

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
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

    //UIを作成する関数
    public void MakeUI(){
         if (prefab_Panel_Button == null)
        {
            //親オブジェクトの指定
            GameObject parentObject
             = GameObject.Find("Canvas");

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

    //UIインスタンスを削除する
    private void DestroyUI()
    {
        Destroy(prefab_Panel_Button);
        prefab_Panel_Button = null;

        for (int i = 0; i < iButton_MaxNumber; i++)
        {
            Destroy(gButton[i]);
            gButton[i] = null;
        }
    }

    //選択肢フォーカスの初期設定
    public void InitialSelectButton()
    {
        ShowHide_Button(true);
 
        gButton[iButton_Initialfocus].GetComponent<Button>().Select();
    }

    //ボタンのフォーカス指定
    public void SetFocus_Button(eMainButton FocusButton)
    {
        gButton[(int)FocusButton].GetComponent<Button>().Select();
        /*
        gButton[(int)FocusButton].GetComponent<Image> ().color
         = new Color(125.0f, 233.0f, 255.0f);
         */
    }

    //ボタンを非表示/表示にする
    public void ShowHide_Button(bool IsShow)
    {
        if (prefab_Panel_Button != null)
        {
            prefab_Panel_Button.SetActive(IsShow);
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