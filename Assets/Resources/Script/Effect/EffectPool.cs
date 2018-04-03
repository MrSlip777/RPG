using UniRx.Toolkit;
using UnityEngine;

/// <summary>
/// EffectObjectのPool
/// </summary>
public class EffectPool : ObjectPool<EffectObject>
{
	private readonly EffectObject _prefab;
	private readonly Transform _parenTransform;

	//コンストラクタ
	public EffectPool(Transform parenTransform, EffectObject prefab)
	{
		_parenTransform = parenTransform;
		_prefab = prefab;
	}

	/// <summary>
	/// オブジェクトの追加生成時に実行される
	/// </summary>
	protected override EffectObject CreateInstance()
	{
		//新しく生成
		var e = GameObject.Instantiate(_prefab);

		//ヒエラルキーが散らからないように一箇所にまとめる
		e.transform.SetParent(_parenTransform);

		return e;
	}
}
