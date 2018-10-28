/*
 *戦闘の自動状態
 * 
 *http://qiita.com/toRisouP/items/e402b15b36a8f9097ee9
 */

//http://megumisoft.hatenablog.com/entry/2016/01/27/235940
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.system;

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

    //エフェクト
    EffectManager mEffectManager = null;

    //UI、エフェクトの演出制御
    BattleSequenceDirection mBattleSequenceDirection = null;

    BattlerAction mBattlerAction = null;

    ActorObject actor = null;

    //UI
    CharacterStatusController mCharacterStatusController = null;
    EnemyGraphicController mEnemyGraphicController = null;

    GameObject m_Manager = null;

    void Awake(){
        GameObject parentObject = null;
        mAutoStatus = eAutoStatus.eAutoStatus_Start;
        parentObject = GameObject.Find("DataSingleton");
        mBattleStateDataSinglton 
        = parentObject.GetComponent<BattleStateDataSinglton>();

        //エフェクト管理
        parentObject = GameObject.Find("Panel_Effect");
        mEffectManager = parentObject.GetComponent<EffectManager>();

        //UI、エフェクトの演出制御
        parentObject = GameObject.Find("Canvas");
        mBattleSequenceDirection = parentObject.GetComponent<BattleSequenceDirection>();
        mBattlerAction = parentObject.GetComponent<BattlerAction>();

        //UI
        parentObject = GameObject.Find("Panel_CharacterStatus");
        mCharacterStatusController 
        = parentObject.GetComponent<CharacterStatusController>();

        parentObject = GameObject.Find("Panel_Enemy");
        mEnemyGraphicController 
        = parentObject.GetComponent<EnemyGraphicController>();

        m_Manager = GameObject.Find("Manager");
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

        switch (mAutoStatus)
        {
            case eAutoStatus.eAutoStatus_Start:
                //状態フラグ変更
                mAutoStatus = eAutoStatus.eAutoStatus_Act;

                //行動者オブジェクト取得
                actor = mBattleStateDataSinglton.ActorObject;

                if(false == mBattlerAction.IsTargetable(actor)){
                    //対象でない場合対象を変更する
                    actor.tergetNum = mBattlerAction.ChangeTargetNumber(actor);
                }

                if(true == mBattlerAction.IsExecutable(actor)){
                    //アクターの行動
                    mBattlerAction.Role[0](actor);
                    
                    //行動対象キャラクターにフォーカス移動
                    if(actor.belong == eActorScope.Friend){
                        mCharacterStatusController.SetFocus_Character(actor.actorNum);
                        mEffectManager.SetActionEffect_Start(
                            mCharacterStatusController.getPosition(actor.actorNum));
                    }
                    else{
                        mEffectManager.SetActionEffect_Start(
                            mEnemyGraphicController.getPosition(actor.actorNum));
                    }

                    //技名
                    mBattleSequenceDirection.UI_ActionName(actor);            
                    StartCoroutine(mBattleSequenceDirection.SequenceEffect(actor));
                }
                else{
                    mAutoStatus = eAutoStatus.eAutoStatus_Act;
                }

                break;
            case eAutoStatus.eAutoStatus_Act:
                temp = GameObject.FindGameObjectWithTag("AutoState_Act");
                if (temp == null){
                    mAutoStatus = eAutoStatus.eAutoStatus_End;
                    StartCoroutine(DelayMethod(0.1f, () =>
                    {
                        //行動後のオブジェクトを消去する
                        mBattleStateDataSinglton.RemoveTopActor();
                        if(false == mBattlerAction.IsActableCharacters()){
                            //ゲームオーバー画面へ遷移
                            TransitionManager transition = m_Manager.GetComponent<TransitionManager>();
                            transition.nextGameScene = GameScenes.GameOver;
                            transition.Fadeout();
                        }
                        else if(false == mBattlerAction.IsActableEnemies()){
                            //戦闘終了画面へ遷移
                            TransitionManager transition = m_Manager.GetComponent<TransitionManager>();
                            transition.nextGameScene = GameScenes.BattleResult;
                            transition.Fadeout();

                        }
                        else{

                            if (mBattleStateDataSinglton.ActorObject != null) {
                                mAutoStatus = eAutoStatus.eAutoStatus_Start;

                            }
                            else
                            {
                                //HP=0の敵を完全消去する
                                mEnemyGraphicController.DestroyUnactablePrefab();
                                mBattlerAction.DestroyUnactableEnemy();

                                mBattleStateDataSinglton.BattleStateMode
                                    = BattleStateDataSinglton.eBattleState.eBattleState_AutoEnd;
                                    
                            }
                        }


                    }));
                }
                break;
            case eAutoStatus.eAutoStatus_End:

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
