using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	public GameObject target;
	//private Vector3 velocity = Vector3.zero;

	private void LateUpdate() {
		transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
		//transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, -10), ref velocity, 0.001f);
	}
}
