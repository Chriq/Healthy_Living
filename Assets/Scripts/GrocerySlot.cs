using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrocerySlot : MonoBehaviour {
	public GameObject checkbox;
	public GameObject checkmark;
	public TextMeshProUGUI text;

	private void Awake() {
		checkmark.SetActive(false);
		gameObject.SetActive(false);
	}
}
