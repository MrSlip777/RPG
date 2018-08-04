using UnityEngine;
using System.Collections.Generic;

//ダメージ、回復量などのパラメータ
public class TergetParam{
	public eTergetScope terget;
	public int tergetNum;
	public int Parameter;
}

public class AbstructActor{

	public readonly int PAERCENTAGEMAX = 100;
    //キャラクターデータシングルトン
    protected static　CharacterDataSingleton mCharacterDataSingleton;

    //敵データ（シングルトン）
    protected static EnemiesDataSingleton mEnemiesDataSingleton;

	//パラメータ　単体、全体、ランダムに対応
	public List<TergetParam> ParamList;

	protected BattlerObject getActor(eActorScope belong,int number){
		BattlerObject result = null;

		if(belong == eActorScope.Friend){
			result = mCharacterDataSingleton.getBattlerObject(number);
		}
		else if(belong == eActorScope.Enemy){
			result = mEnemiesDataSingleton.getBattlerObject(number);
		}
		return result;
	}

	protected BattlerObject getTerget(eTergetScope scope,int number){
		BattlerObject result = null;

		if(scope == eTergetScope.forOne || scope == eTergetScope.forAll
		|| scope == eTergetScope.forRandom){
			result = mEnemiesDataSingleton.getBattlerObject(number);
		}
		else if(scope == eTergetScope.forFriend || scope == eTergetScope.forFriendAll){
			result = mCharacterDataSingleton.getBattlerObject(number);
		}

		return result;
	}	

	public void gainTergetHP(int tergetNum){
		TergetParam tergetParam;

		tergetParam = this.getTergetParam(tergetNum);
		if(tergetParam.terget == eTergetScope.forFriend 
		|| tergetParam.terget == eTergetScope.forFriendAll){
			mCharacterDataSingleton.gainHP(tergetParam.tergetNum,-tergetParam.Parameter);
		}
		else{
			mEnemiesDataSingleton.gainHP(tergetParam.tergetNum,-tergetParam.Parameter);
		}
	}

	public TergetParam getTergetParam(int index){
		return ParamList[index];
	}

	public void EraseTergetParam(){
		ParamList.Clear();
	}

	//敵データを消去
	public void DestroyUnactableEnemy(){
		mEnemiesDataSingleton.DestroyUnactableObject();
	}

	//敵が行動可能かを判定する
	public bool IsActableEnemies(){
		return mEnemiesDataSingleton.IsActable();
	}

    public void StatusAttack(ref BattlerObject Actor,ref BattlerObject Terget){

        //ステータス無効がなければステータス異常になる
        //死亡回避がない場合も死亡
        if(Terget.battleproperty.VoidBadStatus[Actor.tempbattleproperty.tempSkillParameter2] == (int)e_AttributeResponsePattern.Void
            ||(Actor.tempbattleproperty.tempSkillParameter2 == (int)e_BadStatus.Dead 
			&& Terget.tempbattleproperty.tempDeathGuard == (int)e_tempGuard.ON)){
            
        }
        else{
            //混乱状態になっている場合意外は
            //混乱状態はフラグをtrueにする　（解除時もtrue 通常時に戻った時false）
            if(Actor.tempbattleproperty.tempSkillParameter2 == (int)e_BadStatus.Confusion &&
                Terget.battleproperty.Status[(int)e_BadStatus.Confusion] == false){
                    Terget.tempbattleproperty.flag_Confusion_Go_Lift = true;
                    Terget.tempbattleproperty.ActionCost = 0;
            }

            Terget.battleproperty.Status[Actor.tempbattleproperty.tempSkillParameter2] = true;
        }

        //死亡状態であれば死亡状態を維持
        //即死効果でHP0になる
        //即死魔法「デス」は闇属性なので、闇属性に耐性がなければ死亡
        if(Terget.battleproperty.Status[(int)e_BadStatus.Dead] == true){

            Terget.battleproperty.HP = 0;
            Terget.battleproperty.Status[(int)e_BadStatus.Dead] = true;
            Terget.battleproperty.Status[(int)e_BadStatus.Poison] = false;
            Terget.battleproperty.Status[(int)e_BadStatus.Paralysis] = false;
            Terget.battleproperty.Status[(int)e_BadStatus.Confusion] = false;
            
        }

        return;
    }
	
