using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityStandardAssets.Characters.ThirdPerson;

public class FlowchartExecution : MonoBehaviour {

  Flowchart flowchart;
  //FirstPersonController fpc;
  ThirdPersonUserControl tpc;
  void Start() {
    flowchart = GetComponent<Flowchart>();
    //Standard AssetsのFPSのコントローラを取得する
    //fpc = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    tpc = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonUserControl>();
  }
  void Update() {
    //実行中のフローチャートがあるかチェックし、あれば移動を不可にする
    if(flowchart){
      if (flowchart.GetExecutingBlocks().Count == 0) {
        //fpc.enabled = true;
        tpc.enabled = true;
      } else {
        //fpc.enabled = false;
        tpc.enabled = false;
      }
    }
  }
}