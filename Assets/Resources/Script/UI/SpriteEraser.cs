using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEraser : MonoBehaviour {
    SpriteRenderer spRenderer;

    public void Erase()
    {
        spRenderer = this.GetComponent<SpriteRenderer>();

        // スプライトを半透明にします。
		Color temp = spRenderer.color;
		temp.a = 1.0f;
        spRenderer.color = temp;

        // 値を 0.5 から 1.0 へ、1秒かけて変化させます。
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", 1.0f
            , "to", 0.0f
            , "time", 1.0f
            , "onupdate", "SetAlpha"  // 毎フレーム SetAlpha() を呼びます。
            ));
    }

    private void SetAlpha(float value)
    {
        // 受け取った値を、スプライトのアルファ値として代入します。
		Color temp = spRenderer.color;
		temp.a = value;
        spRenderer.color = temp;
    }
}