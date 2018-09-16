using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameScenes
{
    Battle,
    BattleResult,
	Map
}

public class TransitionManager : MonoBehaviour 
{
	[SerializeField]
	Fade fade = null;

	private static GameScenes _currentGameScene = GameScenes.Map;
	private static GameScenes _nextGameScene = GameScenes.Battle;

	public void Fadeout()
	{
		GameObject fadeCanvas = GameObject.Find("FadeCanvas");
		fade = fadeCanvas.GetComponent<Fade>();

		fade.FadeIn (1, () =>
		{
			SceneManager.LoadSceneAsync(_nextGameScene.ToString(),LoadSceneMode.Additive);
			SceneManager.UnloadSceneAsync(_currentGameScene.ToString());

			GameScenes tempScene = _currentGameScene;
			_currentGameScene = _nextGameScene;
			_nextGameScene = tempScene;

			fade.FadeOut(1, ()=>{
			});
			
		});
	}
}
