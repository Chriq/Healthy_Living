using UnityEngine;

public class Store : MonoBehaviour {
	private void Start() {
		GameManager.Instance.PopulateStoreShelves();
	}
}
