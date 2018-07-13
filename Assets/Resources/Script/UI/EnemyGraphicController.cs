using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGraphicController : MonoBehaviour {

    private GameObject[] prefab_Enemy = null;

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

    public Vector3 getPosition(int ActorNum){
        return prefab_Enemy[ActorNum].transform.position;
    }   

    public void Shake(int TergetNum){
        iTween.ShakePosition(prefab_Enemy[TergetNum]
        ,iTween.Hash("x",0.05f,"y",0.05f,"time",0.5f));
    }

    //敵グラ生成
    public void ShowEnemy()
    {
        //ローカル変数定義
        GameObject parentObject = null;

        //親オブジェクトの指定
        parentObject = GameObject.Find("Panel_Enemy");

        //プレハブ作成（0はnullとする）
        prefab_Enemy = new GameObject[mEnemiesDataSingleton.EnemiesNum + 1];

        for (int i=1; i<= mEnemiesDataSingleton.EnemiesNum; i++) {
            prefab_Enemy[i] = Instantiate(
                (GameObject)Resources.Load("Prefabs/Enemy"));
            prefab_Enemy[i].transform.SetParent(parentObject.transform, false);

            //float scale = prefab_Enemy[i].GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
            prefab_Enemy[i].transform.position = new Vector3(-0.9f+0.6f*i, 0.0f, prefab_Enemy[i].transform.position.z);
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
