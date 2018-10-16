using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnouncePlayerPosition : MonoBehaviour {

	PresentMapDataSingleton mPresentMapDataSingleton = null;

	// Use this for initialization
	void Awake(){
        GameObject parentObject = GameObject.Find("DataSingleton");
        mPresentMapDataSingleton 
        = parentObject.GetComponent<PresentMapDataSingleton>();

		mPresentMapDataSingleton.PlayerPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
