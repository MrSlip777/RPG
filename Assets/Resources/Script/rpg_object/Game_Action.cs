using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Action : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool checkItemScope(int[] array)
    {
        //this.item().scope
        List<int> list = new List<int>(array);
        return list.Contains(1);
    }

    bool isForOpponent()
    {
        int[] array = {1,2,3,4,5,6};
        return checkItemScope(array);
    }

    bool isForFriend()
    {
        int[] array = { 7, 8, 9, 10, 11 };
        return this.checkItemScope(array);
    }

    bool isForDeadFriend()
    {
        int[] array = {9,10};
        return this.checkItemScope(array);
    }

    bool isForUser()
    {
        int[] array = {11};
        return this.checkItemScope(array);
    }

    bool isForOne()
    {
        int[] array = { 1, 3, 7, 9, 11 };
        return this.checkItemScope(array);
    }

    bool isForRandom()
    {
        int[] array = { 3, 4, 5, 6 };
        return this.checkItemScope(array);
    }

    bool isForAll()
    {
        int[] array = { 2, 8, 10 };
        return this.checkItemScope(array);
    }

    bool needsSelection()
    {
        int[] array = { 1, 7, 9 };
        return this.checkItemScope(array);
    }
}
