﻿/*
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

    //敵の数
    private static int mEnemiesNum = 1;

    void Start()
    {
        FileRead();

        //暫定的に敵の数を2とする　要修正　Slip 2017/08/05
        mEnemiesNum = 2;
        string[] EnemyNames = {"","Enemy1","Enemy2"};
        mBattlerObject = new BattlerObject[EnemyNames.Length];
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

        //string temp_jsonText = jsonText.Replace("params", "_params");
        //mEnemiesObject = LitJson.JsonMapper.ToObject<EnemiesObject[]>(temp_jsonText);

    }

    //敵の数を取得する
    public int EnemiesNum
    {
        get { return mEnemiesNum; }
    }

    //自動行動用のデータ
    public ActorObject getAutoActorData(int number)
    {
        ActorObject actorObject = new ActorObject();
        Vector3[] friendPositions = GetPosition("CharacterStatus");

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

    //バトラーを設定する
    public void SetBattlerEnemyObject(string BattlerName,int Number)
    {
        //learningsObject[] lerningsObjects = null;
        mBattlerObject[Number] = new BattlerObject();
        mBattlerObject[Number].battleproperty = Resources.Load<BattleProperty> ("data/"+ BattlerName);

        mBattlerObject[Number].skillIndex
            = new int[8];

        int i = 0;

        foreach (int index in mBattlerObject[Number].skillIndex) {
            mBattlerObject[Number].skillIndex[index]
                = 10;

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
        Update_Parameter(ref mBattlerObject[Number]);
        Initialize_BattleParameter(ref mBattlerObject[Number]);
    }
}
