/*
�L�����N�^�[�{�͉̂B������
CharacterManager�����삷��
*/

/*
//��U�R�����g�A�E�g����@Slip 2016/12/26
#ifndef CHARACTORMANAGER_H
#define CHARACTORMANAGER_H

#include "Charactor.h"
#include "MonadStorager.h"
#include "SkillManager.h"

#include "OutputLog.h"

#define NAME_AUTORECOVER_ON "�V���u���j�O���X"
#define NAME_CHANGEATTRIBUTE_ON "�j�������g�z�e�v"

#define MONAD_ELDERSIGN 26	//���i�h�F���_�̈�̊Ǘ��ԍ�
*/

using System.IO;

public class CharacterObject{
    /*����L����*/
    //char Name[Charactor_Name];//��U�R�����g�A�E�g
    int graph_Num;
    int Charactor_Number;   //�L�����l��(�퓬�|�C���^�̂ݒl������)

    /*�퓬�p*/
    int Enemy_Sign; /* 0:���� 1:�G */
    int Terget;
    int NextAction;//���̍s���ԍ�

    /*
    //�X�L���̐�
    //��U�R�����g�A�E�g
    int SkillNumber;
    SkillIndex* skill_index;
    */

    //	���x���A�b�v���Ă���Ƃ���true�@�ʏ�false
    bool flag_levelup;
    //�X�V����X�L���ԍ��@�X�V���ɒl����͂��Ďg�p
    int UpdateSkillNumber;

    //�������Ă��郂�i�h�̔ԍ�
    int PossessionMonadNumber;

    bool flag_UseItem;//�A�C�e�����g�p���邩���f����t���O false�Ŏg�p���Ȃ�

    /*�G��p*/
    int Enemy_Graphic_Number;
    int Enemy_Graphic_Width;
    int Enemy_Graphic_Height;
    bool flag_DeadSE;

    /*
    //��U�R�����g�A�E�g
    MoveState* movestate;
    BattleProperty battleproperty;
    tempBattleProperty tempbattleproperty;
    UpdateBattleProperty updatebattleproperty;
    AutoProperty autoproperty;//�����L������p
    */
}

public class CharacterDataSingleton{

    private int FileHandle;
    //��U�R�����g�A�E�g
    //private MonadStorager *monadstorager;
    //SkillManager *skillmanager;

    //�o���l
    private int Exp;
    //�������@�ݕ��P�ʁFG�i�Q���h�j
    private int mGeld;

    //�G�ϐ��E��_�\���t���O(�퓬���̂ݎg�p)
    private bool flag_weakpoint;
    //�X�e�[�^�X�A�b�v�A�_�E������ʒm�t���O
    private bool flag_StatusLimit;

    //�V���O���g������
    private static CharacterDataSingleton mInstance;

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

	public CharacterObject global_Charactor;

    //����������
    public void Initial() {
    }

    //�ێ����ꂽ�L�����N�^�[�f�[�^��S�Ĕj������
    //Charactor *DeleteCharactor_All(Charactor *charactor);

    //�w�肵���L�����N�^�[�f�[�^��j������
    //Charactor* DeleteCharactor(int charactornumber,Charactor **address_charactor);

    /*
	int GetCharactorNumber(void);
	int GetCharactorState(void);
	int GetCharactorPosX(void);
	int GetCharactorPosY(void);

	void SetCharactorPosX(int X);
	void SetCharactorPosY(int Y);

	void setExperience(int local_Exp);
	void getExperience(int *local_Exp);

	void setflag_weakpoint(bool flag);
	bool getflag_weakpoint(void);

	int ResponseAttribute(Charactor *compChara,int AttackAttribute);//�����U���ւ̉���
	int ResponseBadStatus(Charactor *compChara,int AttackAttribute,int AttackBadStatus);//�X�e�[�^�X�ُ�U���ւ̉���

	void Read_Parameter(Charactor *local_pChara,char *String);
	void Update_Parameter(Charactor *local_pChara);
	void Initialize_BattleParameter(Charactor *local_pChara);

	Charactor *Auto_FileRead(Charactor *charactor,int *pMap_people,char *filename);

	Charactor *MakeCharactor(void);

	void GuardSelf(Charactor *local_pChara);
	int GetGeld(void);
	void SetGeld(int Geld);

	void setflag_StatusLimit(bool flag);
	bool getflag_StatusLimit(void);
    */
};
