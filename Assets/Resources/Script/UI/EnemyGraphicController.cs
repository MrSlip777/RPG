using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicController : MonoBehaviour {

    static private GameObject prefab_Enemy = null;

    static public EnemiesDataSingleton mEnemiesDataSingleton;
    static public TroopsDataSingleton mTroopsDataSingleton;


    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //敵グラ生成
    public void ShowEnemy()
    {
        if (prefab_Enemy == null)
        {
            mEnemiesDataSingleton = EnemiesDataSingleton.Instance;
            mTroopsDataSingleton = TroopsDataSingleton.Instance;


            //バトラーの画像を読み込んで表示する

            //ローカル変数定義
            GameObject parentObject = null;

            //親オブジェクトの指定
            parentObject = GameObject.Find("Panel_Enemy");

            //プレハブ指定
            prefab_Enemy = (GameObject)Instantiate(
                (GameObject)Resources.Load("Prefabs/Enemy"));
            prefab_Enemy.transform.SetParent(parentObject.transform, false);

        }
    }

    //敵グラ消去
    public void HideEnemy()
    {
        Destroy(prefab_Enemy);
        prefab_Enemy = null;
    }


}
