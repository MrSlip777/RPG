using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGEngine;

namespace RPGEngine.database{
	[CreateAssetMenu( menuName = "MyGame/Create SkillEffectData", fileName = "SkillEffectData" )]
	public class SkillEffectData: ScriptableObject
	{
		public eEffectName frontEffect_base;
		public eEffectName frontEffect_op1;
		public eEffectName frontEffect_op2;	
		public eEffectName frontEffect_op3;
		public eEffectName frontEffect_op4;
		public eEffectName backEffect_base;
		public eEffectName backEffect_op1;
		public eEffectName backEffect_op2;
		public eEffectName backEffect_op3;	
		public eEffectName backEffect_op4;

	} // class ParameterTable
}