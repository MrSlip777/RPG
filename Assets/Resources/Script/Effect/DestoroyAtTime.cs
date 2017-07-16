/*
 *アタッチしたオブジェクトを指定時間で削除する
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;   //imageで必要

public class DestoroyAtTime : MonoBehaviour {

    public float destroyTime = 10.0f;
    public float delayTime = 0.0f;

    private string ObjectName = null;

    void Start()
    {
        ObjectName = gameObject.name;
        //デフォルトは非表示にする
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.transform.Find("Text").GetComponent<Text>().enabled = false;

        StartCoroutine(DelayMethod(delayTime, () =>
        {
            GameObject parentObject = GameObject.Find("Canvas");
            parentObject.transform.
            Find(ObjectName).GetComponent<Image>().enabled = true;

            parentObject = GameObject.Find(ObjectName);
            parentObject.transform.
            Find("Text").GetComponent<Text>().enabled = true;

            Destroy(gameObject, destroyTime);
        }));
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    /// <summary>
    /// 渡された処理を指定時間後に実行する
    /// </summary>
    /// <param name="waitTime">遅延時間[ミリ秒]</param>
    /// <param name="action">実行したい処理</param>
    /// <returns></returns>
    IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

}
