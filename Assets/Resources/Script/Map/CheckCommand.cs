using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class CheckCommand : MonoBehaviour {
  [SerializeField]
  float rayLength = 3f;
  Flowchart flowchart;

  void Start() {
    flowchart = FindObjectOfType<Flowchart>();
  }
  // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(KeyCode.Return)) {  //Eキーに調べるコマンドを割り当てる
      Ray ray = new Ray(transform.position, transform.forward);
      RaycastHit hit;
      if (Physics.Raycast(ray, out hit, rayLength)) {
        //衝突したものがBlockTriggerを持っているか調べ、持っていたらそのblockを実行する
        var trigger = hit.transform.GetComponent<BlockTrigger>();
        if (trigger != null) {
          this.TowardPlayer(hit);
          hit.transform.Rotate(transform.rotation.x,transform.rotation.y,transform.rotation.z);
          flowchart.ExecuteBlock(trigger.blockName);
        }
      }
    }
  }

  void TowardPlayer(RaycastHit hit){ 
    var aim = this.transform.position-hit.transform.position;
    var look = Quaternion.LookRotation(aim);
    hit.transform.localRotation = look;
  }
}