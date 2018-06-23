using System.IO;
using UnityEngine;

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

//キャラクターと敵の抽象クラス
public class BattleActor:MonoBehaviour{

    public readonly int STATUS_MAXNUMBER = 4;
    public readonly int ALLPARAMETER = 99;
    public readonly string NAME_AUTORECOVER_ON ="シュブ＝ニグラス";
    public readonly string NAME_CHANGEATTRIBUTE_ON="ニャルラトホテプ";

    protected BattlerObject[] mBattlerObject;
    protected SkillDataSingleton mSkillDataSingleton;

    void Awake()
    {
        mSkillDataSingleton
        = gameObject.GetComponent<SkillDataSingleton>();
    }

    public BattlerObject getBattlerObject(int number){
        BattlerObject result = null;

        if(mBattlerObject != null){
            if(number <= mBattlerObject.Length){
                result = mBattlerObject[number];
            }
        }
        return mBattlerObject[number];
    }

    public void Initialize_BattleParameter(ref BattlerObject battler){

        int i = 0;

        battler.tempbattleproperty.ActionState = 99;
        battler.tempbattleproperty.ActionProperty = 0;
        battler.tempbattleproperty.ActionAttackAttribute = 0;	//初期の攻撃属性は物理
        battler.tempbattleproperty.ActionCost = 0;			//消費
        battler.tempbattleproperty.JudgeHit = 0;				//攻撃ヒット率0
        battler.tempbattleproperty.pre_MP = battler.battleproperty.MP;
        battler.tempbattleproperty.tempGuard = (int)e_tempGuard.OFF;
        battler.tempbattleproperty.tempSkillParameter = -1;
        battler.tempbattleproperty.tempSkillParameter2 = -1;
        battler.tempbattleproperty.tempDeathGuard = (int)e_tempGuard.OFF;
        battler.tempbattleproperty.flag_Confusion_Go_Lift = false;
        battler.tempbattleproperty.flag_StatusDamage_Bind = false;
        battler.tempbattleproperty.tempMgReflect = (int)e_tempGuard.OFF;

        //敵の特殊状態（回復状態）
        if(battler.Name == NAME_AUTORECOVER_ON){
            battler.tempbattleproperty.flag_AutoRecover = true;
            battler.tempbattleproperty.flag_ChangeAttritute = false;
        }
        //敵の特殊状態（属性変化）
        else if(battler.Name == NAME_CHANGEATTRIBUTE_ON){
            battler.tempbattleproperty.flag_AutoRecover = false;
            battler.tempbattleproperty.flag_ChangeAttritute = true;
        }
        else{
            battler.tempbattleproperty.flag_AutoRecover = false;
            battler.tempbattleproperty.flag_ChangeAttritute = false;
        }

        if(battler.tempbattleproperty.tempLevelParameter != null){
            for(i = 0; i< 3; i++){
                battler.tempbattleproperty.tempLevelParameter[i] = 0;
            }
        }

        return;
    }

    public void Update_Parameter(ref BattlerObject battler){

        int i = 0;
        int[] tempIndex = battler.skillIndex;
        float tempHP_UP = 1.0f;//HP増加率
        float tempMP_UP = 1.0f;//MP増加率
        float tempAt_UP = 1.0f;//HP増加率
        float tempGu_UP = 1.0f;//MP増加率
        float tempMg_UP = 1.0f;//HP増加率
        float tempDx_UP = 1.0f;//MP増加率
        float tempEv_UP = 1.0f;//HP増加率
        
        foreach(int number in tempIndex){
            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_HP_UP){
                //10%単位で増加する
                tempHP_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_MP_UP){
                //10%単位で増加する
                tempMP_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_At_UP){
                //10%単位で増加する
                tempAt_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_Gu_UP){
                //10%単位で増加する
                tempGu_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_Mg_UP){
                //10%単位で増加する
                tempMg_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_Sp_UP){
                //10%単位で増加する
                tempDx_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_Av_UP){
                //10%単位で増加する
                tempEv_UP += 0.1f * (float)mSkillDataSingleton.GetPassiveSkillAsstisPara(number);
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_DamageCounter){
                //反射スキル
                battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                GetPassiveSkillAsstisPara(number)]= (int)e_AttributeResponsePattern.Counter;
            }

