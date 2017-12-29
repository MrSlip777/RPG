/*
*参考URL　http://tsubakit1.hateblo.jp/entry/2014/12/18/040252
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuController : MonoBehaviour {

    //シングルトン実装
    private static SubMenuController mInstance;

    // 唯一のインスタンスを取得します。
    public static SubMenuController Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new SubMenuController();
            }

            return mInstance;
        }

    }

    private SubMenuController()
    {

    }

    //UIインスタンス
    static private GameObject ScrollViewPrefab = null;
    static private GameObject DescriptionPrefab = null;

    //インスタンス定義
    private static SkillDataSingleton mSkillDataSingleton;

    //スクロービューのコンテンツ名
    private static string[] mContentName;

    //スクロービューのコンテンツ説明文
    private static string[] mContentDescription;

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
        ScrollViewPrefab.transform.SetParent(parentObject.transform);

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
            prefab.transform.SetParent(parentObject.transform);

            //最初のコンテンツにフォーカスを合わせる
            if (i == 0)
            {
                prefab.GetComponent<Button>().Select();
            }
        }
    }

    private void MakeDescription()
    {
        //ローカル変数定義
        GameObject parentObject = null;

        //説明文UIを生成
        parentObject = GameObject.Find("Canvas");
        DescriptionPrefab = (GameObject)Instantiate(
            (GameObject)Resources.Load("Prefabs/Panel_Text"));

        DescriptionPrefab.transform.SetParent(parentObject.transform);

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
                mSkillDataSingleton = SkillDataSingleton.Instance;
            }

            for (int i = 0; i < index.Length; i++)
            {
                mContentName[i]
                    = mSkillDataSingleton.GetSkillName(index[i]);
                mContentDescription[i]
                    = mSkillDataSingleton.GetSkillDescription(index[i]);
                
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
        ScrollViewPrefab.SetActive(true);
        DescriptionPrefab.SetActive(true);
        SetDescription(mContentDescription[1]);

        GameObject[] Contents = GameObject.FindGameObjectsWithTag("Node");
        //Contents[i].FindChild("Text"). //Slip 2017/12/29 途中だけど一旦マスク
        /*
        if(Contents != null && mContentName != null){
            for(int i=1; i<2; i++){    
                Contents[i].GetComponent<Text>().text = mContentName[i];
            }
        }
        */
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
        string st_temp = ContentName;
        string[] st_split = st_temp.Split('_');
        if (st_split[0] == "contentNo")
        {
            SetDescription(mContentDescription[int.Parse(st_split[1])]);
        }
    }

    //スクロールを動かす
    public void ChangeContentScrollView(string ContentName)
    {
        string st_temp = ContentName;
        string[] st_split = st_temp.Split('_');
        if (st_split[0] == "contentNo")
        {
            //ローカル変数定義
            GameObject parentObject = null;

            parentObject = GameObject.Find("Scroll View(Clone)");

            ScrollRect scrollRect = parentObject.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 1-(float.Parse(st_split[1])) / (mContentName.Length-1);

        }
    }

}
