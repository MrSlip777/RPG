using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour {

	public static int ScreenWidth = 1024;
	public static int ScreenHeight = 768;

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(ScreenWidth, ScreenHeight, false);

    }

	// Use this for initialization
	void Awake () {
		Screen.fullScreen = false;
	}
	
	// Update is called once per frame
	void Update () {
		//全画面
		if (Input.GetKeyDown(KeyCode.F4) == true)
		{
			if (Screen.fullScreen == true)
			{
				Screen.fullScreen = false;
				Screen.SetResolution(ScreenWidth, ScreenHeight, false);
				
			}
			else
			{
				Screen.fullScreen = true;
			}

		}		
	}
}
