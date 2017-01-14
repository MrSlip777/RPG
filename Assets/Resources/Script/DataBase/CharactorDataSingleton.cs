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

public class SkillsObject
{
    public int id = 0;
    public int animationId;
    public damageObject damage;
    public string description;
    public effectsObject[] effects;
    public int hitType;
    public int iconIndex;
    public string message1;
    public string message2;
    public int mpCost;
    public string name;
    public string note;
    public int occasion;
    public int repeat;
    public int requireWtypeId1;
    public int requireWtypeId2;
    public int scope;
    public int speed;
    public int stypeId;
    public int successRate;
    public int tpCost;
    public int tpGain;
}

public class damageObject
{
    public bool critical;
    public int elementId;
    public string formula;
    public int type;
    public int variance;


}

public class effectsObject
{
    public int code;
    public int dataId;
    public int value1;
    public int value2;
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
    private SkillsObject[] mSkillsObject;

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
        GetCharacterData();
        GetClassesData();
        GetSkillData();
    }

    private void GetCharacterData() {

        string folderpath = Application.dataPath + "/Resources/data/";
        string filePath = folderpath + "Actors.json";

        if (!File.Exists(filePath)) return;

        string jsonText = File.ReadAllText(filePath);
        mCharacterObject = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);
    }

    public void SetBattleCharacterObject()
    {
        mBattleCharacterObject = new BattleCharacterObject[mCharacterObject.Length];
        mBattleCharacterObject[1] = new BattleCharacterObject();

        mBattleCharacterObject[1].skillIndex = new int[3];
        mBattleCharacterObject[1].skillIndex[0] = 0;
        mBattleCharacterObject[1].skillIndex[1] = 5;
        mBattleCharacterObject[1].skillIndex[2] = 6;
    }

    private void GetClassesData()
    {

        string folderpath = Application.dataPath + "/Resources/data/";
        string filePath = folderpath + "Classes.json";

        if (!File.Exists(filePath)) return;

        string jsonText = File.ReadAllText(filePath);
        string temp_jsonText = jsonText.Replace("params","_params");
        mClassesObject = LitJson.JsonMapper.ToObject<ClassesObject[]>(temp_jsonText);

    }

    private void GetSkillData()
    {

        string folderpath = Application.dataPath + "/Resources/data/";
        string filePath = folderpath + "Skills.json";

        if (!File.Exists(filePath)) return;

        string jsonText = File.ReadAllText(filePath);
        mSkillsObject = LitJson.JsonMapper.ToObject<SkillsObject[]>(jsonText);

    }

    public string[] GetSkillName(int characterId)
    {
        int[] skillindex = mBattleCharacterObject[characterId].skillIndex;

        string[] skillnames = new string[skillindex.Length];

        for (int i=0; i< skillindex.Length; i++) {
            if (mSkillsObject[i] != null)
            {
                skillnames[i] = mSkillsObject[skillindex[i]].name;
            }
        }
        

        return skillnames;
    }
}
