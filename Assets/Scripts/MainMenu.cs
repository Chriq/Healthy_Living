using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public void StartGame() {
		SceneManager.LoadScene("Main");
	}

	public void Quit() {
		Application.Quit();
	}
}
