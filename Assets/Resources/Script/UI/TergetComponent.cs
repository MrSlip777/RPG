using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TergetComponent : MonoBehaviour
{ 
    //ターゲット対象
    static Vector3[] Positions = null;
    //番号は1番目からはじまる
    static int positionNum = 1;
    static eTergetScope mTergetScope = eTergetScope.forOne;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        //決定の場合
        if (Input.anyKeyDown == true
            && (Input.GetKey(KeyCode.Return) == true
            || Input.GetMouseButton(0) == true))
        {
            //ターゲット表示の位置を渡す（単体用でエフェクト表示に使用する）
            GameObject parentObj = GameObject.Find("Canvas");
            BattleSelectState mInstance
            = parentObj.GetComponent<BattleSelectState>();
            
            mInstance.Implement_DecideAct(positionNum,Positions);
        }

        if (mTergetScope == eTergetScope.forFriend 
            || mTergetScope == eTergetScope.forOne) {

            this.transform.position = Positions[positionNum];

            //カーソル操作 対象が単体である場合のみ操作可能
            if (Positions != null)
            {

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (Positions.Length - 1 > positionNum)
                    {
                        positionNum++;
                    }
                    else
                    {
                        positionNum = 1;
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (1 < positionNum)
                    {
                        positionNum--;
                    }
                    else
                    {
                        positionNum = Positions.Length - 1;
                    }
                }
            }
        }
    }

    public void SetTergetPositions(eTergetScope scope,Vector3[] positions)
    {
        positionNum = 1;//初期化も同時に実施する
        mTergetScope = scope;
        Positions = positions;
    }
}
