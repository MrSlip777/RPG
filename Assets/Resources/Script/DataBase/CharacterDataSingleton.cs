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

public class ClassesObject
{

    public int id = 0;
    public int[] expParams;
    public traitsObject[] traits;
    public learningsObject[] learnings;
    public string name;
    public string note;
    public int[][] _params;
}

public class traitsObject
{
    public int code;
    public int dataId;
    public double value;
}

public class learningsObject
{
    public int level;
    public string note;
    public int skillId;
}

public class BattleCharacterObject
{
    public int[] skillIndex;

}

    public class CharacterDataSingleton:MonoBehaviour{

    //インスタンス定義
    private static CharacterDataSingleton mInstance;

    private CharacterObject[] mCharacterObject;
    private ClassesObject[] mClassesObject;
    private BattleCharacterObject[] mBattleCharacterObject;

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
        FileRead_ClassesData();
    }

    private void FileRead_CharacterData() {

        string fileName = "Actors";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

       mCharacterObject = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);
    }

    public void SetBattleCharacterObject()
    {
        mBattleCharacterObject = new BattleCharacterObject[mCharacterObject.Length];
        mBattleCharacterObject[1] = new BattleCharacterObject();

        mBattleCharacterObject[1].skillIndex = new int[3];
        mBattleCharacterObject[1].skillIndex[0] = 0;
        mBattleCharacterObject[1].skillIndex[1] = 9;
        mBattleCharacterObject[1].skillIndex[2] = 10;
    }

    private void FileRead_ClassesData()
    {

        string fileName = "Classes";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

        string temp_jsonText = jsonText.Replace("params","_params");
        mClassesObject = LitJson.JsonMapper.ToObject<ClassesObject[]>(temp_jsonText);

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

    //行動選択画面の選択状態

    //行動者のターゲット状態
}
