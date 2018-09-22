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
using UnityEngine.UI;
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
    //スキルデータ
    SkillDataSingleton mSkillDataSingleton = null;
    //エフェクト
    EffectManager mEffectManager = null;
    //行動パターン
    BattlerAction mButtlerAction = null;

    ActorObject actor = null;

    //UI
    CharacterStatusController mCharacterStatusController = null;
    EnemyGraphicController mEnemyGraphicController = null;

    void Awake(){
        GameObject parentObject = null;
        mAutoStatus = eAutoStatus.eAutoStatus_Start;
        parentObject = GameObject.Find("DataSingleton");
        mBattleStateDataSinglton 
        = parentObject.GetComponent<BattleStateDataSinglton>();
        mSkillDataSingleton 
        = parentObject.GetComponent<SkillDataSingleton>();

        //エフェクト管理
        parentObject = GameObject.Find("Panel_Effect");
        mEffectManager = parentObject.GetComponent<EffectManager>();

        //行動パターン定義
        mButtlerAction = new BattlerAction();

        //UI
        parentObject = GameObject.Find("Panel_CharacterStatus");
        mCharacterStatusController 
        = parentObject.GetComponent<CharacterStatusController>();

        parentObject = GameObject.Find("Panel_Enemy");
        mEnemyGraphicController 
        = parentObject.GetComponent<EnemyGraphicController>();

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

                if(false == mButtlerAction.IsTargetable(actor)){
                    //対象でない場合対象を変更する
                    actor.tergetNum = mButtlerAction.ChangeTargetNumber(actor);
                }

                if(true == mButtlerAction.IsExecutable(actor)){
                    //アクターの行動
                    mButtlerAction.Role[0](actor);
                    
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
                    UI_ActionName(actor);            
                    StartCoroutine(SequenceEffect(actor));
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
                        if(false == mButtlerAction.IsActableCharacters()){
                            //ゲームオーバー画面へ遷移
                            TransitionManager transition = new TransitionManager();
                            transition.nextGameScene = GameScenes.GameOver;
                            transition.Fadeout();
                        }
                        else if(false == mButtlerAction.IsActableEnemies()){
                            //戦闘終了画面へ遷移
                            TransitionManager transition = new TransitionManager();
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
                                mButtlerAction.DestroyUnactableEnemy();

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

    //パラメータ表示（ダメージなど）
    private void UI_ActionParam(eActorScope belong,int tergetNum){

        TergetParam tergetParam;

        tergetParam = mButtlerAction.getTergetParam(tergetNum);
        //攻撃の場合
        if(tergetParam.terget == eTergetScope.forOne || tergetParam.terget == eTergetScope.forAll
        ||tergetParam.terget == eTergetScope.forRandom){
            if(belong == eActorScope.Friend){
                mEnemyGraphicController.ActionParam(tergetParam.tergetNum,tergetParam.Parameter);
            }
            else{
                mCharacterStatusController.ActionParam(tergetParam.tergetNum,tergetParam.Parameter);
            }
        }
    }

    //UI 行動名
    private void UI_ActionName(ActorObject actor){
        //ローカル変数定義
        GameObject parentObject = GameObject.Find("Canvas");

        //技名
        GameObject prefab_ActionText = null;
        //親を指定し、技名ウインドウを作成する
        prefab_ActionText = Instantiate((GameObject)Resources.Load("Prefabs/Action_Text"));
        prefab_ActionText.transform.SetParent(parentObject.transform,false);
        prefab_ActionText.GetComponent<DestoroyAtTime>().delayTime = 0.0f;
        prefab_ActionText.GetComponentInChildren<Text>().text
            = mSkillDataSingleton.GetSkillName(actor.skillID);
    }

    //UIへのターゲットの動作反映
    private void UI_TergetAction(eActorScope belong,int tergetNum){
        TergetParam tergetParam;
        tergetParam = mButtlerAction.getTergetParam(tergetNum);
        if(tergetParam.terget == eTergetScope.forOne || tergetParam.terget == eTergetScope.forAll
        || tergetParam.terget == eTergetScope.forRandom){
            if(belong == eActorScope.Friend){
                mEnemyGraphicController.Shake(tergetParam.tergetNum);
            }
            else{
                mCharacterStatusController.Shake(tergetParam.tergetNum);
            }
        }
        else{
            
        }
    }

    private void UI_ChangeHPgauge(ActorObject actor){

        if(actor.terget == eTergetScope.forOne || actor.terget == eTergetScope.forAll
        || actor.terget == eTergetScope.forRandom){
            if(actor.belong == eActorScope.Enemy){
                mCharacterStatusController.ChangeHPValue(actor.tergetNum);
            }
            else{

            }
        }        
    }

    private void UI_EnemyErase(ActorObject actor){
        //味方が敵を攻撃した場合
        if(actor.belong == eActorScope.Friend){
            if(actor.terget == eTergetScope.forOne){
                if(false == mButtlerAction.IsTargetable(actor)){
                    //敵グラを消去する
                    mEnemyGraphicController.Erase(actor.tergetNum);
                }
            }
            else if(actor.terget == eTergetScope.forAll ||actor.terget == eTergetScope.forRandom){
                for(int tergetNum=1; tergetNum<=mButtlerAction.EnemiesNumber(); tergetNum++){
                
                    if(false == mButtlerAction.IsTargetable_Num(actor,tergetNum)){
                        //敵グラを消去する
                        mEnemyGraphicController.Erase(tergetNum);
                    }
                }
            }
            else if(actor.terget == eTergetScope.forFriend || actor.terget == eTergetScope.forFriendAll){

            }
        }
    }

    //遅らせる
    private IEnumerator SequenceEffect(ActorObject actor)
    {
        int tergetMAXNUM = 1;
        tergetMAXNUM = mButtlerAction.TergetParamCount();

        if(actor.terget == eTergetScope.forRandom){
            for(int number = 0; number<tergetMAXNUM; number++){
                yield return new WaitForSeconds(0.2f);
                //エフェクト表示
                TergetParam tergetParam;
                tergetParam = mButtlerAction.getTergetParam(number);
                mEffectManager.SetEffect(actor.tergetPos[tergetParam.tergetNum]);
                //ダメージを表示させる
                UI_ActionParam(actor.belong,number);
            
                yield return new WaitForSeconds(0.5f);
                mButtlerAction.gainTergetHP(actor.belong,number);
                UI_TergetAction(actor.belong,number);
            }
        }
        else{
            yield return new WaitForSeconds(0.2f);
            //エフェクト表示
            mEffectManager.SetEffect(actor.tergetPos[actor.tergetNum]);
            //ダメージを表示させる
            for(int number = 0; number<tergetMAXNUM; number++){
                UI_ActionParam(actor.belong,number);
            }
            yield return new WaitForSeconds(1.0f);
            for(int number = 0; number<tergetMAXNUM; number++){
                mButtlerAction.gainTergetHP(actor.belong,number);
                UI_TergetAction(actor.belong,number);
            }
        }

        UI_ChangeHPgauge(actor);
        
        UI_EnemyErase(actor);        
        mButtlerAction.EraseTergetParam();
    }

    //遅らせる
    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
