//http://gametukurikata.com/program/uifocus

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatusController : MonoBehaviour
{

    //各ボタンの名前
    private string[] sCharacterNum = {
        "Character1" ,
        "Character2",
        "Character3",
        "Character4"
    };

    //キャラクター状態の初期設定
    public void InitialSelectCharacter()
    {
        GameObject initial_character
            = GameObject.Find(sCharacterNum[0]);
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

    //キャラクター状態をアクティブ状態にする
    public void ActiveCharacter()
    {
        GameObject terget_status
                = GameObject.Find("Character1");

        Animator _anim = terget_status.GetComponent<Animator>();
        _anim.Play("Character1", 0, 0.0f);
    }

}