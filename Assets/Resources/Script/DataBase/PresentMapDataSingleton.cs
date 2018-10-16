using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentMapDataSingleton : MonoBehaviour {

	//マップ上のプレイヤー位置
	Vector3 mPlayerPosition = new Vector3(0,0,0);

	public Vector3 PlayerPosition{
		get{return this.mPlayerPosition;}
		set{this.mPlayerPosition = value;}
	}

	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
