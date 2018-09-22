using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.system;

public class ColideWithPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){

		if (collision.gameObject.tag == "Player") {
			TransitionManager transition = new TransitionManager();
			transition.nextGameScene = GameScenes.Battle;
			transition.Fadeout();
		}
	}
}
