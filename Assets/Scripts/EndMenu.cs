using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndMenu : MonoBehaviour {
	public GameObject mainContainer;
	public GameObject creditsContainer;
	public TextMeshProUGUI buttonText;
	public TextMeshProUGUI daysLasted;

	private void Awake() {
		daysLasted.text = $"You Lasted {GameManager.Instance.day-1} Days";
	}

	public void ToggleCredits() {
		mainContainer.SetActive(!mainContainer.activeSelf);
		creditsContainer.SetActive(!creditsContainer.activeSelf);

		if(mainContainer.activeSelf) {
			buttonText.text = "CREDITS";
		} else {
			buttonText.text = "BACK";
		}
	}

	public void Quit() {
		Application.Quit();
	}
}