    //行動不能である場合の行動スキップ
    public bool IsExecutable(ActorObject actor){

		BattlerObject Actor = getActor(actor.belong,actor.actorNum);

		if(0 == Actor.battleproperty.HP){
			return false;
		}
		else{
			return true;
		}
    }

	//標的可能非対象（HP=0など）であれば別ターゲットを設定する
    public bool IsTargetable(ActorObject actor){

		BattlerObject Terget = getTerget(actor.terget,actor.tergetNum);

		if(0 == Terget.battleproperty.HP){
			return false;
		}
		else{
			return true;
		}
    }

	public int EnemiesNumber(){
		return mEnemiesDataSingleton.EnemiesNum;
	}

	//標的可能非対象（HP=0など）であれば別ターゲットを設定する
    public bool IsTargetable_Num(ActorObject actor,int number){

		BattlerObject Terget = getTerget(actor.terget,number);

		if(0 == Terget.battleproperty.HP){
			return false;
		}
		else{
			return true;
		}
    }

	public int ChangeTargetNumber(ActorObject actor){
		int result = 0;
		eTergetScope scope = actor.terget;
		//攻撃スキル、回復スキルによっても対象が異なるためactorを参照できるようにしておく（SkillID）

		if(scope == eTergetScope.forOne || scope == eTergetScope.forAll){
			result = mEnemiesDataSingleton.getLivingBattlerNumber();
		}
		else if(scope == eTergetScope.forFriend || scope == eTergetScope.forFriendAll){
			result = mCharacterDataSingleton.getLivingBattlerNumber();
		}

		return result;
	}

	public int TergetParamCount(){
		return ParamList.Count;
	}	
}

public delegate void RoleAction(ActorObject actor);

public class BattlerAction : AbstructActor{


	//スキルデータ
	private static SkillDataSingleton mSkillDataSingleton;

	//スキルの実動作
	public List<RoleAction> Role;


	//文字列から計算する役割
	private OperateString mOpString;

	public BattlerAction(){
        //ローカル変数定義
        GameObject parentObject = null;

        parentObject = GameObject.Find("DataSingleton");
        mCharacterDataSingleton = parentObject.GetComponent<CharacterDataSingleton>();
        mEnemiesDataSingleton = parentObject.GetComponent<EnemiesDataSingleton>();	
		mSkillDataSingleton = parentObject.GetComponent<SkillDataSingleton>();

		Role = new List<RoleAction>();
		Role.Add(ActionDamage);

		ParamList = new List<TergetParam>();

		mOpString = new OperateString();
	}

	private void CalcDamage(BattlerObject Actor,BattlerObject Terget){
		int tempJudgeHit = 0;
		int random = 0;

		//乱数の引数は時間(msec)。
		random = (int)(Random.value*(int)e_JudgeHit.ProbabilityMax);

		tempJudgeHit = (int)((double)(Actor.updatebattleproperty.Dexterity)/(double)(Terget.updatebattleproperty.Evasion)/2*PAERCENTAGEMAX);

		if(tempJudgeHit>(int)e_JudgeHit.ProbabilityMax){
			tempJudgeHit = (int)e_JudgeHit.ProbabilityMax;
		}
		else if(tempJudgeHit<(int)e_JudgeHit.ProbabilityMin){
			tempJudgeHit = (int)e_JudgeHit.ProbabilityMin;
		}

		if(random >= 0 && random<tempJudgeHit){
			//スキル威力の補正 ダメージ計算式はSkills.jsonファイルから取得する
			string formula = mSkillDataSingleton.GetFomura(1);
			formula = mOpString.Trans_a_atk(formula,Actor.updatebattleproperty.OffensivePower);
			formula = mOpString.Trans_b_def(formula,Terget.updatebattleproperty.DefenseForce);
			Terget.tempbattleproperty.Parameter = (int)mOpString.Calcformula(formula);
			
			if(Terget.tempbattleproperty.Parameter < 0){
				Terget.tempbattleproperty.Parameter = 0;
			}

			//付加効果（毒、即死など）
			if((int)e_AttackOption.OFF != Actor.tempbattleproperty.tempSkillParameter2){
				StatusAttack(ref Actor,ref Terget);
			}

			//混乱状態であれば解除する
			if(Terget.battleproperty.Status != null){
				if(Terget.battleproperty.Status[(int)e_BadStatus.Confusion] == true){
					Terget.tempbattleproperty.flag_Confusion_Go_Lift = true;
					Terget.tempbattleproperty.ActionCost = 0;
					Terget.battleproperty.Status[(int)e_BadStatus.Confusion] = false;
				}
			}
			Terget.tempbattleproperty.JudgeHit = (int)e_JudgeHit.Hit;
		}
		else{
			Terget.tempbattleproperty.JudgeHit = (int)e_JudgeHit.Miss;
		}

		//HPパラメータのみ表示
		Terget.tempbattleproperty.DisplayMode = (int)e_ParameterDisplayMode.OnlyDisplayHP;
		Terget.tempbattleproperty.Parameter_MP = 0;

		return;
	}

