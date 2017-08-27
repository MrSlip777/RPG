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

    //敵の数
    private static int mEnemiesNum = 1;

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

        //暫定的に敵の数を2とする　要修正　Slip 2017/08/05
        mEnemiesNum = 2;
    }

    private void FileRead()
    {

        string fileName = "Enemies";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

        string temp_jsonText = jsonText.Replace("params", "_params");
        mEnemiesObject = LitJson.JsonMapper.ToObject<EnemiesObject[]>(temp_jsonText);

    }

    //敵の数を取得する
    public int EnemiesNum
    {
        get { return mEnemiesNum; }
    }

    //自動行動用のデータ
    public ActorObject getAutoActorData()
    {
        ActorObject actorObject = new ActorObject();
        Vector3[] friendPositions = GetPosition("CharacterStatus");

        actorObject.actorNum = -1;
        actorObject.speed = 100;
        actorObject.actor = eActorScope.Enemy;

        //保持している行動者のスキルIDを渡す（キャンセル操作に注意）
        actorObject.skillID = 12;

        //ターゲットの渡す
        actorObject.terget = eTergetScope.forOne;
        actorObject.tergetNum = 2;
        actorObject.tergetPos = friendPositions;

        return actorObject;
    }

    /// <summary>
    /// 対象の位置を取得する
    /// </summary>
    /// <param name="TagName"></param>
    /// <returns></returns>
    public Vector3[] GetPosition(string TagName)
    {
        int i = 0;
        Vector3[] result = null;

        GameObject[] tergets = GameObject.FindGameObjectsWithTag(TagName);

        if (tergets != null)
        {

            result = new Vector3[tergets.Length];

            foreach (GameObject terget in tergets)
            {
                result[i] = terget.transform.position;
                i++;
            }
        }

        return result;
    }
}
