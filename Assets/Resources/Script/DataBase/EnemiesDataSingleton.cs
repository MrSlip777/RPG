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

public class EnemiesDataSingleton :BattleActor{

    private EnemiesObject[] mEnemiesObject;

    void Start()
    {
        FileRead();

        string[] EnemyNames = {"","Enemy1","Enemy2"};
        mBattlerObject = new List<BattlerObject>();
        BattlerObject battlerObj = null;
        mBattlerObject.Add(battlerObj);
        int i = 0;
        foreach(string EnemyName in EnemyNames){
            if(EnemyName != ""){
                SetBattlerEnemyObject(EnemyName,i);
            }
            i++;
        }
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
        get { return (mBattlerObject.Count-1); }
    }

    //自動行動用のデータ
    public ActorObject getAutoActorData(int number)
    {
        ActorObject actorObject = new ActorObject();
        SearchUI searchUI = new SearchUI();
        Vector3[] friendPositions = searchUI.GetPosition("CharacterStatus");

        actorObject.actorNum = number;
        actorObject.speed = 100;
        actorObject.belong = eActorScope.Enemy;

        //保持している行動者のスキルIDを渡す（キャンセル操作に注意）
        actorObject.skillID = 12;

        //ターゲットの渡す
        actorObject.terget = eTergetScope.forFriend;
        actorObject.tergetNum = 2;
        actorObject.tergetPos = friendPositions;

        return actorObject;
    }

    //バトラーを設定する
    public void SetBattlerEnemyObject(string BattlerName,int Number)
    {
        BattlerObject battlerObj = new BattlerObject();
        //mBattlerObject[Number].battleproperty = Resources.Load<BattleProperty> ("data/"+ BattlerName);
        battlerObj.battleproperty = new BattleProperty();

        battlerObj.battleproperty.HP_max
            = mEnemiesObject[1]._params[(int)e_StatusLabel.HP];
        battlerObj.battleproperty.MP_max
            = mEnemiesObject[1]._params[(int)e_StatusLabel.MP];
        battlerObj.battleproperty.At
            = mEnemiesObject[1]._params[(int)e_StatusLabel.At];
        battlerObj.battleproperty.Df
            = mEnemiesObject[1]._params[(int)e_StatusLabel.Df];
        battlerObj.battleproperty.Mg
            = mEnemiesObject[1]._params[(int)e_StatusLabel.Mg];
        battlerObj.battleproperty.Sp
            = mEnemiesObject[1]._params[(int)e_StatusLabel.Sp];
        battlerObj.battleproperty.Lc
            = mEnemiesObject[1]._params[(int)e_StatusLabel.Lc];
        
        battlerObj.battleproperty.HP = battlerObj.battleproperty.HP_max;
        battlerObj.battleproperty.MP = battlerObj.battleproperty.MP_max;
        battlerObj.skillIndex = new int[8];

        foreach (int index in battlerObj.skillIndex) {
            battlerObj.skillIndex[index] = 10;
        }        

        //ラーニングオブジェクトは敵と味方で値が異なるため修正の必要あり
        /*
        lerningsObjects = mClassesDataSingleton.getLearningObject(1);

        mBattleCharacterObject[Number].skillIndex
            = new int[lerningsObjects.Length];

        int i = 0;
        //�X�L���ݒ�
        foreach (learningsObject learningObject in lerningsObjects) {
            mBattleCharacterObject[Number].skillIndex[i]
                = learningObject.skillId;
            i++;
        }
        */
        Update_Parameter(ref battlerObj);
        Initialize_BattleParameter(ref battlerObj);

        mBattlerObject.Add(battlerObj);
    }
}
