using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterManager : MonoBehaviour {

    private static int mSelectingCharacterNum = 1;

    public int GetSelectingCharacter()
    {
        return mSelectingCharacterNum;
    }

    public int NextSelectingCharacter()
    {

        mSelectingCharacterNum++;

        return mSelectingCharacterNum;
    }

    public int BeforeSelectingCharacter()
    {

        mSelectingCharacterNum--;

        return mSelectingCharacterNum;
    }

    public int TurnStartCharacter()
    {
        mSelectingCharacterNum = 1;

        return mSelectingCharacterNum;
    }	
}
