/* 
using System;
using System.Collections;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// シーン遷移を管理する
/// </summary>
public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
{
	/// <summary>
	/// 蓋絵（トランジションアニメーションの管理コンポーネント）
	/// Easy Masking Transitionを利用しない場合は自作して下さい
	/// </summary>
	private EMTransition _transitionComponent;

	/// <summary>
	/// 蓋絵のImage
	/// </summary>
	private RawImage _image;

	/// <summary>
	/// シーン遷移処理を実行中であるか
	/// </summary>
	private bool _isRunning = false;

	public bool IsRunning { get { return _isRunning; } }

	/// <summary>
	/// トランジションアニメーションを終了させてよいか
	/// (蓋絵が開くアニメーションを再生してよいか)
	/// </summary>
	private ReactiveProperty<bool> CanEndTransition = new ReactiveProperty<bool>(false);

	private GameScenes _currentGameScene;

	/// <summary>
	/// 現在のシーン情報
	/// </summary>
	public GameScenes CurrentGameScene
	{
		get { return _currentGameScene; }
	}


	/// <summary>
	/// トランジションのアニメーションの終了通知
	/// (蓋絵が開き切ったり、閉じきったことを通知する)
	/// </summary>
	private Subject<Unit> _onTransactionFinishedInternal = new Subject<Unit>();

	/// <summary>
	/// トランジションが終了しシーンが開始したことを通知する
	/// </summary>
	private Subject<Unit> _onTransitionAnimationFinishedSubject = new Subject<Unit>();

	private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();

	/// <summary>
	/// 全シーンのロードが完了したことを通知する
	/// </summary>
	public IObservable<Unit> OnScenesLoaded { get { return onAllSceneLoaded; } }

	/// <summary>
	/// トランジションが終了し、シーンが開始したことを通知する
	/// OnCompletedもセットで発行する
	/// </summary>
	public IObservable<Unit> OnTransitionAnimationFinished
	{
		get
		{
			if (_isRunning)
			{
				return _onTransitionAnimationFinishedSubject.FirstOrDefault();
			}
			else
			{
				//シーン遷移を実行していないら即値を返却
				return Observable.Return(Unit.Default);
			}
		}
	}


	/// <summary>
	/// トランジションアニメーションを終了させる
	/// （AutoMove=falseを指定した際に呼び出す必要がある)
	/// </summary>
	public void Open()
	{
		CanEndTransition.Value = true;
	}

	private void Awake()
	{
		//勝手に消さない
		DontDestroyOnLoad(gameObject);

		try
		{
			//現在のシーンを取得する
			_currentGameScene =
				(GameScenes)Enum.Parse(typeof(GameScenes), SceneManager.GetActiveScene().name, false);
		}
		catch
		{
			Debug.Log("現在のシーンの取得に失敗");
			_currentGameScene = GameScenes.TitleScene; //Debugシーンとかの場合は適当なシーンで埋めておく
		}
	}

	private void Start()
	{
		Initialize();

		//トランジションの終了を待機してゲームを開始するような設定の場合を想定して
		//初期化直後にシーン遷移完了通知を発行する(デバッグで任意のシーンからゲームを開始できるように)
		onAllSceneLoaded.OnNext(Unit.Default);
	}

	/// <summary>
	/// 初期化
	/// </summary>
	private void Initialize()
	{
		if (_transitionComponent == null)
		{
			_transitionComponent = GetComponent<EMTransition>();
			_image = GetComponent<RawImage>();
			_image.raycastTarget = false; //タッチイベントを蓋絵でブロクしない

			//この辺はEMTの設定
			//アニメーションが終わったら自動的に反転する
			_transitionComponent.flipAfterAnimation = true;
			//トランジションアニメーションが終了したイベントをObservableに変換する
			_transitionComponent.onTransitionComplete.AddListener(
				() => _onTransactionFinishedInternal.OnNext(Unit.Default));
		}
	}

	/// <summary>
	/// シーン遷移を実行する
	/// </summary>
	/// <param name="nextScene">次のシーン</param>
	/// <param name="data">次のシーンへ引き継ぐデータ</param>
	/// <param name="additiveLoadScenes">追加ロードするシーン</param>
	/// <param name="autoMove">トランジションの自動遷移を行うか</param>
	public void StartTransaction(
		GameScenes nextScene,
		SceneDataPack data,
		GameScenes[] additiveLoadScenes,
		bool autoMove
		)
	{
		if (_isRunning) return;
		StartCoroutine(TransitionCoroutine(nextScene, data, additiveLoadScenes, autoMove));
	}

	/// <summary>
	/// シーン遷移処理の本体
	/// </summary>
	private IEnumerator TransitionCoroutine(
		GameScenes nextScene,
		SceneDataPack data,
		GameScenes[] additiveLoadScenes,
		bool autoMove
		)
	{
		//処理開始フラグセット
		_isRunning = true;

		//トランジションの自動遷移設定
		CanEndTransition.Value = autoMove;

		if (_transitionComponent == null)
		{
			//初期化できてなかったらここで初期化する
			Initialize();
			yield return null;
		}

		//蓋絵でuGUIのタッチイベントをブロックする
		_image.raycastTarget = true;

		//トランジション開始（蓋絵で画面を隠す）
		_transitionComponent.flip = false;
		_transitionComponent.ignoreTimeScale = true;
		_transitionComponent.Play();

		//トランジションアニメーションが終了するのを待つ
		yield return _onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();

		//前のシーンから受け取った情報を登録
		SceneLoader.PreviousSceneData = data;

		//メインとなるシーンをSingleで読み込む
		yield return SceneManager.LoadSceneAsync(nextScene.ToString(), LoadSceneMode.Single);

		//追加シーンがある場合は一緒に読み込む
		if (additiveLoadScenes != null)
		{
			yield return additiveLoadScenes.Select(scene =>
				SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive)
				.AsObservable()).WhenAll().ToYieldInstruction();
		}
		yield return null;

		//使ってないリソースの解放とGCを実行
		Resources.UnloadUnusedAssets();
		GC.Collect();

		yield return null;

		//現在シーンを設定
		_currentGameScene = nextScene;

		//シーンロードの完了通知を発行
		onAllSceneLoaded.OnNext(Unit.Default);

		if (!autoMove)
		{
			//自動遷移しない設定の場合はフラグがtrueに変化するまで待機
			yield return CanEndTransition.FirstOrDefault(x => x).ToYieldInstruction();
		}
		CanEndTransition.Value = false;


		//蓋絵を開く方のアニメーション開始
		_transitionComponent.Play();

		//蓋絵が開ききるのを待つ
		yield return _onTransactionFinishedInternal.FirstOrDefault().ToYieldInstruction();

		//蓋絵のイベントブロックを解除
		_image.raycastTarget = false;

		//トランジションが全て完了したことを通知
		_onTransitionAnimationFinishedSubject.OnNext(Unit.Default);

		//終了
		_isRunning = false;
	}
}
*/