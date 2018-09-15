/*
*参考URL　http://tsubakit1.hateblo.jp/entry/2014/12/18/040252
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuController : MonoBehaviour {

    //UIインスタンス
    static private GameObject ScrollViewPrefab = null;
    static private GameObject DescriptionPrefab = null;

    //インスタンス定義
    private static SkillDataSingleton mSkillDataSingleton;

    //スクロービューのコンテンツ名
    private static string[] mContentName;

    //スクロービューのコンテンツ説明文
    private static string[] mContentDescription;

    //コンテンツのボタンオブジェクト（有効化、無効化に必要）
    private GameObject[] mContents;

    //アイテムコンテンツ（仮）
    string[] ItemContentName = { "アイテム１", "アイテム２" };
    string[] ItemContentDescription = { "アイテム１説明", "アイテム２説明" };

    //UI作成
    public void MakeUI(){
        InitializeContents();
        MakeScrollView();
        MakeDescription();
    }

    private void MakeScrollView(){
        //ローカル変数定義
        GameObject parentObject = null;
        GameObject prefab = null;

        //スクロールビューを生成
        parentObject = GameObject.Find("Canvas");
        ScrollViewPrefab = Instantiate(
            (GameObject)Resources.Load("Prefabs/Scroll View"));
        ScrollViewPrefab.transform.SetParent(parentObject.transform,false);

        //各項目を生成
        parentObject = GameObject.Find("Content");
        GameObject obj = (GameObject)Resources.Load("Prefabs/Node");

        for (int i = 0; i < mContentName.Length; i++)
        {
            Transform gText = obj.transform.Find("Text");

            obj.name = "contentNo_" + i + "_";

            //コンテンツの名前
            gText.GetComponent<Text>().text = mContentName[i];

            prefab = Instantiate(obj);
            prefab.transform.SetParent(parentObject.transform,false);

        }

        mContents = GameObject.FindGameObjectsWithTag("Node");
    }

    private void MakeDescription()
    {
        //ローカル変数定義
        GameObject parentObject = null;

        //説明文UIを生成
        parentObject = GameObject.Find("Canvas");
        DescriptionPrefab = (GameObject)Instantiate(
            (GameObject)Resources.Load("Prefabs/Panel_Text"));

        DescriptionPrefab.transform.SetParent(parentObject.transform,false);

    }

    //UI削除
    public void DestroyUI(){
        Destroy(ScrollViewPrefab);
        Destroy(DescriptionPrefab);
    }  

    //コンテンツを初期化する
    public void InitializeContents(){
        mContentName = new string[8];
        mContentDescription = new string[8];
        for (int i = 0; i < 8; i++)
        {
            mContentName[i] = "----";
            mContentDescription[i] = "----";
        }
    }

    //キャラクターから所持スキルの名前、スキルの説明文を取得、設定する
    public void SetContents(MainMenuController.eMainButton pushbutton, int[] index)
    {
        
        if (MainMenuController.eMainButton.eButton_Skill == pushbutton) {

            mContentName = new string[index.Length];
            mContentDescription = new string[index.Length];

            if (mSkillDataSingleton == null)
            {
                GameObject parentObject = GameObject.Find("DataSingleton");
                mSkillDataSingleton 
                = parentObject.GetComponent<SkillDataSingleton>();
            }

            for (int i = 0; i < index.Length; i++)
            {
                mContentName[i]
                    = mSkillDataSingleton.GetSkillName(index[i]);
                mContentDescription[i]
                    = mSkillDataSingleton.GetSkillDescription(index[i]);
                
            }
        }
        else{
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
        ScrollViewPrefab.SetActive(true);

        ScrollRect scrollRect = ScrollViewPrefab.GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 1;

        DescriptionPrefab.SetActive(true);
        SetDescription(mContentDescription[1]);

        if(mContents != null && mContentName != null){
            for(int i=0; i<mContents.Length; i++){
                mContents[i].GetComponent<Button>().interactable = true;
                Transform gText = mContents[i].transform.Find("Text");
                if(i<mContentName.Length){
                    gText.GetComponent<Text>().text = mContentName[i];
                }
                else{
                    gText.GetComponent<Text>().text = "----";
                    mContents[i].GetComponent<Button>().interactable = false;
                }

                //最初のコンテンツにフォーカスを合わせる（使用不可の場合は次のコンテンツへ（未実装））
                if (i == 0)
                {
                    mContents[i].GetComponent<Button>().Select();
                }            
            }
        }
    }

    //サブメニュー（スクロール、説明文）を表示
    public void HideSubMenu()
    {
        ScrollViewPrefab.SetActive(false);
        DescriptionPrefab.SetActive(false);
    }

    //コンテンツ説明用のテキストを設定する
    private void SetDescription(string Descriptions)
    {
        //ローカル変数定義
        GameObject parentObject = null;

        parentObject = GameObject.Find("Panel_Text(Clone)");

        Transform gText = parentObject.transform.Find("Text");

        //暫定的なコンテンツの名前
        gText.GetComponent<Text>().text = Descriptions;
    }

    //コンテンツ説明用のテキストを変更する
    public void ChangeDescription(string ContentName)
    {
        if(ScrollViewPrefab.activeSelf == true){
            string st_temp = ContentName;
            string[] st_split = st_temp.Split('_');
            if (st_split[0] == "contentNo")
            {
                if(int.Parse(st_split[1]) < mContentDescription.Length){
                    SetDescription(mContentDescription[int.Parse(st_split[1])]);
                }
                else{
                    SetDescription("----");
                }
            }
        }
    }

    //スクロールを動かす
    public void ChangeContentScrollView(string ContentName)
    {
        if(ScrollViewPrefab.activeSelf == true){        
            string st_temp = ContentName;
            string[] st_split = st_temp.Split('_');
            if (st_split[0] == "contentNo")
            {
                ScrollRect scrollRect = ScrollViewPrefab.GetComponent<ScrollRect>();
                scrollRect.verticalNormalizedPosition = 1-(float.Parse(st_split[1])) / (mContentName.Length-1);

            }
        }
    }

}
