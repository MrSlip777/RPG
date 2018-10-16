using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.system;

public class ColideWithPlayer : MonoBehaviour {

	PresentMapDataSingleton mPresentMapDataSingleton = null;

	// Use this for initialization
	void Awake(){
        GameObject parentObject = GameObject.Find("DataSingleton");
        mPresentMapDataSingleton 
        = parentObject.GetComponent<PresentMapDataSingleton>();

	}
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){

		if (collision.gameObject.tag == "Player") {
			mPresentMapDataSingleton.PlayerPosition = gameObject.transform.position;
			TransitionManager transition = new TransitionManager();
			transition.nextGameScene = GameScenes.Battle;
			transition.Fadeout();
		}
	}
}
