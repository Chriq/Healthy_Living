using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour, Interactable {
	public void Interact() {
		GameManager.Instance.GoToBed();
	}
}
