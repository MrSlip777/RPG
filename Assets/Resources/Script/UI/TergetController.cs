using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //imageで必要

public class SearchUI: MonoBehaviour {
    public Vector3[] GetPosition(string TagName)
    {
        int i = 0;
        Vector3[] result = null;

        GameObject[] tergets = GameObject.FindGameObjectsWithTag(TagName);

        if (tergets != null)
        {

            result = new Vector3[tergets.Length + 1];

            result[0] = new Vector3();
            i=1;
            foreach (GameObject terget in tergets)
            {
                result[i] = terget.transform.position;
                i++;
            }
        }

        return result;
    }
}

public class TergetController : SearchUI {

    //UIの最大個数
    private readonly int UIMaxNumber = 8;
    static GameObject[] UIPrefabs = null;

    private TergetController()
    {

    }

    //UIを作成する関数
    public void MakeUI(){
        //格納先を確保
        UIPrefabs = new GameObject[UIMaxNumber];

        //親を探す
        GameObject parentObject = GameObject.Find("Canvas");

        for(int i=0; i<UIMaxNumber; i++){
        //プレハブ指定
            UIPrefabs[i] = Instantiate(
                (GameObject)Resources.Load("Prefabs/terget"));
            
            UIPrefabs[i].transform.SetParent(parentObject.transform, false);
            UIPrefabs[i].SetActive(true);
        }
        //単体用
        UIPrefabs[0].AddComponent<TergetComponent>();
        //全体用
        UIPrefabs[1].AddComponent<TergetComponent>();
    }

    //ターゲットを表示/非表示にする
    public void ShowHide_Terget(eTergetScope Scope)
    {
        Vector3[] friendPositions = GetPosition("CharacterStatus");
        Vector3[] enemyPositions = GetPosition("EnemyGraphic");

        //敵の位置、味方の位置をシングルトン経由でわたすこと　0720 slip
        switch (Scope)
        {
            case eTergetScope.forOne:
                UIPrefabs[0].SetActive(true);
                UIPrefabs[0].transform.position = enemyPositions[1];
                UIPrefabs[0].GetComponent<TergetComponent>().SetTergetPositions(Scope,enemyPositions);

                break;

            case eTergetScope.forAll:
                for(int i = 1; i<enemyPositions.Length; i++)
                {
                    UIPrefabs[i].SetActive(true);
                    UIPrefabs[i].transform.position = enemyPositions[i];
                }
                UIPrefabs[1].GetComponent<TergetComponent>().SetTergetPositions(Scope,enemyPositions);
                break;
               
            case eTergetScope.forFriend:
                UIPrefabs[0].SetActive(true);
                UIPrefabs[0].transform.position = friendPositions[1];
                UIPrefabs[0].GetComponent<TergetComponent>().SetTergetPositions(Scope,friendPositions);
                
                break;

            case eTergetScope.forFriendAll:
                for(int i=1; i<friendPositions.Length; i++)
                {
                    UIPrefabs[i].SetActive(true);
                    UIPrefabs[i].transform.position = friendPositions[i];
                }
                UIPrefabs[1].GetComponent<TergetComponent>().SetTergetPositions(Scope,friendPositions);
                break;

            default:

                HideUIAll();
                break;
        }
    }

    //ターゲット非表示
    private void HideUIAll(){
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Terget");
        foreach (GameObject prefab in prefabs) {
            prefab.SetActive(false);
        }        
    }

    //プレハブ削除
    private void DestroyPrefab()
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Terget");
        foreach (GameObject prefab in prefabs) {
            Destroy(prefab);
        }

    }

    //位置設定
    public void SetPosition(Vector3 position){
        UIPrefabs[0].transform.position = position;
    }

}
