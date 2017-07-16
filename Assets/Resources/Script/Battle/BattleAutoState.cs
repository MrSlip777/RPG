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

    private BattleAutoState()
    {

    }

    //キャラクターステータス表示ウインドウ
    CharacterStatusController mCharacterStatusController
        = CharacterStatusController.Instance;

    //行動フラグ
    private static bool flag = false;


    // Use this for initialization
    //エフェクトを自動で表示
    public void StartAction () {

        if (flag == false)
        {
            flag = true;

            //行動対象キャラクターにフォーカス移動
            mCharacterStatusController.SetFocus_Character(1);

            //テストコード　2017/07/09
            EffectManager mtest = new EffectManager();
            mtest.MakePrefab();

            //ローカル変数定義
            GameObject parentObject = null;
            parentObject = GameObject.Find("Canvas");

            //技名
            GameObject prefab_ActionText = null;
            //親を指定し、技名ウインドウを作成する
            prefab_ActionText = Instantiate((GameObject)Resources.Load("Prefabs/Action_Text"));
            prefab_ActionText.transform.SetParent(parentObject.transform);

            //ダメージを表示させる
            GameObject prefab_Damage = null;
            //親を指定し、技名ウインドウを作成する
            prefab_Damage = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
            prefab_Damage.transform.SetParent(parentObject.transform);
        }
    }

    // Update is called once per frame
    public void _Update () {
		
	}

    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="waitTime">遅延時間[ミリ秒]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
