using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour, Interactable {
	public string sceneName;

	private void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void Interact() {
		GameManager.Instance.fade.FadeOutWithCallback(delegate {
			SceneManager.LoadScene(sceneName);
		});
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		GameManager.Instance.fade.FadeInWithCallback(delegate { });
	}
}
