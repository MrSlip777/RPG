/*
*参考URL　http://tsubakit1.hateblo.jp/entry/2014/12/18/040252
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //サブメニュー（スクロール）を表示する
    public void ShowScrollView(string[] ContentName)
    {
        //ローカル変数定義
        GameObject parentObject = null;
        GameObject prefab = null;
        string sTypeName = null;
        
        //スクロールビューを生成
        parentObject = GameObject.Find("Canvas");
        prefab = (GameObject)Instantiate(
            (GameObject)Resources.Load("Prefabs/Scroll View"));
        prefab.transform.SetParent(parentObject.transform);

        //各項目を生成
        parentObject = GameObject.Find("Content");
        GameObject obj = (GameObject)Resources.Load("Prefabs/Node");

        for (int i = 1; i < ContentName.Length; i++)
        {
            Transform gText = obj.transform.FindChild("Text");

            //暫定的なコンテンツの名前
            gText.GetComponent<Text>().text = ContentName[i];

            prefab = (GameObject)Instantiate(obj);
            prefab.transform.SetParent(parentObject.transform);

            //最初のコンテンツにフォーカスを合わせる
            if (i == 1)
            {
                prefab.GetComponent<Button>().Select();
            }

            /*情報を追加する予定
            var text = item.GetComponentInChildren<Text>();
            text.text = "item:" + i.ToString();
            */
        }

    }

    //サブメニュー（スクロール）を非表示する
    public void HideScrollView()
    {
        GameObject tergetObject = GameObject.Find("Scroll View(Clone)");
        Destroy(tergetObject);
    }

}
