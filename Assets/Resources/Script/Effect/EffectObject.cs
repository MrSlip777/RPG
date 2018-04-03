using System;
using UniRx;
using UnityEngine;

/// <summary>
/// エフェクトを再生して一定時間後に通知する
/// </summary>
public class EffectObject : MonoBehaviour
{
	private EffekseerEmitter _effectEmitter;
	private EffekseerEmitter Emitter
	{
		get
		{
			//遅延初期化に変更
			return _effectEmitter ?? (_effectEmitter = GetComponent<EffekseerEmitter>());
		}
	}

	/// <summary>
	/// エフェクトを再生する
	/// </summary>
	/// <param name="position">再生する座標</param>
	/// <returns>再生終了通知</returns>
	public IObservable<Unit> PlayEffect(Vector3 position)
	{
		transform.position = position;

		//エフェクトを再生
		Emitter.Play();

		//1秒後にエフェクトを止めて終了通知
		return Observable.Timer(TimeSpan.FromSeconds(1.0f))
			.ForEachAsync(_ => Emitter.Stop());
	}
}
