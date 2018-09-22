﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.system;

public class CheckResult : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){ 
			TransitionManager transition = new TransitionManager();
			transition.nextGameScene = GameScenes.Map;
			transition.Fadeout();
		}
	}
}
