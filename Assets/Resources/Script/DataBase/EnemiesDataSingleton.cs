/*
*方針：ツクールMVのjsonファイル形式と互換性があるように作成する
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesObject
{
    public int id = 0;
    public actionsObject[] actions;
    public int battleHue;
    public string battlerName;
    public dropItemsObject[] dropItems;
    public int exp = 0;
    public traitsObject[] traits;
    public int gold = 0;
    public string name;
    public string note;
    public int[] _params;
}

public class actionsObject
{
    public int conditionParam1 = 0;
    public int conditionParam2 = 0;
    public int conditionType = 0;
    public int rating = 0;
}

public class dropItemsObject
{
    public int dataId = 0;
    public int denominator = 0;
    public int kind = 0;
}

public class EnemiesDataSingleton : MonoBehaviour {

    private static EnemiesDataSingleton mInstance;

    //オブジェクト定義
    private EnemiesObject[] mEnemiesObject;

    // 唯一のインスタンスを取得します。
    public static EnemiesDataSingleton Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new EnemiesDataSingleton();
            }

            return mInstance;
        }

    }

    //シングルトン実装
    private EnemiesDataSingleton()
    {
        FileRead();
    }

    private void FileRead()
    {

        string fileName = "Enemies";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

        string temp_jsonText = jsonText.Replace("params", "_params");
        mEnemiesObject = LitJson.JsonMapper.ToObject<EnemiesObject[]>(temp_jsonText);

    }
}
