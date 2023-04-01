using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public Fade fade;
	public Canvas noteCanvas;

	private void Awake() {
		fade.FadeInWithCallback(delegate { });
	}

	public void ShowNote() {
		fade.FadeOutWithCallback(delegate {
			noteCanvas.gameObject.SetActive(true);
			fade.FadeInWithCallback(delegate { });
		});
		
	}

	public void StartGame() {
		fade.FadeOutWithCallback(delegate {
			SceneManager.LoadScene("Home");
		});
	}

	public void Quit() {
		Application.Quit();
	}
}
