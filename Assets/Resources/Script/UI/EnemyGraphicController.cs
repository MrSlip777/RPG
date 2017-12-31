using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicController : MonoBehaviour {

    static private GameObject[] prefab_Enemy = null;

    public EnemiesDataSingleton mEnemiesDataSingleton;
    public TroopsDataSingleton mTroopsDataSingleton;

    // Use this for initialization
    void Start () {
        //ローカル変数定義
        GameObject parentObject = null;

        parentObject = GameObject.Find("DataSingleton");       

        mEnemiesDataSingleton = parentObject.GetComponent<EnemiesDataSingleton>();
        mTroopsDataSingleton = parentObject.GetComponent<TroopsDataSingleton>();

		ShowEnemy();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //敵グラ生成
    public void ShowEnemy()
    {
        //ローカル変数定義
        GameObject parentObject = null;

        //親オブジェクトの指定
        parentObject = GameObject.Find("Panel_Enemy");

        //プレハブ指定
        prefab_Enemy = new GameObject[mEnemiesDataSingleton.EnemiesNum];

        for (int i=0; i< mEnemiesDataSingleton.EnemiesNum; i++) {
            prefab_Enemy[i] = Instantiate(
                (GameObject)Resources.Load("Prefabs/Enemy"));
            prefab_Enemy[i].transform.SetParent(parentObject.transform, false);
            //敵画像は100倍スケールになっている　1移動で100pixel移動
            //float scale = prefab_Enemy[i].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            prefab_Enemy[i].transform.position = new Vector3(-2+4*i, 0, 0);
        }

    }

    //敵グラ消去
    public void HideEnemy()
    {
        foreach (GameObject prefab in prefab_Enemy) {
            Destroy(prefab);
            prefab_Enemy = null;
        }
    }


}