	private void ActionDamage(ActorObject actor){
		BattlerObject Actor = getActor(actor.belong,actor.actorNum);
		
		ParamList.Clear();

		if(actor.terget == eTergetScope.forAll){
			for(int TergetNum=1; TergetNum<=mEnemiesDataSingleton.EnemiesNum; TergetNum++){
				TergetParam tergetParam = new TergetParam();
				BattlerObject Terget = getTerget(actor.terget,TergetNum);
				if(IsTargetable_Num(actor,TergetNum)){
					CalcDamage(Actor,Terget);
					tergetParam.Parameter = Terget.tempbattleproperty.Parameter;
					tergetParam.tergetNum = TergetNum;
					tergetParam.terget = actor.terget;
					ParamList.Add(tergetParam);
				}
			}
		}
		else if(actor.terget == eTergetScope.forRandom){
			int RandomNum = Random.Range (3, 6);
			for(int i=1; i<=RandomNum; i++){
				TergetParam tergetParam = new TergetParam();
				int TergetNum = Random.Range (1, mEnemiesDataSingleton.EnemiesNum+1);
				BattlerObject Terget = getTerget(actor.terget,TergetNum);
				if(IsTargetable_Num(actor,TergetNum)){
					CalcDamage(Actor,Terget);
					tergetParam.Parameter = Terget.tempbattleproperty.Parameter;
					tergetParam.tergetNum = TergetNum;
					tergetParam.terget = actor.terget;
					ParamList.Add(tergetParam);
				}
				else{
					i--;
				}
			}			
		}
		else{
			TergetParam tergetParam = new TergetParam();
			BattlerObject Terget = getTerget(actor.terget,actor.tergetNum);
			CalcDamage(Actor,Terget);
			tergetParam.Parameter = Terget.tempbattleproperty.Parameter;
			tergetParam.tergetNum = actor.tergetNum;
			tergetParam.terget = actor.terget;
			ParamList.Add(tergetParam);
		}
	}
}

