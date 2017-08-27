using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    GameObject prefab_Effect = null;
    readonly float mPixelPerUnit = 100; //エフェクトの倍率変換

	// Use this for initialization

    public void MakePrefab(string prefabName)
    {
        //プレハブ指定
        if (prefab_Effect == null)
        {
            GameObject parentObject = GameObject.Find("Panel_Effect");

            prefab_Effect
                = Instantiate((GameObject)Resources.Load(prefabName));
            //Load("FT_Infinity_lite/_Prefabs/Charge/Charge_Fire"));

            prefab_Effect.transform.SetParent(parentObject.transform,false);

            prefab_Effect.GetComponent<ParticleSystem>()
                .GetComponent<Renderer>().sortingLayerName = "Effect";

        }
    }

    //エフェクトの位置設定
    public void SetPosition(Vector3 pos)
    {
        if (prefab_Effect != null)
        {
            pos.x = (pos.x - (float)Screen.width/2)/ mPixelPerUnit;
            pos.y = (pos.y - (float)Screen.height/2)/mPixelPerUnit;

            prefab_Effect.transform.position = pos;
        }
    }
}
