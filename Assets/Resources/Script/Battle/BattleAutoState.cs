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
using System.Collections.Generic;

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
    //行動パターン
    BattlerAction mButtlerAction = null;

    ActorObject actor = null;

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

        //行動パターン定義
        mButtlerAction = new BattlerAction();

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
                actor = mBattleStateDataSinglton.ActorObject;

                //アクターの行動
                mButtlerAction.Role[0](actor);
                
                parentObject = GameObject.Find("Panel_CharacterStatus");
                CharacterStatusController mCharacterStatusController 
                = parentObject.GetComponent<CharacterStatusController>();

                parentObject = GameObject.Find("Panel_Enemy");
                EnemyGraphicController mEnemyGraphicController 
                = parentObject.GetComponent<EnemyGraphicController>();

                //行動対象キャラクターにフォーカス移動
                if(actor.belong == eActorScope.Friend){
                    mCharacterStatusController.SetFocus_Character(actor.actorNum);
                }

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
                //親を指定し、数値を作成する
                prefab_Damage = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
                prefab_Damage.transform.SetParent(parentObject.transform);
                Vector3 posDamage = new Vector3(actor.tergetPos[actor.tergetNum].x,actor.tergetPos[actor.tergetNum].y);
                prefab_Damage.transform.position = posDamage;

                int param = mButtlerAction.getTergetParam(actor);
                prefab_Damage.GetComponentInChildren<Text>().color
                    = new Color(1.0f,0.125f,0.125f,1.0f);
                prefab_Damage.GetComponentInChildren<Text>().text
                    = param.ToString();
                
                StartCoroutine(DelayMethod(1.0f, () =>
                {
                    mButtlerAction.gainTergetHP(actor,-param);
                    if(actor.terget == eTergetScope.forOne || actor.terget == eTergetScope.forAll){
                        mEnemyGraphicController.Shake(actor.tergetNum);
                    }
                    else if(actor.terget == eTergetScope.forFriend || actor.terget == eTergetScope.forFriendAll){
                        mCharacterStatusController.Shake(actor.tergetNum);
                    }
                }));
                break;
            case eAutoStatus.eAutoStatus_Act:
                //攻撃エフェクト～ダメージ消去まで表示されたら終了ステータスへ
                temp = GameObject.FindGameObjectWithTag("AutoState_Act");

                if (temp == null){
                    
                     
                    GameObject dummy = new GameObject();
                    dummy.tag = "AutoState_End";
                    //適当な時間待っている。調整は必要？そこまで必要ではない　Slip 2017/08/05
                    Destroy(dummy, 0.1f);
                    
                    mAutoStatus = eAutoStatus.eAutoStatus_End;
               }

                break;
            case eAutoStatus.eAutoStatus_End:
                //ウェイト時間経過後に次の行動へ移行する
                 
                temp = GameObject.FindGameObjectWithTag("AutoState_End");

                if (temp == null)
                {
                    //行動後のオブジェクトを消去する
                    mBattleStateDataSinglton.RemoveTopActor();

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

    //遅らせる
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