/*
void ActionAttackMagic::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;
	DWORD time;
	int tempJudgeHit = 0;
	int random = 0;

	time = timeGetTime();

	//乱数の引数は時間(msec)。
	random = system.getRandomNumber((int)time)%e_JudgeHit_ProbabilityMax;
	
	tempJudgeHit = (int)((double)(Actor.updatebattleproperty.Dexterity)/(double)(Terget.updatebattleproperty.Evasion)/2*PAERCENTAGEMAX);

	if(tempJudgeHit>e_JudgeHit_ProbabilityMax){
		tempJudgeHit = e_JudgeHit_ProbabilityMax;
	}
	else if(tempJudgeHit<e_JudgeHit_ProbabilityMin){
		tempJudgeHit = e_JudgeHit_ProbabilityMin;
	}

	if(random >= 0 && random<tempJudgeHit){
			
			//スキル威力の補正
			int tempAttackParameter = (int)((double)Actor.updatebattleproperty.OffensivePower_Magic 
				* (double)Actor.tempbattleproperty.tempSkillParameter/10); 

			//ダメージ計算式　超重要
			Terget.tempbattleproperty.Parameter = (tempAttackParameter
				- (int)((double)Terget.updatebattleproperty.DefenseForce_Magic/2));

			if(Terget.tempbattleproperty.Parameter < 0){
				Terget.tempbattleproperty.Parameter = 0;
			}

			Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Hit;
	}
	else{
		Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Miss;
		Terget.tempbattleproperty.Parameter = 0;
	}

	//HPパラメータのみ表示
	Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

//スキルで回復
void ActionRecover::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;
	//死亡時は回復しない
	if(Terget.battleproperty.Status[e_BadStatus_Dead] != true){
		
		if(Actor.tempbattleproperty.tempSkillParameter == RECOVERVALUE_MAX){
			Terget.tempbattleproperty.Parameter = -Terget.updatebattleproperty.HP_max_real;
		}
		else{	
			Terget.tempbattleproperty.Parameter = -(int)((double)Actor.battleproperty.Mg*RECOVER_CONST_A
			* (double)Actor.tempbattleproperty.tempSkillParameter/10);
		}

		//ステータス回復を実施
		if(Actor.tempbattleproperty.tempSkillParameter2 == ADD_RECOVERSTATUS){
			Terget.battleproperty.Status[e_BadStatus_Poison] = false;
			Terget.battleproperty.Status[e_BadStatus_Paralysis] = false;
			Terget.battleproperty.Status[e_BadStatus_Confusion] = false;				
		}
	}
	else{
		Terget.tempbattleproperty.Parameter = 0;
	}

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Hit;

	//HPパラメータのみ表示
	Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;

}

//アイテムで回復
void ActionRecoverItem::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	//死亡時は回復しない
	if(Terget.battleproperty.Status[e_BadStatus_Dead] != true){
		switch (Actor.tempbattleproperty.tempSkillParameter){
			case eItemRecover_HP_S:
				Terget.tempbattleproperty.Parameter = -50;
				Terget.tempbattleproperty.Parameter_MP = 0;
				//HPパラメータのみ表示
				Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;

				break;
			case eItemRecover_HP_M:
				Terget.tempbattleproperty.Parameter = -200;
				Terget.tempbattleproperty.Parameter_MP = 0;
				//HPパラメータのみ表示
				Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;

				break;
			case eItemRecover_HP_L:
				Terget.tempbattleproperty.Parameter = -Terget.updatebattleproperty.HP_max_real;
				Terget.tempbattleproperty.Parameter_MP = 0;
				//HPパラメータのみ表示
				Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;

				break;
			case eItemRecover_MP_S:
				Terget.tempbattleproperty.Parameter = 0;
				Terget.tempbattleproperty.Parameter_MP = -50;
				//MPパラメータのみ表示
				Terget.tempbattleproperty.DisplayMode = eOnlyDisplayMP;

				break;
			case eItemRecover_MP_M:
				Terget.tempbattleproperty.Parameter = 0;
				Terget.tempbattleproperty.Parameter_MP = -200;
				//MPパラメータのみ表示
				Terget.tempbattleproperty.DisplayMode = eOnlyDisplayMP;

				break;
			case eItemRecover_MP_L:
				Terget.tempbattleproperty.Parameter = 0;
				Terget.tempbattleproperty.Parameter_MP = -Terget.updatebattleproperty.MP_max_real;
				//MPパラメータのみ表示
				Terget.tempbattleproperty.DisplayMode = eOnlyDisplayMP;

				break;
			case eItemRecover_HPMP_MAX:
				Terget.tempbattleproperty.Parameter = -Terget.updatebattleproperty.HP_max_real;
				Terget.tempbattleproperty.Parameter_MP = -Terget.updatebattleproperty.MP_max_real;
				//HP,MPパラメータ表示
				Terget.tempbattleproperty.DisplayMode = eDisplayHP_MP;

				break;
			default:
				break;
		}
	}
	else{
		Terget.tempbattleproperty.Parameter = 0;
		Terget.tempbattleproperty.Parameter_MP = 0;
	}

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Hit;

	return;
}

void ActionGuard::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	Actor.tempbattleproperty.tempGuard = e_tempGuard_ON;
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

void ActionAssist::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	Actor.tempbattleproperty.tempGuard = e_tempGuard_ON;
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

//即死回避
void ActionDeathGuard::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	Terget.tempbattleproperty.tempDeathGuard = e_tempGuard_ON;
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;
	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_BadStatus;

	return;
}

//魔法反射
void ActionMgReflect::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	Terget.tempbattleproperty.tempMgReflect = e_tempGuard_ON;
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;
	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_BadStatus;

	return;
}

void ActionStatusDamage::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	if(Actor.NextAction == NextAction_StatusDamage){
		//HP=　1以上でダメージあり
		if(Terget.battleproperty.HP - (int)((double)Terget.updatebattleproperty.HP_max_real/10) <= 0){
			Terget.tempbattleproperty.Parameter = Terget.battleproperty.HP -1;
		}
		else if(Terget.battleproperty.HP == 1){
			Terget.tempbattleproperty.Parameter = 0;
		}
		else{
			Terget.tempbattleproperty.Parameter = (int)((double)Terget.updatebattleproperty.HP_max_real/10);
		}
	}

	if(Actor.NextAction == NextAction_AutoRecover){
		Terget.tempbattleproperty.Parameter = -(int)((double)Terget.updatebattleproperty.HP_max_real/10);
	}

	//麻痺状態でも実行したフラグ
	Actor.tempbattleproperty.flag_StatusDamage_Bind = true;

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Hit;
	//HPパラメータのみ表示
	Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

void ActionBadStatusAttack::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	DWORD time;
	int random = 0;
	int tempJudgeHit = 0;
	time = timeGetTime();

	//乱数の引数は時間(msec)。
	random = system.getRandomNumber((int)time)%e_JudgeHit_ProbabilityMax;

	tempJudgeHit = (int)((double)(Actor.updatebattleproperty.Dexterity)/(double)(Terget.updatebattleproperty.Evasion)/2*PAERCENTAGEMAX);

	if(tempJudgeHit>e_JudgeHit_ProbabilityMax){
		tempJudgeHit = e_JudgeHit_ProbabilityMax;
	}
	else if(tempJudgeHit<e_JudgeHit_ProbabilityMin){
		tempJudgeHit = e_JudgeHit_ProbabilityMin;
	}

	if(random >= 0 && random<tempJudgeHit){

		StatusAttack(Actor,Terget);

		Terget.tempbattleproperty.JudgeHit = e_JudgeHit_BadStatus;
	
	}
	else{

		Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Miss;

	}

	Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

void ActionBadStatusRecover::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	//混乱状態になっている場合
	//混乱状態はフラグをtrueにする　（解除時もtrue 通常時に戻った時false）
	if(Actor.tempbattleproperty.tempSkillParameter2 == e_BadStatus_Confusion &&
		Terget.battleproperty.Status[e_BadStatus_Confusion] == true){
			Terget.tempbattleproperty.flag_Confusion_Go_Lift = true;
			Terget.tempbattleproperty.ActionCost = 0;
	}

	Terget.battleproperty.Status[Actor.tempbattleproperty.tempSkillParameter2] = false;

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_BadStatus;
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

ActionStatusUpDown::ActionStatusUpDown(){
	charactormanager = CharactorManager::GetInstance();
}

void ActionStatusUpDown::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	charactormanager = CharactorManager::GetInstance();
	//ステータスはfalseにする
	charactormanager->setflag_StatusLimit(false);

	if(Actor.tempbattleproperty.tempSkillParameter != STATUSINITIALIZE){

		int tempKind = 0;
		int tempVariation = 0;

		if(Actor.tempbattleproperty.tempSkillParameter < STATUSDOWNSKILLNUMBER){
			tempKind = Actor.tempbattleproperty.tempSkillParameter;
			tempVariation = 1;
		}
		else{
			tempKind = Actor.tempbattleproperty.tempSkillParameter - STATUSDOWNSKILLNUMBER;
			tempVariation = -1;
		}

		if(STATUSUPDOWN_ALL != tempKind){
			if(Terget.tempbattleproperty.tempLevelParameter[tempKind] < UP_LIMIT
				&& Terget.tempbattleproperty.tempLevelParameter[tempKind] > DOWN_LIMIT){
				Terget.tempbattleproperty.tempLevelParameter[tempKind] =
					Terget.tempbattleproperty.tempLevelParameter[tempKind] + tempVariation;
			}
			else{
				charactormanager->setflag_StatusLimit(true);//ステータスが限界でtrue			
			}
		}
		else{

			int CountLimit = 0;

			for(int i = 0; i<STATUSKINDNUMBER; i++){
				if(Terget.tempbattleproperty.tempLevelParameter[i] < UP_LIMIT
					&& Terget.tempbattleproperty.tempLevelParameter[i] > DOWN_LIMIT){
					Terget.tempbattleproperty.tempLevelParameter[i] =
						Terget.tempbattleproperty.tempLevelParameter[i] + tempVariation;
				}
				else{
					CountLimit++;
				}

			}

			if(STATUSKINDNUMBER == CountLimit){
				charactormanager->setflag_StatusLimit(true);//ステータスが上がればtrue
			}
		}
	}
	else{
		//ステータス初期化
		for(int i = 0; i<STATUSKINDNUMBER; i++){
			Terget.tempbattleproperty.tempLevelParameter[i] = 0;
		}

	}

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_BadStatus;

	//パラメータは変動しないこと
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

//蘇生
void ActionRevaival::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	(*pflag_state) = 2;

	//死亡時に回復
	if(Terget.battleproperty.Status[e_BadStatus_Dead] == true){

		if(RECOVERVALUE_MAX == Actor.tempbattleproperty.tempSkillParameter){
			//全快して復活
			Terget.tempbattleproperty.Parameter = -(int)((double)Terget.updatebattleproperty.HP_max_real);
		}
		else{
			//半回復して復活
			Terget.tempbattleproperty.Parameter = -(int)((double)Terget.updatebattleproperty.HP_max_real/2);
		}
		Terget.battleproperty.Status[e_BadStatus_Dead] = false;
	}
	else{
		Terget.tempbattleproperty.Parameter = 0;
	}

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_Hit;
	//HPパラメータのみ表示
	Terget.tempbattleproperty.DisplayMode = eOnlyDisplayHP;
	Terget.tempbattleproperty.Parameter_MP = 0;

	return;
}

void ActionChangeAttribute::RoleAction(int *pflag_state,Charactor *Actor,Charactor *Terget){

	int i = 0;
	int sampleNum = 0;
	int ChangePoint[2] = {0};
	int tempPoint[TOTALATTRIBUTENUMBER-3] = {0};
	DWORD time;

	time = timeGetTime();

	//万能属性以外を反射属性に変更
	for(i = 0; i<TOTALATTRIBUTENUMBER-1 ; i++){
		Terget.battleproperty.GuardAttribute[i] = e_AttributeCounter;
	}

	ChangePoint[0] = system.getRandomNumber((int)time)%(TOTALATTRIBUTENUMBER-3);

	sampleNum = 0;

	for(i = 0; i<TOTALATTRIBUTENUMBER-3 ; i++){
		if(ChangePoint[0] != i){
			tempPoint[sampleNum] = i;
			sampleNum++;
		}
	}

	ChangePoint[1] = tempPoint[system.getRandomNumber((int)time)%(sampleNum-1)];

	Terget.battleproperty.GuardAttribute[ChangePoint[0]] = e_AttributeWeak;
	Terget.battleproperty.GuardAttribute[ChangePoint[1]] = e_AttributeWeak;

	Terget.tempbattleproperty.JudgeHit = e_JudgeHit_BadStatus;
	//パラメータは変動しないこと
	Terget.tempbattleproperty.Parameter = 0;
	Terget.tempbattleproperty.Parameter_MP = 0;

	(*pflag_state) = 2;

	return;
}
*/