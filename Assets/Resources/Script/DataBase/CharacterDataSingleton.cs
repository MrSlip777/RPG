/*
*���j�F�c�N�[��MV��json�t�@�C���`���ƌ݊���������悤�ɍ쐬����
*/

using System.IO;
using UnityEngine;

/*
*Actor.json�ɍ��킹��
*�c�N�[��MV ver1.3.4�
*/

public class CharacterObject{

    public int id = 0;
    public string battterName = "";
    public int characterIndex = 0;
    public string characterName = "";
    public int classId = 0;
    public int[] equips;
    public int faceIndex = 0;
    public string faceName = "";
    public int[] traits;
    public int initialLevel = 0;
    public int maxLevel = 0;
    public string name = "";
    public string nickname = "";
    public string note = "";
    public string profile = "";

}

public class BattleCharacterObject
{
    public int[] skillIndex;

}

public class CharacterDataSingleton:MonoBehaviour{

    //�C���X�^���X��`
    private static CharacterDataSingleton mInstance;

    private CharacterObject[] mCharacterObject;
    private BattleCharacterObject[] mBattleCharacterObject;

    //�s���L����
    private static int mSelectingCharacterNum = 1;

    // �B��̃C���X�^���X���擾���܂��B
    public static CharacterDataSingleton Instance
    {

        get
        {
            if (mInstance == null)
            {
                mInstance = new CharacterDataSingleton();
            }

            return mInstance;
        }

    }

    //�V���O���g������
    private CharacterDataSingleton()
    {
        FileRead_CharacterData();
    }

    //json�f�[�^�ǂݍ���
    private void FileRead_CharacterData() {

        string fileName = "Actors";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

       mCharacterObject = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);
    }

    //�I�u�W�F�N�g�Ɋe��f�[�^��ݒ肷��
    public void SetBattleCharacterObject()
    {
        mBattleCharacterObject = new BattleCharacterObject[mCharacterObject.Length];
        mBattleCharacterObject[1] = new BattleCharacterObject();

        mBattleCharacterObject[1].skillIndex = new int[3];
        mBattleCharacterObject[1].skillIndex[0] = 0;
        mBattleCharacterObject[1].skillIndex[1] = 9;
        mBattleCharacterObject[1].skillIndex[2] = 10;
    }

    //�L�����N�^�[�̏����X�L����n��
    public int[] GetSkillIndex(int characterId)
    {
        int[] result = null;

        if (characterId>0 && characterId<=mBattleCharacterObject.Length) {
            result = mBattleCharacterObject[characterId].skillIndex;
        }

        return result;
    }

    //�s���I����ʂ̑I����Ԃ̎擾
    public int GetSelectingCharacter()
    {
        return mSelectingCharacterNum;
    }

    //���̑I����ԃL�����ύX
    public int NextSelectingCharacter()
    {
        //���L������HP�����ɃC���N�������g���邩�𔻒f����
        //�������@Slip
        mSelectingCharacterNum++;

        return mSelectingCharacterNum;
    }

    //�O�̑I����ԃL�����ύX
    public int BeforeSelectingCharacter()
    {
        //���L������HP�����Ƀf�N�������g���邩�𔻒f����
        //�������@Slip
        mSelectingCharacterNum--;

        return mSelectingCharacterNum;
    }

    //�s���I���҂̏�����
    public int TurnStartCharacter()
    {
        //���L������HP�����ɒl��K�X�ύX����
        //�������@Slip
        mSelectingCharacterNum = 1;

        return mSelectingCharacterNum;
    }

    //�s���҂̃^�[�Q�b�g���

}
