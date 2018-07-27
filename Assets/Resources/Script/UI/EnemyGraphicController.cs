using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Textに必要

public class EnemyGraphicController : MonoBehaviour {

    private GameObject[] prefab_Enemy = null;
    public GameObject[] prefab_Damage = null;
    public EnemiesDataSingleton mEnemiesDataSingleton;
    public TroopsDataSingleton mTroopsDataSingleton;

    static float CountTime = 0.0f;

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

        if(Mathf.Sin(Time.frameCount*8.0f)>0){
            iTween.ScaleAdd(prefab_Enemy[TergetNum]
            ,iTween.Hash("x",10.0f,"y",50.0f,"time",8.0f));
        }
        else{
            iTween.ScaleAdd(prefab_Enemy[TergetNum]
            ,iTween.Hash("x",-10.0f,"y",-50.0f,"time",8.0f));
        }
        
    }   

    public void Scale_all(){
        for(int TergetNum = 1; TergetNum<prefab_Enemy.Length; TergetNum++){
            Scale(TergetNum);
        }
    }

    public void Shake(int TergetNum){
        iTween.ShakePosition(prefab_Enemy[TergetNum]
        ,iTween.Hash("x",0.05f,"y",0.05f,"time",0.5f));
    }

    public void Shake_all(){
        for(int TergetNum = 1; TergetNum<prefab_Enemy.Length; TergetNum++){
            Shake(TergetNum);
        }
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

    public void ActionParam_all(int param){
        //ローカル変数定義
        GameObject parentObject = GameObject.Find("Panel_Enemy");
        
        for(int i = 1; i<prefab_Enemy.Length; i++){

            prefab_Damage[i]
            = Instantiate((GameObject)Resources.Load("Prefabs/Damage_Text"));
            prefab_Damage[i].transform.SetParent(parentObject.transform,false);
            
            Vector3 posDamage
            = new Vector3(prefab_Enemy[i].transform.position.x
            ,prefab_Enemy[i].transform.position.y
            ,prefab_Damage[i].transform.position.z);
            
            prefab_Damage[i].transform.position = posDamage;

            prefab_Damage[i].GetComponentInChildren<Text>().color
                = new Color(1.0f,0.125f,0.125f,1.0f);
            prefab_Damage[i].GetComponentInChildren<Text>().text
                = param.ToString();
        }
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
