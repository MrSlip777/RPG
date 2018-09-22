using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.database;
public class PopMessageController : AbstructActor {

	public PopMessageController(){
                //ローカル変数定義
                GameObject parentObject = null;

                parentObject = GameObject.Find("DataSingleton");
                mCharacterDataSingleton = parentObject.GetComponent<CharacterDataSingleton>();
                mEnemiesDataSingleton = parentObject.GetComponent<EnemiesDataSingleton>();	
                        
	}

	public string TergetDamage(ActorObject actor){
                BattlerObject Terget = getTerget(actor.belong,actor.terget,actor.tergetNum);
                return Terget.tempbattleproperty.Parameter.ToString();
	}

}
