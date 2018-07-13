using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    private void OnSliderUpdate(float value){
        gameObject.GetComponent<Slider>().value = value;
    }

	//値を変化せるiTween.ValueTOをラップしたメソッド
	public void ValueTransition (float to)
	{
		float from = gameObject.GetComponent<Slider>().value;

		iTween.ValueTo (gameObject, iTween.Hash (
			//変更元の値
			"from", from,
			//変更先の値
			"to", to,
			//時間
			"time", 0.5f,
			//tween開始までの時間
			"delay", 0.0f,
			//tween中に実行するコールバックメソッド(このメソッドに値が渡る)
			"onupdate", "OnSliderUpdate",
			//tweenの値の変化の仕方を定義
			"easetype", iTween.EaseType.easeOutExpo
		));
	}
}
