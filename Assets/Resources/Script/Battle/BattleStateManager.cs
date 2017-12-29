/*
*参考URL http://fantastic-works.com/archives/148
*
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour
{

    //インスタンス定義
    private static BattleStateManager mInstance;

    // 唯一のインスタンスを取得します。
    public static BattleStateManager Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new BattleStateManager();
            }

            return mInstance;
        }

    }

    //シングルトン実装
    private BattleStateManager()
    {

    }

    //インスタンス定義
    BattleSelectState mState = null;

    //下は後で統合する　Slip 2017/07/11
    BattleAutoState mState1 = null;

    //戦闘画面の状態データ
    BattleStateDataSinglton mBattleStateDataSinglton = null; 

    // Use this for initialization
    void Start () {

        //インスタンス取得
        mState = BattleSelectState.Instance;
        mState1 = BattleAutoState.Instance;
        mBattleStateDataSinglton = BattleStateDataSinglton.Instance;

        mBattleStateDataSinglton.BattleStateMode
            = BattleStateDataSinglton.eBattleState.eBattleState_Select;

        mState._Start();
        
    }

    // Update is called once per frame
    void Update () {

        switch (mBattleStateDataSinglton.BattleStateMode)
        {
            case BattleStateDataSinglton.eBattleState.eBattleState_Select:
                mState._Update();
                break;
            case BattleStateDataSinglton.eBattleState.eBattleState_SelectEnd:
                mState1.TurnStart();
                mBattleStateDataSinglton.BattleStateMode
                    = BattleStateDataSinglton.eBattleState.eBattleState_Auto;
                break;
            case BattleStateDataSinglton.eBattleState.eBattleState_Auto:
                mState1._Update();
                break;
            case BattleStateDataSinglton.eBattleState.eBattleState_AutoEnd:
                mState.TurnStart();
                mBattleStateDataSinglton.BattleStateMode
                    = BattleStateDataSinglton.eBattleState.eBattleState_Select;
                break;
            default:
                break;
        }

    }
}
