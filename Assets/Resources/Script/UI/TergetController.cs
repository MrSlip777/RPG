﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TergetController : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //ボタンを非表示/表示にする
    public void ShowHide_Terget(bool IsShow)
    {
        //ローカル変数定義
        GameObject parentObject = null;

        //親オブジェクトの指定
        parentObject = GameObject.Find("Enemy(Clone)");

        parentObject.transform.FindChild("terget").
            GetComponent<SpriteRenderer>().enabled = IsShow;

    }

}
