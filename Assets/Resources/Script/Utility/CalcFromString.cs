using Microsoft.JScript;
using Microsoft.Vsa;
using RPGEngine;

namespace RPGEngine.system{
	public class OperateString{

		public string Trans_a_atk(string formula,int a_atk){
			return formula.Replace("a.atk",a_atk.ToString());
		}

		public string Trans_b_def(string formula,int b_def){
			return formula.Replace("b.def",b_def.ToString());
		}

		public double Calcformula (string formula) {
			Microsoft.JScript.Vsa.VsaEngine ve =
				Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
			return (int)Microsoft.JScript.Eval.JScriptEvaluate(formula,ve);
		}
	}
}