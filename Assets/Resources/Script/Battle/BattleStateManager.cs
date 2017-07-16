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
    BattleSelectState mState = BattleSelectState.Instance;

    //下は後で統合する　Slip 2017/07/11
    BattleAutoState mState1 = BattleAutoState.Instance;


    EnemyGraphicController mEnemyGraphicController
        = new EnemyGraphicController();


    Game_Action action = new Game_Action();



    // Use this for initialization
    void Start () {


        mEnemyGraphicController.ShowEnemy();
        mState._Start();
        
    }

    // Update is called once per frame
    void Update () {

        if (BattleSelectState.eUIStatus.eUIStatus_Auto != mState.getUIStatus)
        {
            mState._Update();
        }
        else
        {
            mState1.StartAction();
        }
    }
}
