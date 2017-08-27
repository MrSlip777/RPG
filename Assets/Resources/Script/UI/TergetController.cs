using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //imageで必要

public class TergetController : MonoBehaviour {

    //シングルトン実装
    private static TergetController mInstance;

    // 唯一のインスタンスを取得します。
    public static TergetController Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new TergetController();
            }

            return mInstance;
        }

    }


    private TergetController()
    {

    }

    //インスタンス定義
    CharacterStatusController mCharacterStatusController = CharacterStatusController.Instance;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //ターゲットを表示/非表示にする
    public void ShowHide_Terget(eTergetScope Scope)
    {
        Vector3[] friendPositions = GetPosition("CharacterStatus");
        Vector3[] enemyPositions = GetPosition("EnemyGraphic");
        GameObject obj = null;

        //敵グラのみ座標変換
        enemyPositions = TransformPosition(enemyPositions);

        //敵の位置、味方の位置をシングルトン経由でわたすこと　0720 slip
        switch (Scope)
        {
            case eTergetScope.forOne:
                obj = MakePrefab(enemyPositions[0]);
                obj.GetComponent<TergetComponent>().SetTergetPositions(Scope,enemyPositions);

                break;
            case eTergetScope.forAll:
                foreach (Vector3 position in enemyPositions)
                {
                    MakePrefab(position);
                }
                break;
            case eTergetScope.forFriend:
                obj = MakePrefab(friendPositions[0]);
                obj.GetComponent<TergetComponent>().SetTergetPositions(Scope,friendPositions);

                break;
            case eTergetScope.forFriendAll:
                foreach (Vector3 position in friendPositions)
                {
                    MakePrefab(position);
                }
                break;

            default:
                DestroyPrefab();
                break;
        }
    }

    //ターゲット表示
    private GameObject MakePrefab(Vector3 position)
    {
        GameObject parentObject = GameObject.Find("Canvas");

        //プレハブ指定
        GameObject prefab = Instantiate(
            (GameObject)Resources.Load("Prefabs/terget"));
        
        prefab.transform.SetParent(parentObject.transform, false);
        prefab.transform.position = position;

        return prefab;
    }

    //ターゲット非表示
    private void DestroyPrefab()
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("Terget");
        foreach (GameObject prefab in prefabs) {
            Destroy(prefab);
        }

    }

    public Vector3[] GetPosition(string TagName)
    {
        int i = 0;
        Vector3[] result = null;

        GameObject[] tergets = GameObject.FindGameObjectsWithTag(TagName);

        if (tergets != null)
        {

            result = new Vector3[tergets.Length];

            foreach (GameObject terget in tergets)
            {
                result[i] = terget.transform.position;
                i++;
            }
        }

        return result;
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
