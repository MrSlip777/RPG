//http://gametukurikata.com/program/uifocus

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    //ボタンの各状態
    public enum eMainButton
    {
        eButton_Attack = 0,
        eButton_Skill = 1,
        eButton_Item = 2,
        eButton_Escape = 3,
    };

    //各ボタンの名前
    private string[] sButtonName = {
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
        gButton = new GameObject[iButton_MaxNumber];

        for (int i = 0; i < iButton_MaxNumber; i++)
        {
            gButton[i] = GameObject.Find(sButtonName[i]);
        }

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
        for (int i = 0; i < iButton_MaxNumber; i++)
        {
            gButton[i].SetActive(IsShow);
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