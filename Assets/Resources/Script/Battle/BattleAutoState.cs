/*
 *戦闘の自動状態
 * 
 *http://qiita.com/toRisouP/items/e402b15b36a8f9097ee9
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BattleAutoState : MonoBehaviour {

    //自動行動の状態
    public enum eAutoStatus
    {
        eAutoStatus_Start = 0,
        eAutoStatus_Act = 1,
        eAutoStatus_End = 2,

    };

    //自動行動の状態
    private static eAutoStatus mAutoStatus;

    //戦闘画面状態データ
    BattleStateDataSinglton mBattleStateDataSinglton = null;
    //スキルデータ
    SkillDataSingleton mSkillDataSingleton = null;
    //エフェクト
    EffectManager mEffectManager = null;

    void Awake(){
        GameObject parentObj = null;
        mAutoStatus = eAutoStatus.eAutoStatus_Start;
        parentObj = GameObject.Find("DataSingleton");
        mBattleStateDataSinglton 
        = parentObj.GetComponent<BattleStateDataSinglton>();
        mSkillDataSingleton 
        = parentObj.GetComponent<SkillDataSingleton>();

        //エフェクト管理
        parentObj = GameObject.Find("Panel_Effect");
        mEffectManager = parentObj.GetComponent<EffectManager>();
    }

    //ターン開始時の初期化
    public void TurnStart()
    {
        mAutoStatus = eAutoStatus.eAutoStatus_Start;

        //行動順を設定
        mBattleStateDataSinglton.SortActorSpeed();
    }

    // Use this for initialization
    //エフェクトを自動で表示
    public void _Update() {

        //一時的なゲームオブジェクト
        GameObject temp = null;
        GameObject parentObject = null;

        switch (mAutoStatus)
        {
            case eAutoStatus.eAutoStatus_Start:
                //状態フラグ変更
                mAutoStatus = eAutoStatus.eAutoStatus_Act;

                //行動者オブジェクト取得
                ActorObject actor = mBattleStateDataSinglton.ActorObject;

                parentObject = GameObject.Find("Panel_CharacterStatus");
                CharacterStatusController mCharacterStatusController 
                = parentObject.GetComponent<CharacterStatusController>();

                //行動対象キャラクターにフォーカス移動
                mCharacterStatusController.SetFocus_Character(actor.actorNum);

                //ローカル変数定義
                parentObject = GameObject.Find("Canvas");

                //技名
                GameObject prefab_ActionText = null;
                //親を指定し、技名ウインドウを作成する
                prefab_ActionText = Instantiate((GameObject)Resources.Load("Prefabs/Action_Text"));
                prefab_ActionText.transform.SetParent(parentObject.transform);
                prefab_ActionText.GetComponent<DestoroyAtTime>().delayTime = 0.0f;
                prefab_ActionText.GetComponentInChildren<Text>().text
                    = mSkillDataSingleton.GetSkillName(actor.skillID);

                //エフェクト表示
                mEffectManager.SetEffect(actor.tergetPos[actor.tergetNum]);

                //ダメージを表示させる
                GameObject prefab_Damage = null;
                //親を指定し、技名ウインドウを作成する
                prefab_Damage = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
                prefab_Damage.transform.SetParent(parentObject.transform);
                Vector3 posDamage = new Vector3(actor.tergetPos[actor.tergetNum].x,400);
                prefab_Damage.transform.position = posDamage;

                break;
            case eAutoStatus.eAutoStatus_Act:
                //攻撃エフェクト～ダメージ消去まで表示されたら終了ステータスへ
                temp = GameObject.FindGameObjectWithTag("AutoState_Act");

                if (temp == null){
                    GameObject dummy = new GameObject();
                    dummy.tag = "AutoState_End";
                    //適当な時間待っている。調整は必要？そこまで必要ではない　Slip 2017/08/05
                    Destroy(dummy, 0.1f);

                    //行動後のオブジェクトを消去する
                    mBattleStateDataSinglton.RemoveTopActor();

                    mAutoStatus = eAutoStatus.eAutoStatus_End;
                }

                break;
            case eAutoStatus.eAutoStatus_End:
                //ウェイト時間経過後に次の行動へ移行する
                temp = GameObject.FindGameObjectWithTag("AutoState_End");

                if (temp == null)
                {

                    if (mBattleStateDataSinglton.ActorObject != null) {
                        mAutoStatus = eAutoStatus.eAutoStatus_Start;

                    }
                    else
                    {
                        mBattleStateDataSinglton.BattleStateMode
                            = BattleStateDataSinglton.eBattleState.eBattleState_AutoEnd;
                            
                    }
                }

                break;
            default:
                break;
        }
    }

}
