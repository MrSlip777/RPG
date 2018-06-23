using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMessageController : MonoBehaviour {

    //キャラクターデータシングルトン
    static　CharacterDataSingleton mCharacterDataSingleton;

    //敵データ（シングルトン）
    static EnemiesDataSingleton mEnemiesDataSingleton;

	public PopMessageController(){
        //ローカル変数定義
        GameObject parentObject = null;

        parentObject = GameObject.Find("DataSingleton");
        mCharacterDataSingleton = parentObject.GetComponent<CharacterDataSingleton>();
        mEnemiesDataSingleton = parentObject.GetComponent<EnemiesDataSingleton>();	
		
	}

	public void test(){
		BattlerObject test1 = mCharacterDataSingleton.getBattlerObject(1);
        BattlerObject test2 = mCharacterDataSingleton.getBattlerObject(2);
        BattlerObject test3 = mCharacterDataSingleton.getBattlerObject(3);
        BattlerObject test4 = mCharacterDataSingleton.getBattlerObject(4);
		BattlerObject test5 = mEnemiesDataSingleton.getBattlerObject(1);
        BattlerObject test6 = mEnemiesDataSingleton.getBattlerObject(2);
	}

}
