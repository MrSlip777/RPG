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
            UIPrefabs[i].SetActive(false);
        }
    }

    //ターゲットを表示/非表示にする
    public void ShowHide_Terget(eTergetScope Scope)
    {
        Vector3[] friendPositions = GetPosition("CharacterStatus");
        Vector3[] enemyPositions = GetPosition("EnemyGraphic");

        //敵グラのみ座標変換
        enemyPositions = TransformPosition(enemyPositions);

        //敵の位置、味方の位置をシングルトン経由でわたすこと　0720 slip
        switch (Scope)
        {
            case eTergetScope.forOne:
                UIPrefabs[0].SetActive(true);
                UIPrefabs[0].transform.position = enemyPositions[1];
                if(UIPrefabs[0].GetComponent<TergetComponent>() == null){
                    UIPrefabs[0].AddComponent<TergetComponent>().SetTergetPositions(Scope,enemyPositions);
                }
                break;

            case eTergetScope.forAll:
                for(int i = 1; i<enemyPositions.Length; i++)
                {
                    UIPrefabs[i].SetActive(true);
                    UIPrefabs[i].transform.position = enemyPositions[i];
                }
                break;
               
            case eTergetScope.forFriend:
                UIPrefabs[0].SetActive(true);
                UIPrefabs[0].transform.position = friendPositions[1];
                if(UIPrefabs[0].GetComponent<TergetComponent>() == null){
                    UIPrefabs[0].AddComponent<TergetComponent>().SetTergetPositions(Scope,friendPositions);
                }                
                break;

            case eTergetScope.forFriendAll:
                for(int i=1; i<friendPositions.Length; i++)
                {
                    UIPrefabs[i].SetActive(true);
                    UIPrefabs[i].transform.position = friendPositions[i];
                }
                break;

            default:
                //選択UIを削除する
                if(UIPrefabs[0].GetComponent<TergetComponent>() != null){
                    Destroy(UIPrefabs[0].GetComponent<TergetComponent>());
                }
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

    //座標変換
    private Vector3[] TransformPosition(Vector3[] positions)
    {
        int i = 0;
        Vector3[] result = null;

        GameObject canvas = GameObject.Find("Canvas");
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        if (positions != null)
        {

            result = new Vector3[positions.Length];

            foreach (Vector3 position in positions)
            {
                result[i] = position;
                result[i].x = result[i].x * 100;
                result[i].x += canvasSize.x / 2;
                result[i].y += canvasSize.y / 2;
                i++;
            }
        }

        return result;
    }

}
