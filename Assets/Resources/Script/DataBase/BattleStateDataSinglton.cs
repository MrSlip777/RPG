/*
 * 戦闘状態の管理ステータス
 * 自動戦闘状態での行動者ステータス
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ターゲット
public enum eTergetScope
{
    Hide = -1,
    forOne = 1,
    forAll = 2,
    forRandom = 3,
    forFriend = 7,
    forFriendAll = 8,
};

//行動者
public enum eActorScope
{
    Enemy = 1,
    Friend = 7,
};


//行動者のオブジェクト　敵味方関係ない
public class ActorObject
{
    public int speed;
    public int skillID;

    public eActorScope belong;
    public int actorNum;
    public eTergetScope terget;
    public int tergetNum;
    public Vector3[] tergetPos;   //対象者の位置

}

public class BattleStateDataSinglton: MonoBehaviour
{
    //戦闘画面の状態
    public enum eBattleState
    {
        eBattleState_Select = 0,
        eBattleState_SelectEnd = 1,
        eBattleState_Auto = 2,
        eBattleState_AutoEnd = 3,
        eBattleState_BattleEnd = 4,
    };

    //自動行動の状態
    private static eBattleState mBattleState;


    private static List<ActorObject> mActorObjects;

    public eBattleState BattleStateMode
    {
        set { mBattleState = value; }
        get { return mBattleState; }
    }

    //
    //使用者
    //スキル名
    //・プレハブファイル名
    //対象者（対象者HP0の場合は変更）
    //・ダメージ

    public ActorObject ActorObject
    {
        set {
            if (mActorObjects == null)
            {
                mActorObjects = new List<ActorObject>();
            }
            mActorObjects.Add(value);
        }
        get
        {
            ActorObject result = null;
            if (mActorObjects.Count != 0)
            {
                result = mActorObjects[0];
            }

            return result;
        }
    }

    //速さで行動者を降順にする
    public void SortActorSpeed()
    {
        mActorObjects.Sort((a,b)=>b.speed-a.speed);
    }

    //先頭のActorObjectを消去する
    public void RemoveTopActor()
    {
        mActorObjects.Remove(mActorObjects[0]);
    }
}
