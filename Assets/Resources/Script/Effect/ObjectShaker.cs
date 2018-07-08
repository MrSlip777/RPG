using UnityEngine;
using System.Collections;
 
public class ObjectShaker : MonoBehaviour {
 
    public void Shake(GameObject shakeObj){
        iTween.ShakePosition(shakeObj,iTween.Hash("x",0.3f,"y",0.3f,"time",0.5f));
    }
 
}