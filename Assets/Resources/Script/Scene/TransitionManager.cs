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

		GameObject m_fadeCanvas = null;
		GameObject m_loadingUI = null;

		void Awake(){
			m_fadeCanvas = GameObject.Find("FadeCanvas");
			m_loadingUI = GameObject.Find("loadingUI");
			m_loadingUI.SetActive(false);
			//SceneManager.sceneLoaded += OnSceneLoaded;
		}
/*
		void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode){
			fade.FadeOut(1, ()=>{
				Scene nextScene = SceneManager.GetSceneByName(e_nextGameScene.ToString());
				SceneManager.SetActiveScene(nextScene);
			});			
		}
*/
		public void Fadeout()
		{
			fade = m_fadeCanvas.GetComponent<Fade>();
			m_loadingUI.SetActive(true);
			string ActiveSceneName = SceneManager.GetActiveScene().name;

			fade.FadeIn (1, () =>
			{
				AsyncOperation async_unload = 
				SceneManager.UnloadSceneAsync(ActiveSceneName);
				AsyncState(async_unload);
 
				SceneManager.LoadScene	(e_nextGameScene.ToString(),LoadSceneMode.Additive);				
				
				m_loadingUI.SetActive(false);
				fade.FadeOut(1, ()=>{
					Scene nextScene = SceneManager.GetSceneByName(e_nextGameScene.ToString());
					SceneManager.SetActiveScene(nextScene);
				});
				
			});
		}

		IEnumerator AsyncState(AsyncOperation async) 
		{
			do
			{
				yield return new WaitForEndOfFrame();
			}
    		while ( async.isDone == false );
		}

		public GameScenes nextGameScene{
			set{
				e_nextGameScene = value;
			}
		}
	}
}