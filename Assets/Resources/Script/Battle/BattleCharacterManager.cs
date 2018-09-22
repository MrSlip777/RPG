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

    public bool IsCharacterTurnEnd(){
        bool result = true;

        int TernEndCharacter = mCharacterDataSingleton.CharactersNum;

        while(0 == mCharacterDataSingleton.getHP(TernEndCharacter)){
            TernEndCharacter--;
            if(TernEndCharacter <= 1){
                    break;
            }
        }

        if(TernEndCharacter > this.GetSelectingCharacter()){
            result = true;
        }
        else{
            result = false;
        }

        return result;
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
        int characterNum = mSelectingCharacterNum;

        mSelectingCharacterNum--;

        while(0 == mCharacterDataSingleton.getHP(mSelectingCharacterNum)){
            
            if(mSelectingCharacterNum <= 1){
                break;
            }
            else{
                mSelectingCharacterNum--;

                if(mSelectingCharacterNum <= 1){
                    break;
                }
            }
        }

        //すべて戦闘不能である場合、もとに戻す
        if(0 == mCharacterDataSingleton.getHP(mSelectingCharacterNum)){
            mSelectingCharacterNum = characterNum;
        }

        return mSelectingCharacterNum;
    }

    public int TurnStartCharacter()
    {
        mSelectingCharacterNum = 1;

        while(0 == mCharacterDataSingleton.getHP(mSelectingCharacterNum)){
            mSelectingCharacterNum++;

            if(mSelectingCharacterNum > 4){
                break;
            }
        }        

        return mSelectingCharacterNum;
    }	
}
