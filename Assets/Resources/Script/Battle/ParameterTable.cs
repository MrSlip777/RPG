using UnityEngine;
using RPGEngine;

namespace RPGEngine.database{
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

	//ステータス用パラメータラベル
	public enum e_StatusLabel{
		HP = 0,
		MP = 1,
		At = 2,
		Df = 3,
		Mg = 4,
		Sp = 6,
		Lc = 7,
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

	//行動がどのような特性をもたらすか（おおまかな分類）
	public enum ActProperty{

		//戦闘用
		Damage = 0,
		Recover = 1,
		Assist = 2,
		NoMove = 3,
		ActInitial = 4,
		Guard = 5,
		StatusDamage = 6,
		BadStatus = 7,
		StatusUpDown = 8,

		//蘇生魔法
		Active_Revaival = 9,

		//アイテム使用
		Active_RecoverItem = 10,

		//パッシブスキル
		Passive_DamageCounter = 11,
		Passive_DamageVoid = 12,
		Passive_DamageHalf = 13,
		
		//攻撃魔法
		Active_AttackMagic = 14,

		//ステータス回復
		StatusRecover = 15,

		//パッシブスキル
		Passive_DamageWeak = 16,

		//パッシブスキル(装備)
		Passive_HP_UP = 20,
		Passive_MP_UP = 21,
		Passive_At_UP = 22,
		Passive_Gu_UP = 23,
		Passive_Mg_UP = 30,
		Passive_Sp_UP = 31,
		Passive_Av_UP = 32,

		//パッシブスキル(ステータス異常無効)
		Passive_BadStatusVoid = 24,

		//補助効果（一時即死回避）
		Active_DeathGuard = 25,

		//属性変更
		Active_ChangeAttribute = 26,

		//魔法反射
		Active_MgReflect = 27,

	};

	public class BattlerObject: ScriptableObject{

		//共通パラメータ
		public string Name;

		/*操作キャラ*/
		int graph_Num;
		int Charactor_Number;	//キャラ人数(戦闘ポインタのみ値がある)

		/*戦闘用*/
		//int Enemy_Sign;	/* 0:味方 1:敵 */
		//int Terget;
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

	[CreateAssetMenu( menuName = "MyGame/Create BattleProperty", fileName = "BattleProperty" )]
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
}