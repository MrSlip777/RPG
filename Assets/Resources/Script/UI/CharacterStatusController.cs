//http://gametukurikata.com/program/uifocus

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatusController : MonoBehaviour
{
    //キャラクターステータスの番号
    public enum eCharacterNum
    {
        eCharacter1 = 0,
        eCharacter2 = 1,
        eCharacter3 = 2,
        eCharacter4 = 3,
    };

    //キャラクターステータスの名前
    private string[] sCharacterNum = {
        "Character1" ,
        "Character2",
        "Character3",
        "Character4"
    };

    //UIの設定値
    private float f_maxColor = 255;
    private float[] f_focusColor = {125,233,255};

    //役割の設定値
    private static eCharacterNum mRole = eCharacterNum.eCharacter1;

    //役割を次キャラに渡す
    public void Increment_mRole(){
        mRole++;
    }

    //役割を次キャラに渡す
    public void decrement_mRole()
    {
        mRole--;
    }

    //キャラクター状態の初期設定
    public void InitialSelectCharacter()
    {
        SetFocus_Character(mRole);
    }

    //キャラクターのフォーカス指定
    public void SetFocus_Character(eCharacterNum focusCharacter)
    {
        GameObject tergetCharacter
            = GameObject.Find(sCharacterNum[(int)focusCharacter]);
        tergetCharacter.GetComponent<Image>().color
            = new Color(f_focusColor[0] / f_maxColor
            , f_focusColor[1] / f_maxColor
            , f_focusColor[2] / f_maxColor);
    }

    //キャラクター状態表示にする
    public void ShowHide_Button(bool IsShow)
    {
        foreach (string sTergetName in sCharacterNum)
        {
            GameObject terget_status
                = GameObject.Find(sTergetName);
           //画像
           //ステータス
           //HP、MPゲージ
        }
    }
}