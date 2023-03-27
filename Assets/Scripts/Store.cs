using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Store : MonoBehaviour {
	private void Start() {
		GameManager.Instance.PopulateStoreShelves();
	}
}
