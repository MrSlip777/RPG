using System.IO;
using UnityEngine;

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

public class SkillDataSingleton : MonoBehaviour {

    //インスタンス定義
    private static SkillDataSingleton mInstance;

    private SkillsObject[] mSkillsObject;

    // 唯一のインスタンスを取得します。
    public static SkillDataSingleton Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new SkillDataSingleton();
            }

            return mInstance;
        }

    }

    //シングルトン実装
    private SkillDataSingleton()
    {
        FileRead_SkillData();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FileRead_SkillData()
    {

        string folderpath = Application.dataPath + "/Resources/data/";
        string filePath = folderpath + "Skills.json";

        if (!File.Exists(filePath)) return;

        string jsonText = File.ReadAllText(filePath);
        mSkillsObject = LitJson.JsonMapper.ToObject<SkillsObject[]>(jsonText);

    }

    //パーティキャラクターの所持スキル名取得
    public string GetSkillName(int Id)
    {
        string result = null;

        if (Id > 0 && Id<=mSkillsObject.Length)
        {
            result = mSkillsObject[Id].name;
        }

        return result;
    }

    //パーティキャラクターの所持スキル名の説明文を取得
    public string GetSkillDescription(int Id)
    {
        string result = null;

        if (Id > 0 && Id <= mSkillsObject.Length)
        {
            result = mSkillsObject[Id].description;
        }

        return result;
    }
}
