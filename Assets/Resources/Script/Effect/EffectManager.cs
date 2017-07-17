using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    GameObject prefab_Effect = null;

	// Use this for initialization

    public void MakePrefab(string prefabName)
    {
        //プレハブ指定
        if (prefab_Effect == null)
        {
            prefab_Effect
                = Instantiate((GameObject)Resources.Load(prefabName));
                //Load("FT_Infinity_lite/_Prefabs/Charge/Charge_Fire"));
        }
    }
}