            if(battler.battleproperty.GuardAttribute.Length != 0){

                if(battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                GetPassiveSkillAsstisPara(number)] != (int)e_AttributeResponsePattern.Counter){
                    //キャラに反射属性がない場合

                    if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_DamageVoid){
                        //無効スキル
                        battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                        GetPassiveSkillAsstisPara(number)]= (int)e_AttributeResponsePattern.Void;
                    }

                    if(battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                    GetPassiveSkillAsstisPara(number)] != (int)e_AttributeResponsePattern.Void){
                        //キャラに無効属性がない場合
                        if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_DamageHalf){
                            //半減スキル
                            battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                            GetPassiveSkillAsstisPara(number)]= (int)e_AttributeResponsePattern.Half;
                        }

                        if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_DamageWeak
                            && battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                            GetPassiveSkillAsstisPara(number)] != (int)e_AttributeResponsePattern.Half){
                            //半減スキルがない場合、弱点スキル
                            battler.battleproperty.GuardAttribute[mSkillDataSingleton.
                            GetPassiveSkillAsstisPara(number)]= (int)e_AttributeResponsePattern.Weak;
                        }

                    }
                }
            }

            if(mSkillDataSingleton.GetPassiveSkillProperty(number) == (int)ActProperty.Passive_BadStatusVoid){
                //耐ステータス異常
                if(mSkillDataSingleton.GetPassiveSkillAsstisPara(number) == ALLPARAMETER){
                    
                    for(i = 0; i<STATUS_MAXNUMBER; i++){
                        battler.battleproperty.VoidBadStatus[i] = (int)e_AttributeResponsePattern.Void;
                    }
                }
                else{
                    battler.battleproperty.VoidBadStatus[mSkillDataSingleton.
                    GetPassiveSkillAsstisPara(number)] = (int)e_AttributeResponsePattern.Void;
                }
            }
        }

        battler.updatebattleproperty.HP_max_real
        = (int)((float)battler.battleproperty.HP_max * tempHP_UP);
        battler.updatebattleproperty.MP_max_real 
        = (int)((float)battler.battleproperty.MP_max * tempMP_UP);

        //攻撃力
        battler.updatebattleproperty.OffensivePower = (int)((float)e_AttackConst.At_A 
            * (float)battler.battleproperty.At * tempAt_UP);

        //魔法攻撃力
        battler.updatebattleproperty.OffensivePower_Magic = (int)((float)e_AttackConst.Mg_A 
            * (float)battler.battleproperty.Mg * tempMg_UP);

        //防御力
        battler.updatebattleproperty.DefenseForce = (int)((float)e_AttackConst.Df_A 
            * (float)battler.battleproperty.Df * tempGu_UP);

        //魔法防御力
        battler.updatebattleproperty.DefenseForce_Magic = (int)((float)e_AttackConst.DfMg_A 
            * (float)battler.battleproperty.Mg * tempGu_UP);

        //回避率、命中率の計算
        battler.updatebattleproperty.Dexterity = (int)(((float)e_HitRateConst.Dexterity_A 
            + (float)battler.battleproperty.Sp/(float)e_HitRateConst.Dexterity_B
            + (float)battler.battleproperty.Lc/(float)e_HitRateConst.Dexterity_C) * tempDx_UP);

        battler.updatebattleproperty.Evasion = (int)(((float)e_HitRateConst.Evasion_A 
            + (float)battler.battleproperty.Sp/(float)e_HitRateConst.Evasion_B
            + (float)battler.battleproperty.Lc/(float)e_HitRateConst.Evasion_C) * tempEv_UP);

        if(battler.tempbattleproperty.tempLevelParameter != null){
            //パラメータの補正(戦闘中以外は補正値0)
            battler.updatebattleproperty.OffensivePower = 
                (int)((float)battler.updatebattleproperty.OffensivePower 
                * (1 + 0.2f*(float)battler.tempbattleproperty.tempLevelParameter[(int)e_tempLevelParameter.Power]));

            battler.updatebattleproperty.OffensivePower_Magic = 
                (int)((float)battler.updatebattleproperty.OffensivePower_Magic
                * (1 + 0.2f*(float)battler.tempbattleproperty.tempLevelParameter[(int)e_tempLevelParameter.Power]));

            battler.updatebattleproperty.DefenseForce = 
                (int)((float)battler.updatebattleproperty.DefenseForce 
                * (1 + 0.2f*(float)battler.tempbattleproperty.tempLevelParameter[(int)e_tempLevelParameter.Guard]));

            battler.updatebattleproperty.DefenseForce_Magic = 
                (int)((float)battler.updatebattleproperty.DefenseForce_Magic
                * (1 + 0.2f*(float)battler.tempbattleproperty.tempLevelParameter[(int)e_tempLevelParameter.Guard]));

            battler.updatebattleproperty.Dexterity = 
                (int)((float)battler.updatebattleproperty.Dexterity 
                * (1 + 0.2f*(float)battler.tempbattleproperty.tempLevelParameter[(int)e_tempLevelParameter.Speed]));

            battler.updatebattleproperty.Evasion = 
                (int)((float)battler.updatebattleproperty.Evasion
                * (1 + 0.2f*(float)battler.tempbattleproperty.tempLevelParameter[(int)e_tempLevelParameter.Speed]));
        }
        //防御力、魔法防御力だけ別関数（プレイヤーキャラは防御コマンド選択した場合、２倍になる）
        if(battler.tempbattleproperty.ActionProperty == (int)ActProperty.Guard){
            battler.updatebattleproperty.DefenseForce = battler.updatebattleproperty.DefenseForce*2;
	        battler.updatebattleproperty.DefenseForce_Magic = battler.updatebattleproperty.DefenseForce_Magic*2;

        }

        return;
    }            
}

