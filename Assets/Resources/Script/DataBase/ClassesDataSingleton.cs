/*
*方針：ツクールMVのjsonファイル形式と互換性があるように作成する
*/

using System.IO;
using UnityEngine;

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

public class ClassesDataSingleton : MonoBehaviour {

    //オブジェクト定義
    private ClassesObject[] mClassesObject;

    void Start()
    {
        FileRead_ClassesData();
    }

    private void FileRead_ClassesData()
    {

        string fileName = "Classes";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

        string temp_jsonText = jsonText.Replace("params", "_params");
        mClassesObject = LitJson.JsonMapper.ToObject<ClassesObject[]>(temp_jsonText);

    }

    //職業毎のスキル一覧取得
    public learningsObject[] getLearningObject(int id)
    {
        return mClassesObject[id].learnings;
    }

}
