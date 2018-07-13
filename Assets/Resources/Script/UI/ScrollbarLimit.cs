using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarLimit : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(gameObject.GetComponent<Scrollbar>().value < 0.2f){
			gameObject.GetComponent<Scrollbar>().value = 0.2f;
		}
	}
}
