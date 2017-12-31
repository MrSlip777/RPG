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
    public int HP;
    public int MP;
    public int Atk;
    public int Def;
    public int Speed;
}

public class CharacterDataSingleton:MonoBehaviour{

    private CharacterObject[] mCharacterObject;
    private BattleCharacterObject[] mBattleCharacterObject;

    //�C���X�^���X��`
    ClassesDataSingleton mClassesDataSingleton;
    SkillDataSingleton mSkillDataSingleton;

    //�s���L����
    private static int mSelectingCharacterNum = 1;

    void Start()
    {
        FileRead_CharacterData();
        mClassesDataSingleton 
        = gameObject.GetComponent<ClassesDataSingleton>();
        mSkillDataSingleton
        = gameObject.GetComponent<SkillDataSingleton>();
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
        learningsObject[] lerningsObjects = null;

        mBattleCharacterObject = new BattleCharacterObject[mCharacterObject.Length];

        for (int j=0; j< mCharacterObject.Length; j++) {
            mBattleCharacterObject[j] = new BattleCharacterObject();

            //�\�͐ݒ�
            mBattleCharacterObject[j].Atk = 10;
            mBattleCharacterObject[j].Def = 11;
            mBattleCharacterObject[j].Speed = j+3;
            mBattleCharacterObject[j].HP = 30;
            mBattleCharacterObject[j].MP = 30;

            lerningsObjects = mClassesDataSingleton.getLearningObject(1);

            mBattleCharacterObject[j].skillIndex
                = new int[lerningsObjects.Length];

            int i = 0;
            //�X�L���ݒ�
            foreach (learningsObject learningObject in lerningsObjects) {
                mBattleCharacterObject[j].skillIndex[i]
                    = learningObject.skillId;
                i++;
            }
        }
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

    //�g�p�X�L���̑Ώۂ�n��
    public eTergetScope GetSkillScope(int skillId)
    {   
        return (eTergetScope)mSkillDataSingleton.GetSkillScope(skillId);
    }

    //�s���I���ʂ̑I���Ԃ̎擾
    public int GetSelectingCharacter()
    {
        return mSelectingCharacterNum;
    }

    //���̑I���ԃL�����ύX
    public int NextSelectingCharacter()
    {
        //���L������HP�����ɃC���N�������g���邩�𔻒f����
        //�������@Slip
        mSelectingCharacterNum++;

        return mSelectingCharacterNum;
    }

    //�O�̑I���ԃL�����ύX
    public int BeforeSelectingCharacter()
    {
        //���L������HP�����Ƀf�N�������g���邩�𔻒f����
        //�������@Slip
        mSelectingCharacterNum--;

        return mSelectingCharacterNum;
    }

    //�s���I��҂̏�����
    public int TurnStartCharacter()
    {
        //���L������HP�����ɒl��K�X�ύX����
        //�������@Slip
        mSelectingCharacterNum = 1;

        return mSelectingCharacterNum;
    }

    //�s���҂̃^�[�Q�b�g���

    //�f�����擾
    public int CharaSpeed(int characterNum)
    {
        return mBattleCharacterObject[characterNum].Speed;
    }
}
