using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour, Interactable {
	public string sceneName;
	public Vector3 playerLocationOnLoad;

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void Interact() {
		GameManager.Instance.fade.FadeOutWithCallback(delegate {
			GameManager.Instance.playerPositionOnLoad = playerLocationOnLoad;
			SceneManager.LoadScene(sceneName);
		});
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if(GameManager.Instance.playerPositionOnLoad != Vector3.zero) {
			GameObject.Find("Player").transform.position = GameManager.Instance.playerPositionOnLoad;
			GameManager.Instance.playerPositionOnLoad = Vector3.zero;
		}
		
		GameManager.Instance.fade.FadeInWithCallback(delegate { });
	}
}