public class CharacterDataSingleton:BattleActor{

    private CharacterObject[] mCharacterObject;

    ClassesDataSingleton mClassesDataSingleton;
 
    void Awake()
    {
        FileRead_CharacterData();
        mClassesDataSingleton 
        = gameObject.GetComponent<ClassesDataSingleton>();
        
        mSkillDataSingleton
        = gameObject.GetComponent<SkillDataSingleton>();
    }

    //jsonファイルを読み取る
    private void FileRead_CharacterData() {

        string fileName = "Actors";
        TextAsset txt = Instantiate(Resources.Load("data/" + fileName)) as TextAsset;
        string jsonText = txt.text;

       mCharacterObject = LitJson.JsonMapper.ToObject<CharacterObject[]>(jsonText);
    }

    public void SetBattleCharacterObject()
    {
        learningsObject[] lerningsObjects = null;

        mBattlerObject = new BattlerObject[mCharacterObject.Length];
        
        for (int j=1; j< mCharacterObject.Length; j++) {
            mBattlerObject[j] = new BattlerObject();
            mBattlerObject[j].battleproperty = Resources.Load<BattleProperty> ("data/Character"+j.ToString());

            lerningsObjects = mClassesDataSingleton.getLearningObject(1);

            mBattlerObject[j].skillIndex
                = new int[lerningsObjects.Length];

            
            int i = 0;
            
            foreach (learningsObject learningObject in lerningsObjects) {
                mBattlerObject[j].skillIndex[i]
                    = learningObject.skillId;
                i++;
            }

            Update_Parameter(ref mBattlerObject[j]);
            Initialize_BattleParameter(ref mBattlerObject[j]);
        }
    }

    public int[] GetSkillIndex(int characterId)
    {
        int[] result = null;

        if (characterId>0 && characterId<=mBattlerObject.Length) {
            result = mBattlerObject[characterId].skillIndex;
        }

        return result;
    }

    public eTergetScope GetSkillScope(int skillId)
    {   
        return (eTergetScope)mSkillDataSingleton.GetSkillScope(skillId);
    }

    public int CharaSpeed(int characterNum)
    {
        return mBattlerObject[characterNum].battleproperty.Sp;
    }


}
