using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGEngine.system;
using RPGEngine.database;

public class BattleSequenceDirection : MonoBehaviour {

    //エフェクト
    EffectManager mEffectManager = null;
	//行動パターン
	BattlerAction mBattlerAction = null;
	//スキルデータ
    SkillDataSingleton mSkillDataSingleton = null;
    //UI
    CharacterStatusController mCharacterStatusController = null;
    EnemyGraphicController mEnemyGraphicController = null;

	void Awake(){
		GameObject parentObject = null;

		//行動パターン定義
        parentObject = GameObject.Find("Canvas");
        mBattlerAction = parentObject.GetComponent<BattlerAction>();

		//エフェクト管理
        parentObject = GameObject.Find("Panel_Effect");
		mEffectManager = parentObject.GetComponent<EffectManager>();

		parentObject = GameObject.Find("DataSingleton");
        mSkillDataSingleton 
        = parentObject.GetComponent<SkillDataSingleton>();

        //UI
        parentObject = GameObject.Find("Panel_CharacterStatus");
        mCharacterStatusController 
        = parentObject.GetComponent<CharacterStatusController>();

        parentObject = GameObject.Find("Panel_Enemy");
        mEnemyGraphicController 
        = parentObject.GetComponent<EnemyGraphicController>();

	}

    //パラメータ表示（ダメージなど）
    private void UI_ActionParam(eActorScope belong,int tergetNum){

        TergetParam tergetParam;

        tergetParam = mBattlerAction.getTergetParam(tergetNum);
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
    public void UI_ActionName(ActorObject actor){
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
        tergetParam = mBattlerAction.getTergetParam(tergetNum);
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
                if(false == mBattlerAction.IsTargetable(actor)){
                    //敵グラを消去する
                    mEnemyGraphicController.Erase(actor.tergetNum);
                }
            }
            else if(actor.terget == eTergetScope.forAll ||actor.terget == eTergetScope.forRandom){
                for(int tergetNum=1; tergetNum<=mBattlerAction.EnemiesNumber(); tergetNum++){
                
                    if(false == mBattlerAction.IsTargetable_Num(actor,tergetNum)){
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
    public IEnumerator SequenceEffect(ActorObject actor)
    {

        int tergetMAXNUM = 1;
        tergetMAXNUM = mBattlerAction.TergetParamCount();
        SkillEffectData skillEffectData = Resources.Load<SkillEffectData> ("data/Physical1");

        if(actor.terget == eTergetScope.forRandom){
            mEffectManager.SetBackEffect(skillEffectData.backEffect_base,new Vector3(0,0,0));
            for(int number = 0; number<tergetMAXNUM; number++){
                yield return new WaitForSeconds(0.2f);
                //エフェクト表示
                TergetParam tergetParam;
                tergetParam = mBattlerAction.getTergetParam(number);
                mEffectManager.SetEffect(skillEffectData.frontEffect_base,actor.tergetPos[tergetParam.tergetNum]);
                //ダメージを表示させる
                UI_ActionParam(actor.belong,number);
            
                yield return new WaitForSeconds(0.5f);
                mBattlerAction.gainTergetHP(actor.belong,number);
                UI_TergetAction(actor.belong,number);
            }
        }
        else{
            yield return new WaitForSeconds(0.2f);
            //エフェクト表示

            mEffectManager.SetBackEffect(skillEffectData.backEffect_base,actor.tergetPos[actor.tergetNum]);
            mEffectManager.SetEffect(skillEffectData.frontEffect_base,actor.tergetPos[actor.tergetNum]);

            //ダメージを表示させる
            for(int number = 0; number<tergetMAXNUM; number++){
                UI_ActionParam(actor.belong,number);
            }
            yield return new WaitForSeconds(1.0f);
            for(int number = 0; number<tergetMAXNUM; number++){
                mBattlerAction.gainTergetHP(actor.belong,number);
                UI_TergetAction(actor.belong,number);
            }
        }

        UI_ChangeHPgauge(actor);
        
        UI_EnemyErase(actor);        
        mBattlerAction.EraseTergetParam();
    }
}
