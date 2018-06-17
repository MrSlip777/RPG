using UnityEngine;

//戦闘中の防御
public enum e_tempGuard{

	OFF = 0,
	ON = 1,

};

//戦闘中のパラメータ増加、減少
enum e_tempLevelParameter{

	Power = 0,
	Guard = 1,
	Speed = 2,

};

enum e_HitRateConst{

	ParameterMax = 255,
	Dexterity_A = 227,//命中率定数
	Dexterity_B = 5,//命中率定数
	Dexterity_C = 12,//命中率定数
	Evasion_A = 57,//回避率
	Evasion_B = 1,//回避率
	Evasion_C = 1,//回避率

};

//保留　レベルに応じて変化するように・・・
public enum e_AttackConst{

	At_A = 3,//攻撃力　＝　定数A×力
	Mg_A = 3,//攻撃力　＝　定数A×力
	Df_A = 1,//防御力　＝　定数A
	DfMg_A = 1,//防御力　＝　定数A


};

public enum e_JudgeHit{

	Miss = 0,
	Hit = 1,
	BadStatus = 2,
	ProbabilityMax = 100,
	ProbabilityMin = 5

}

public enum e_BadStatus{

	Normal = -1,
	Dead = 0,
	Poison = 1,
	Paralysis = 2,
	Confusion = 3

}

enum e_Belong{

	Party = 0,
	Enemy = 1,

};

//パッシブスキル（属性用）
public enum e_AttributeResponsePattern{

	NoGuard = 0,
	Counter = 1,
	Void = 2,
	Half = 3,
	Weak = 4

}

public enum e_ParameterDisplayMode{

	OnlyDisplayHP = 0,
	OnlyDisplayMP = 1,
	DisplayHP_MP = 2

}

//スキルの追加効果
public enum e_AttackOption{

	OFF = -1,
	ON = 1,

};


public class BattlerObject: ScriptableObject{

	/*操作キャラ*/
	string Name;
	int graph_Num;
	int Charactor_Number;	//キャラ人数(戦闘ポインタのみ値がある)

	/*戦闘用*/
	int Enemy_Sign;	/* 0:味方 1:敵 */
	int Terget;
	int NextAction;//次の行動番号

	//スキルのインデックス
    public int[] skillIndex;

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

	public MoveState movestate;
	public BattleProperty battleproperty;
	public tempBattleProperty tempbattleproperty;
	public UpdateBattleProperty updatebattleproperty;
	public AutoProperty autoproperty;//自律キャラ専用
}

[CreateAssetMenu( menuName = "MyGame/Create ParameterTable", fileName = "BattleProperty" )]
public class BattleProperty: ScriptableObject
{
 	public int Level;
	public int HP_max;
	public int HP;
	public int MP_max;
	public int MP;
	public int At;
	public int Df;
	public int Mg;
	public int Sp;
	public int Lc;
	public int Exp;

	public bool[] Status;
	public int[] GuardAttribute;
	public int[] VoidBadStatus;
} // class ParameterTable

public struct UpdateBattleProperty{

	public int HP_max_real;//HP最大値（パッシブスキルの補正値）
	public int MP_max_real;//MP最大値（パッシブスキルの補正値）

	public int Dexterity;//命中率
	public int Evasion;//回避率
	public int OffensivePower;//攻撃力
	public int OffensivePower_Magic;//魔法攻撃力
	public int DefenseForce;//防御力
	public int DefenseForce_Magic;//魔法防御力

}

public struct tempBattleProperty{
	//攻撃、回復などHPなどの表示用
	public int Parameter_Graphic_x;
	public int Parameter_Graphic_y;
	public int Parameter;		//HP変動用
	public int Parameter_MP;	//MP変動用
	public int DisplayMode;	//表示用パラメータ（HP、MP、HP,MP両方）

	public int pre_MP;

	public int ActionState;	//行動状態	攻撃　スキル　アイテム 行動済み　99
	public int ActionProperty;	//行動属性　ダメージ　回復　補助
	public int ActionCost;		//行動コスト
	public int ActionAttackAttribute;	//攻撃属性　

	public int[] tempLevelParameter;
	public int tempSkillParameter;
	public int tempSkillParameter2;

	public int JudgeHit;
	//防御状態
	public int tempGuard;
	//即死回避状態（デス・アヴォイダンス）
	public int tempDeathGuard;
	//魔法反射状態　（リフレクタ）
	public int tempMgReflect;

	//混乱状態突入、解除通知フラグ
	public bool flag_Confusion_Go_Lift;
	//自己回復フラグ
	public bool flag_AutoRecover;
	//自己属性変更フラグ
	public bool flag_ChangeAttritute;

	//麻痺状態時の毒ダメージフラグ
	public bool flag_StatusDamage_Bind;
	
}

public struct AutoProperty{

	//自律キャラ用
	public int Move;
	public int Auto;
	public int rand; //debug
	public string String;
	public int Number;

}

public struct MoveState{

	//マップ移動用
	public int X;
	public int Y;
	public int state;
	public int Chara_Num;
	
}



