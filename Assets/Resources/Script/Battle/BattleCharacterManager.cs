using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCharacterManager : MonoBehaviour {

    private static int mSelectingCharacterNum = 1;

    //キャラクターデータシングルトン
    static　CharacterDataSingleton mCharacterDataSingleton;
    
    void Awake(){
        GameObject parentObject = GameObject.Find("DataSingleton");
        mCharacterDataSingleton = parentObject.GetComponent<CharacterDataSingleton>();
    }

    public int GetSelectingCharacter()
    {
        return mSelectingCharacterNum;
    }

    public int NextSelectingCharacter()
    {
        mSelectingCharacterNum++;

        while(0 == mCharacterDataSingleton.getHP(mSelectingCharacterNum)){
            mSelectingCharacterNum++;

            if(mSelectingCharacterNum > 4){
                break;
            }
        }

        return mSelectingCharacterNum;
    }

    public int BeforeSelectingCharacter()
    {
        mSelectingCharacterNum--;

        while(0 == mCharacterDataSingleton.getHP(mSelectingCharacterNum)){
            mSelectingCharacterNum--;

            if(mSelectingCharacterNum <= 1){
                break;
            }
        }

        return mSelectingCharacterNum;
    }

    public int TurnStartCharacter()
    {
        mSelectingCharacterNum = 1;

        return mSelectingCharacterNum;
    }	
}
