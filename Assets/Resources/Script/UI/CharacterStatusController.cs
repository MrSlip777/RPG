﻿//http://gametukurikata.com/program/uifocus

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterStatusController : MonoBehaviour
{
    private CharacterDataSingleton mCharacterDataSingleton;

    //各キャラHP
    private GameObject[] HP;

    private CharacterStatusController()
    {

    }

    //キャラクターステータスの番号
    public enum eCharacterNum
    {
        eCharacter1 = 1,
        eCharacter2 = 2,
        eCharacter3 = 3,
        eCharacter4 = 4,
    };
    //役割の設定値
    private static int mSelectingCharacter = (int)eCharacterNum.eCharacter1;

    //UIの設定値
    private float f_maxColor = 255;
    private float[] f_focusColor = {125,233,255};
    private float[] f_DefaultColor = { 255, 255, 255 };

    GameObject[] prefab_CharacterStatus;


    void Awake(){
        GameObject parentObject = GameObject.Find("DataSingleton");
        mCharacterDataSingleton = parentObject.GetComponent<CharacterDataSingleton>();
    }

    void Update(){
        for (int i=1; i<=4; i++) {
            HP[i].GetComponent<Slider>().value
             = mCharacterDataSingleton.HPRate(i);
        }
    }

    public void Shake(int TergetNum){
        iTween.ShakePosition(prefab_CharacterStatus[TergetNum]
        ,iTween.Hash("x",30.0f,"y",30.0f,"time",0.5f));
    }

    public void MakeUI()
    {
        //プレハブ生成（0番目はnullとする）
        prefab_CharacterStatus = new GameObject[5];
        HP = new GameObject[5];

        //親オブジェクトの指定
        GameObject parentObject = GameObject.Find("Panel_CharacterStatus");

        //プレハブ指定
        for (int i=1; i<=4; i++) {
            prefab_CharacterStatus[i] = Instantiate(
                (GameObject)Resources.Load("Prefabs/CharacterStatus"));

            prefab_CharacterStatus[i].transform.SetParent(parentObject.transform, false);

            Vector3 temp = prefab_CharacterStatus[i].transform.position;
            prefab_CharacterStatus[i].transform.position = new Vector3(temp.x+300*i,temp.y,temp.z);

            //キャラ名
            GameObject Name = prefab_CharacterStatus[i].transform.Find("Name").gameObject;
            Name.GetComponent<Text>().text = "キャラ";

            //画像表示


            //HP、MP表示
            HP[i] = prefab_CharacterStatus[i].transform.Find("HPGauge").gameObject;
            HP[i].GetComponent<Slider>().maxValue = 100;
            HP[i].GetComponent<Slider>().minValue = 0;
            HP[i].GetComponent<Slider>().value = mCharacterDataSingleton.HPRate(i);

        }
    }

    //キャラクター状態の初期設定
    public void InitialSelectCharacter()
    {
        SetFocus_Character(mSelectingCharacter);
    }

    //キャラクターのフォーカス指定
    public void SetFocus_Character(int SelectingCharacter)
    {
        GameObject[] tergets = GameObject.FindGameObjectsWithTag("CharacterStatus");

        //フォーカスなし状態
        SetNoFocus();

        if (SelectingCharacter>=1 && SelectingCharacter <= 4) {
            //フォーカス色の設定
            tergets[SelectingCharacter - 1].GetComponent<Image>().color
                = new Color(f_focusColor[0] / f_maxColor
                , f_focusColor[1] / f_maxColor
                , f_focusColor[2] / f_maxColor);
        }
    }

    //フォーカスしない状態（自動行動状態への遷移中）
    public void SetNoFocus()
    {
        GameObject[] tergets = GameObject.FindGameObjectsWithTag("CharacterStatus");

        //色の初期化
        foreach (GameObject terget in tergets)
        {
            if (terget != null)
            {
                terget.GetComponent<Image>().color
                    = new Color(f_DefaultColor[0] / f_maxColor
                    , f_DefaultColor[1] / f_maxColor
                    , f_DefaultColor[2] / f_maxColor);
            }

        }
    }
}