/*
 * 戦闘状態の管理ステータス
 * 自動戦闘状態での行動者ステータス
 * 
 * 
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateDataSinglton{

    //シングルトン実装
    private static BattleStateDataSinglton mInstance;


    // 唯一のインスタンスを取得します。
    public static BattleStateDataSinglton Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new BattleStateDataSinglton();
            }

            return mInstance;
        }

    }

    private BattleStateDataSinglton()
    {

    }

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

    public eBattleState BattleStateMode
    {
        set { mBattleState = value; }
        get { return mBattleState; }
    }

    //
    //使用者
    //スキル名
    //プレハブファイル名
    //対象者（対象者HP0の場合は変更）
    //ダメージ

}
