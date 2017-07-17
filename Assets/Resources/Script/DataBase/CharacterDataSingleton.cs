/*
*方針：ツクールMVのjsonファイル形式と互換性があるように作成する
*/

using System.IO;
using UnityEngine;

/*
*Actor.jsonに合わせる
*ツクールMV ver1.3.4基準
*/

public class CharacterObject{

    public int id = 0;
    public string battterName = "";
    public int characterIndex = 0;
    public string characterName = "";
    public int classId = 0;
    public int[] equips;
    public int faceIndex = 0;
    public string faceName = "";
    public int[] traits;
    public int initialLevel = 0;
    public int maxLevel = 0;
    public string name = "";
    public string nickname = "";
    public string note = "";
    public string profile = "";

}

public class BattleCharacterObject
{
    public int[] skillIndex;

}

public class CharacterDataSingleton:MonoBehaviour{

    //インスタンス定義
    private static CharacterDataSingleton mInstance;

    private CharacterObject[] mCharacterObject;
    private BattleCharacterObject[] mBattleCharacterObject;

    //行動キャラ
    private static int mSelectingCharacterNum = 1;

    // 唯一のインスタンスを取得します。
    public static CharacterDataSingleton Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new CharacterDataSingleton();
            }

            return mInstance;
        }

    }

    //シングルトン実装
    private CharacterDataSingleton()
    {
        FileRead_CharacterData();
    }

    //jsonデータ読み込み
    private void FileRead_CharacterData() {

        string fileName = "Actors";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

       mCharacterObject = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);
    }

    //オブジェクトに各種データを設定する
    public void SetBattleCharacterObject()
    {
        mBattleCharacterObject = new BattleCharacterObject[mCharacterObject.Length];
        mBattleCharacterObject[1] = new BattleCharacterObject();

        mBattleCharacterObject[1].skillIndex = new int[3];
        mBattleCharacterObject[1].skillIndex[0] = 0;
        mBattleCharacterObject[1].skillIndex[1] = 9;
        mBattleCharacterObject[1].skillIndex[2] = 10;
    }

    //キャラクターの所持スキルを渡す
    public int[] GetSkillIndex(int characterId)
    {
        int[] result = null;

        if (characterId>0 && characterId<=mBattleCharacterObject.Length) {
            result = mBattleCharacterObject[characterId].skillIndex;
        }

        return result;
    }

    //行動選択画面の選択状態の取得
    public int GetSelectingCharacter()
    {
        return mSelectingCharacterNum;
    }

    //次の選択状態キャラ変更
    public int NextSelectingCharacter()
    {
        //自キャラのHP判定後にインクリメントするかを判断する
        //実装中　Slip
        mSelectingCharacterNum++;

        return mSelectingCharacterNum;
    }

    //前の選択状態キャラ変更
    public int BeforeSelectingCharacter()
    {
        //自キャラのHP判定後にデクリメントするかを判断する
        //実装中　Slip
        mSelectingCharacterNum--;

        return mSelectingCharacterNum;
    }

    //行動選択者の初期化
    public int TurnStartCharacter()
    {
        //自キャラのHP判定後に値を適宜変更する
        //実装中　Slip
        mSelectingCharacterNum = 1;

        return mSelectingCharacterNum;
    }

    //行動者のターゲット状態

}
