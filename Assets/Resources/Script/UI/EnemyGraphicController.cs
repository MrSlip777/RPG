using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Textに必要
using System;

public class EnemyGraphicController : MonoBehaviour {

    private List<GameObject> prefab_Enemy;
    public GameObject[] prefab_Damage = null;
    public EnemiesDataSingleton mEnemiesDataSingleton;
    public TroopsDataSingleton mTroopsDataSingleton;

    // Use this for initialization
    void Start () {
        //ローカル変数定義
        GameObject parentObject = null;
        prefab_Damage = new GameObject[5];

        parentObject = GameObject.Find("DataSingleton");       

        mEnemiesDataSingleton = parentObject.GetComponent<EnemiesDataSingleton>();
        mTroopsDataSingleton = parentObject.GetComponent<TroopsDataSingleton>();
  
		ShowEnemy();

	}
	
	// Update is called once per frame
	void Update () {
        Scale_all();
	}

    public Vector3 getPosition(int ActorNum){
        return prefab_Enemy[ActorNum].transform.position;
    }   

    public void Scale(int TergetNum){

        if(prefab_Enemy[TergetNum]){
            if(Mathf.Sin(Time.frameCount*8.0f)>0){
                iTween.ScaleAdd(prefab_Enemy[TergetNum]
                ,iTween.Hash("x",10.0f,"y",50.0f,"time",8.0f));
            }
            else{
                iTween.ScaleAdd(prefab_Enemy[TergetNum]
                ,iTween.Hash("x",-10.0f,"y",-50.0f,"time",8.0f));
            }
        }
    }   

    public void Scale_all(){
        for(int TergetNum = 1; TergetNum<prefab_Enemy.Count; TergetNum++){
            Scale(TergetNum);
        }
    }

    public void Shake(int TergetNum){
        iTween.ShakePosition(prefab_Enemy[TergetNum]
        ,iTween.Hash("x",0.05f,"y",0.05f,"time",0.5f));
    }

    public void Erase(int TergetNum){
        prefab_Enemy[TergetNum].GetComponent<SpriteEraser>().Erase();
    }

    public void ActionParam(int TergetNum,int param){
        //ローカル変数定義
        GameObject parentObject = GameObject.Find("Canvas");
        
        prefab_Damage[0]
        = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
        prefab_Damage[0].transform.SetParent(parentObject.transform,false);
        
        Vector3 posDamage
        = new Vector3(prefab_Enemy[TergetNum].transform.position.x
        ,prefab_Enemy[TergetNum].transform.position.y
        ,prefab_Damage[0].transform.position.z);
        
        prefab_Damage[0].transform.position = posDamage;

        prefab_Damage[0].GetComponentInChildren<Text>().color
            = new Color(1.0f,0.125f,0.125f,1.0f);
        prefab_Damage[0].GetComponentInChildren<Text>().text
            = param.ToString();
    }

    //敵グラ生成
    public void ShowEnemy()
    {
        //ローカル変数定義
        GameObject parentObject = null;

        //親オブジェクトの指定
        parentObject = GameObject.Find("Panel_Enemy");

        //プレハブ作成（0はnullとする）
        prefab_Enemy = new List<GameObject>();

        GameObject prefab = null;

        prefab_Enemy.Add(prefab);

        for (int i=1; i<= mEnemiesDataSingleton.EnemiesNum; i++) {
            prefab = Instantiate(
                (GameObject)Resources.Load("Prefabs/Enemy"));
            prefab.transform.SetParent(parentObject.transform, false);
            prefab.transform.position = new Vector3(-0.9f+0.6f*i, 0.0f, prefab.transform.position.z);
            prefab_Enemy.Add(prefab);
        }

    }

    public void DestroyUnactablePrefab(){
        for (int i = prefab_Enemy.Count - 1; i >= 0; i--) {
            if(prefab_Enemy[i]){
                if (prefab_Enemy[i].GetComponent<SpriteEraser>().IsDead == true) {
                    //Unityのオブジェクトを削除してから
                    Destroy(prefab_Enemy[i]);
                    //List内のデータを削除する
                    prefab_Enemy.Remove(prefab_Enemy[i]);
                }
            }
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
