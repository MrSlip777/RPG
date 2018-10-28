using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine.system;

public class TransitionfromGameOver : MonoBehaviour {

	GameObject m_Manager = null;

	// Use this for initialization
	void Awake () {
		m_Manager = GameObject.Find("Manager");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
			if(m_Manager == null){
				m_Manager = GameObject.Find("Manager");
			}
			TransitionManager transition = m_Manager.GetComponent<TransitionManager>();
			transition.nextGameScene = GameScenes.Title;
			transition.Fadeout();
		}
	}
}
