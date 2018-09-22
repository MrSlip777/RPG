//http://gametukurikata.com/program/uifocus

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

    string[] s_Name = {"","ビーバー","キンシコウ","サーバル","アミメキリン"};
    string[] s_fileName = {"","beaber","kinshikou","serval","amimekirin"};

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

    public Vector3 getPosition(int ActorNum){
        return prefab_CharacterStatus[ActorNum].transform.position;
    }

    public void ChangeHPValue(int TergetNum){
        HP[TergetNum].GetComponent<SliderController>()
        .ValueTransition (mCharacterDataSingleton.HPRate(TergetNum));
    }

    public void Shake(int TergetNum){
        iTween.ShakePosition(prefab_CharacterStatus[TergetNum]
        ,iTween.Hash("x",0.05f,"y",0.05f,"time",0.5f));
    }

    public void ActionParam(int TergetNum,int param){
        //ローカル変数定義
        GameObject parentObject = GameObject.Find("Canvas");
        
        GameObject[] prefab_Damage = new GameObject[5];

        prefab_Damage[0]
        = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
        prefab_Damage[0].transform.SetParent(parentObject.transform,false);
        
        Vector3 posDamage
        = new Vector3(prefab_CharacterStatus[TergetNum].transform.position.x
        ,prefab_CharacterStatus[TergetNum].transform.position.y
        ,prefab_Damage[0].transform.position.z);
        
        prefab_Damage[0].transform.position = posDamage;

        prefab_Damage[0].GetComponentInChildren<Text>().color
            = new Color(1.0f,0.125f,0.125f,1.0f);
        prefab_Damage[0].GetComponentInChildren<Text>().text
            = param.ToString();
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

            float distance = Screen.width/4;
            float screenw = Screen.width;

            Vector3 temp = prefab_CharacterStatus[i].transform.localPosition;
            
            prefab_CharacterStatus[i].transform.localPosition
             = new Vector3((distance*(i-1)+distance/2-Screen.width/2.0f),temp.y,temp.z);
            //PositionChange(temp);
            //キャラ名
            GameObject Name = prefab_CharacterStatus[i].transform.Find("Name").gameObject;
            Name.GetComponent<Text>().text = s_Name[i];

            //画像表示
            GameObject CharactorImage = prefab_CharacterStatus[i].transform.Find("Image").gameObject;
            CharactorImage.GetComponent<Image>().sprite = Resources.Load<Sprite> ("Image/Character/"+s_fileName[i]);

            //HP、MP表示
            HP[i] = prefab_CharacterStatus[i].transform.Find("HPGauge").gameObject;
            HP[i].GetComponent<Slider>().maxValue = 100;
            HP[i].GetComponent<Slider>().minValue = 0;
            HP[i].GetComponent<Slider>().value = mCharacterDataSingleton.HPRate(i);

        }
    }

    private Vector3 PositionChange(Vector3 targetPos){
 		var pos = Vector2.zero;
		var uiCamera = Camera.main;
		var worldCamera = Camera.main;

        GameObject canvas = GameObject.Find("Canvas");
		var canvasRect = canvas.GetComponent<RectTransform> ();

		var screenPos = RectTransformUtility.WorldToScreenPoint (worldCamera, targetPos);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out pos);
		return pos;       
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