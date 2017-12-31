/*
*方針：ツクールMVのjsonファイル形式と互換性があるように作成する
*/

using System.IO;
using UnityEngine;

public class TroopsObject
{
    public int id = 0;
    public membersObject[] members;
    public string name;
    public pagesObject[] pages;
}

public class membersObject
{
    public int enemyId;
    public int x;
    public int y;
    public bool hidden;
}

public class pagesObject
{
    public conditionsObject conditions;
    public listObject[] list;
    public int span;
}

public class conditionsObject
{
    public int actorHp;
    public int actorId;
    public bool actorValid;
    public int enemyHp;
    public int enemyIndex;
    public bool enemyValid;
    public int switchId;
    public bool switchValid;
    public int turnA;
    public int turnB;
    public bool turnEnding;
    public bool turnValid;
}

public class listObject
{
    public int code;
    public int indent;
    public int[] parameters;
}

public class TroopsDataSingleton : MonoBehaviour
{
    //オブジェクト定義
    private TroopsObject[] mTroopsObject;

    void Start(){
        FileRead();
    }

    private void FileRead()
    {

        string fileName = "Troops";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

        string temp_jsonText = jsonText.Replace("params", "_params");
        mTroopsObject = LitJson.JsonMapper.ToObject<TroopsObject[]>(temp_jsonText);

    }

}

