using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {

    GameObject prefab_Effect = null;

	// Use this for initialization

    public void MakePrefab()
    {
        //プレハブ指定
        if (prefab_Effect == null)
        {
            prefab_Effect
                = Instantiate((GameObject)Resources.
                Load("FT_Infinity_lite/_Prefabs/Buff/Discharge_Lightning"));
                //Load("FT_Infinity_lite/_Prefabs/Charge/Charge_Fire"));
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void DoEffect()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(5);
        MakePrefab();
    }
}
