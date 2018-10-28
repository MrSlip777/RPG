using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.system;

public class CheckResult : MonoBehaviour {

	GameObject m_Manager = null;

	// Use this for initialization
	void Start () {
		m_Manager = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){

			TransitionManager transition = m_Manager.GetComponent<TransitionManager>();
			transition.nextGameScene = GameScenes.Map;
			transition.Fadeout();
		}
	}
}
