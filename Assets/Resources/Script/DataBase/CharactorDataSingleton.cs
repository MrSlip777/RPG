/*
キャラクター本体は隠蔽する
CharacterManagerが操作する
*/

/*
//一旦コメントアウトする　Slip 2016/12/26
#ifndef CHARACTORMANAGER_H
#define CHARACTORMANAGER_H

#include "Charactor.h"
#include "MonadStorager.h"
#include "SkillManager.h"

#include "OutputLog.h"

#define NAME_AUTORECOVER_ON "シュブ＝ニグラス"
#define NAME_CHANGEATTRIBUTE_ON "ニャルラトホテプ"

#define MONAD_ELDERSIGN 26	//モナド：旧神の印の管理番号
*/

using System.IO;

public class CharacterObject{
    /*操作キャラ*/
    //char Name[Charactor_Name];//一旦コメントアウト
    int graph_Num;
    int Charactor_Number;   //キャラ人数(戦闘ポインタのみ値がある)

    /*戦闘用*/
    int Enemy_Sign; /* 0:味方 1:敵 */
    int Terget;
    int NextAction;//次の行動番号

    /*
    //スキルの数
    //一旦コメントアウト
    int SkillNumber;
    SkillIndex* skill_index;
    */

    //	レベルアップしているときにtrue　通常false
    bool flag_levelup;
    //更新するスキル番号　更新時に値を入力して使用
    int UpdateSkillNumber;

    //所持しているモナドの番号
    int PossessionMonadNumber;

    bool flag_UseItem;//アイテムを使用するか判断するフラグ falseで使用しない

    /*敵専用*/
    int Enemy_Graphic_Number;
    int Enemy_Graphic_Width;
    int Enemy_Graphic_Height;
    bool flag_DeadSE;

    /*
    //一旦コメントアウト
    MoveState* movestate;
    BattleProperty battleproperty;
    tempBattleProperty tempbattleproperty;
    UpdateBattleProperty updatebattleproperty;
    AutoProperty autoproperty;//自律キャラ専用
    */
}

public class CharacterDataSingleton{

    private int FileHandle;
    //一旦コメントアウト
    //private MonadStorager *monadstorager;
    //SkillManager *skillmanager;

    //経験値
    private int Exp;
    //所持金　貨幣単位：G（ゲルド）
    private int mGeld;

    //敵耐性・弱点表示フラグ(戦闘中のみ使用)
    private bool flag_weakpoint;
    //ステータスアップ、ダウン上限通知フラグ
    private bool flag_StatusLimit;

    //シングルトン実装
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

    //初期化する
    public void Initial() {
    }

    //保持されたキャラクターデータを全て破棄する
    //Charactor *DeleteCharactor_All(Charactor *charactor);

    //指定したキャラクターデータを破棄する
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

	int ResponseAttribute(Charactor *compChara,int AttackAttribute);//属性攻撃への応答
	int ResponseBadStatus(Charactor *compChara,int AttackAttribute,int AttackBadStatus);//ステータス異常攻撃への応答

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
