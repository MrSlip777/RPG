/*
*参考URL　http://tsubakit1.hateblo.jp/entry/2014/12/18/040252
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuController : MonoBehaviour {

    //インスタンス定義
    private static SkillDataSingleton mSkillDataSingleton;

    //スクロービューのコンテンツ名
    private static string[] mContentName;

    //スクロービューのコンテンツ名
    private static string[] mContentDescription;

    //アイテムコンテンツ（仮）
    string[] ItemContentName = { "アイテム１", "アイテム２" };
    string[] ItemContentDescription = { "アイテム１説明", "アイテム２説明" };

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {


    }

    //キャラクターから所持スキルの名前、スキルの説明文を取得、設定する
    public void SetContents(MainMenuController.eMainButton pushbutton, int[] index)
    {
        if (MainMenuController.eMainButton.eButton_Skill == pushbutton) {
            mContentName = new string[index.Length];
            mContentDescription = new string[index.Length];

            if (mSkillDataSingleton == null)
            {
                mSkillDataSingleton = SkillDataSingleton.Instance;
            }

            for (int i = 0; i < index.Length; i++)
            {
                mContentName[i] = mSkillDataSingleton
                    .GetSkillName(index[i]);
                mContentDescription[i] = mSkillDataSingleton
                    .GetSkillDescription(index[i]);
            }
        }
        else
        {
            mContentName = new string[ItemContentName.Length];
            mContentDescription = new string[ItemContentName.Length];

            for (int i = 0; i < ItemContentName.Length; i++)
            {
                mContentName[i] = ItemContentName[i];
                mContentDescription[i] = ItemContentDescription[i];
            }
        }
    }

    //サブメニュー（スクロール、説明文）を表示
    public void ShowSubMenu()
    {
        ShowScrollView();
        ShowDescription();
        SetDescription(mContentDescription[1]);
    }

    //サブメニュー（スクロール、説明文）を表示
    public void HideSubMenu()
    {
        HideScrollView();
        HideDescription();
    }

    //サブメニュー（スクロール）を表示する
    private void ShowScrollView()
    {
        //ローカル変数定義
        GameObject parentObject = null;
        GameObject prefab = null;
      
        //スクロールビューを生成
        parentObject = GameObject.Find("Canvas");
        prefab = (GameObject)Instantiate(
            (GameObject)Resources.Load("Prefabs/Scroll View"));
        prefab.transform.SetParent(parentObject.transform);

        //各項目を生成
        parentObject = GameObject.Find("Content");
        GameObject obj = (GameObject)Resources.Load("Prefabs/Node");

        for (int i = 1; i < mContentName.Length; i++)
        {
            Transform gText = obj.transform.FindChild("Text");

            obj.name = "contentNo" + i;

            //暫定的なコンテンツの名前
            gText.GetComponent<Text>().text = mContentName[i];

            prefab = (GameObject)Instantiate(obj);
            prefab.transform.SetParent(parentObject.transform);

            //最初のコンテンツにフォーカスを合わせる
            if (i == 1)
            {
                prefab.GetComponent<Button>().Select();
            }
        }
        
    }

    //サブメニュー（スクロール）を非表示する
    private void HideScrollView()
    {
        GameObject tergetObject = GameObject.Find("Scroll View(Clone)");
        Destroy(tergetObject);
    }

    //コンテンツ説明用のテキストを表示する
    private void ShowDescription()
    {
        //ローカル変数定義
        GameObject parentObject = null;
        GameObject prefab = null;

        //スクロールビューを生成
        parentObject = GameObject.Find("Canvas");
        prefab = (GameObject)Instantiate(
            (GameObject)Resources.Load("Prefabs/Panel_Text"));

        prefab.transform.SetParent(parentObject.transform);

    }

    //コンテンツ説明用のテキストを設定する
    private void SetDescription(string Descriptions)
    {
        //ローカル変数定義
        GameObject parentObject = null;

        parentObject = GameObject.Find("Panel_Text(Clone)");

        Transform gText = parentObject.transform.FindChild("Text");

        //暫定的なコンテンツの名前
        gText.GetComponent<Text>().text = Descriptions;
    }

    //コンテンツ説明用のテキストを非表示する
    private void HideDescription()
    {
        GameObject tergetObject = GameObject.Find("Panel_Text(Clone)");
        Destroy(tergetObject);
    }

}
