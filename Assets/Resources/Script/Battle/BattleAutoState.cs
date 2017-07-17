/*
 *戦闘の自動状態
 * 
 *http://qiita.com/toRisouP/items/e402b15b36a8f9097ee9
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleAutoState : MonoBehaviour {

    //シングルトン実装
    private static BattleAutoState mInstance;


    // 唯一のインスタンスを取得します。
    public static BattleAutoState Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new BattleAutoState();
            }

            return mInstance;
        }

    }

    //自動行動の状態
    public enum eAutoStatus
    {
        eAutoStatus_Start = 0,
        eAutoStatus_Act = 1,
        eAutoStatus_End = 2,

    };

    //自動行動の状態
    private static eAutoStatus mAutoStatus;

    private BattleAutoState()
    {
        mAutoStatus = eAutoStatus.eAutoStatus_Start;
    }

    //各インスタンス定義
        //キャラクターステータス表示ウインドウ
    CharacterStatusController mCharacterStatusController
        = CharacterStatusController.Instance;

        //戦闘画面状態データ
    BattleStateDataSinglton mBattleStateDataSinglton = BattleStateDataSinglton.Instance;

    //ターン開始時の初期化
    public void TurnStart()
    {
        mAutoStatus = eAutoStatus.eAutoStatus_Start;
    }

    // Use this for initialization
    //エフェクトを自動で表示
    public void _Update() {

        //一時的なゲームオブジェクト
        GameObject temp = null;

        switch (mAutoStatus)
        {
            case eAutoStatus.eAutoStatus_Start:
                mAutoStatus = eAutoStatus.eAutoStatus_Act;

                //行動対象キャラクターにフォーカス移動
                mCharacterStatusController.SetFocus_Character(1);

                
                //ローカル変数定義
                GameObject parentObject = null;
                parentObject = GameObject.Find("Canvas");

                //技名
                GameObject prefab_ActionText = null;
                //親を指定し、技名ウインドウを作成する
                prefab_ActionText = Instantiate((GameObject)Resources.Load("Prefabs/Action_Text"));
                prefab_ActionText.transform.SetParent(parentObject.transform);
                prefab_ActionText.GetComponent<DestoroyAtTime>().delayTime = 0.0f;


                //エフェクト表示
                EffectManager mEffectManager = new EffectManager();
                mEffectManager.MakePrefab("FT_Infinity_lite/_Prefabs/Buff/Discharge_Lightning");

                //ダメージを表示させる
                GameObject prefab_Damage = null;
                //親を指定し、技名ウインドウを作成する
                prefab_Damage = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
                prefab_Damage.transform.SetParent(parentObject.transform);

                break;
            case eAutoStatus.eAutoStatus_Act:
                //攻撃エフェクト～ダメージ消去まで表示されたら終了ステータスへ
                temp = GameObject.FindGameObjectWithTag("AutoState_Act");

                if (temp == null){
                    GameObject dummy = new GameObject();
                    dummy.tag = "AutoState_End";
                    Destroy(dummy, 5.0f);

                    mAutoStatus = eAutoStatus.eAutoStatus_End;
                }

                break;
            case eAutoStatus.eAutoStatus_End:
                //ウェイト時間経過後に次の行動へ移行する
                temp = GameObject.FindGameObjectWithTag("AutoState_End");

                if (temp == null)
                {
                    //mAutoStatus = eAutoStatus.eAutoStatus_Start;
                    mBattleStateDataSinglton.BattleStateMode
                        = BattleStateDataSinglton.eBattleState.eBattleState_AutoEnd;
                }

                break;
            default:
                break;
        }
    }

}
