using System.IO;
using UnityEngine;

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

//キャラクターと敵の抽象クラス
public class BattleActor:MonoBehaviour{
    protected BattlerObject[] mBattlerObject;

}

public class CharacterDataSingleton:BattleActor{

    private CharacterObject[] mCharacterObject;

    ClassesDataSingleton mClassesDataSingleton;
    SkillDataSingleton mSkillDataSingleton;


    void Awake()
    {
        FileRead_CharacterData();
        mClassesDataSingleton 
        = gameObject.GetComponent<ClassesDataSingleton>();
        mSkillDataSingleton
        = gameObject.GetComponent<SkillDataSingleton>();
    }

    //jsonファイルを読み取る
    private void FileRead_CharacterData() {

        string fileName = "Actors";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

       mCharacterObject = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);
    }

    public void SetBattleCharacterObject()
    {
        learningsObject[] lerningsObjects = null;

        mBattlerObject = new BattlerObject[mCharacterObject.Length];
        
        for (int j=1; j< mCharacterObject.Length; j++) {
            mBattlerObject[j] = new BattlerObject();
            mBattlerObject[j].battleproperty = Resources.Load<BattleProperty> ("data/Character"+j.ToString());

            lerningsObjects = mClassesDataSingleton.getLearningObject(1);

            mBattlerObject[j].skillIndex
                = new int[lerningsObjects.Length];

            
            int i = 0;
            
            foreach (learningsObject learningObject in lerningsObjects) {
                mBattlerObject[j].skillIndex[i]
                    = learningObject.skillId;
                i++;
            }
        }
    }

    public int[] GetSkillIndex(int characterId)
    {
        int[] result = null;

        if (characterId>0 && characterId<=mBattlerObject.Length) {
            result = mBattlerObject[characterId].skillIndex;
        }

        return result;
    }

    public eTergetScope GetSkillScope(int skillId)
    {   
        return (eTergetScope)mSkillDataSingleton.GetSkillScope(skillId);
    }

    public int CharaSpeed(int characterNum)
    {
        return mBattlerObject[characterNum].battleproperty.Sp;
    }
}
