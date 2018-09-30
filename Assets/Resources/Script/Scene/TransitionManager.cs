using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using RPGEngine;
using RPGEngine.system;
using UniRx;

namespace RPGEngine.system{
	public enum GameScenes
	{
		Battle,
		BattleResult,
		Map,
		GameOver,
		Title
	}

	public class TransitionManager : MonoBehaviour 
	{
		[SerializeField]
		Fade fade = null;

		private static GameScenes e_nextGameScene = GameScenes.Map;

		private Subject<Unit> onAllSceneLoaded = new Subject<Unit>();

		public void Fadeout()
		{
			GameObject fadeCanvas = GameObject.Find("FadeCanvas");
			fade = fadeCanvas.GetComponent<Fade>();

			fade.FadeIn (1, () =>
			{
				SceneManager.LoadScene(e_nextGameScene.ToString(),LoadSceneMode.Additive);				
				SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);

				fade.FadeOut(1, ()=>{
					Scene nextScene = SceneManager.GetSceneByName(e_nextGameScene.ToString());
					SceneManager.SetActiveScene(nextScene);
				});
				
			});
		}

		public GameScenes nextGameScene{
			set{
				e_nextGameScene = value;
			}
		}
	}
}