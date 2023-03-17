using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {
	public float speed;
	public Animator animator;

	private bool isMoving;
	private Vector2 input;

	private void Update() {
		input = Vector2.zero;

		if (!isMoving) {
			if(Input.GetKey(KeyCode.W)) {
				input = Vector2.up;
			} 
			else if(Input.GetKey(KeyCode.A)) {
				input = Vector2.left;
			}
			else if(Input.GetKey(KeyCode.S)) {
				input = Vector2.down;
			}
			else if(Input.GetKey(KeyCode.D)) {
				input = Vector2.right;
			} 

			if(input != Vector2.zero) {
				Vector3 targetPos = transform.position;
				targetPos.x += input.x;
				targetPos.y += input.y;

				animator.SetFloat("horizontal", input.x);
				animator.SetFloat("vertical", input.y);

				StartCoroutine(MoveToCell(targetPos));
			}
		}

		
		animator.SetBool("isMoving", isMoving);
	}

	IEnumerator MoveToCell(Vector3 targetPos) {
		isMoving = true;
		while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
			transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
			yield return null;
		}

		transform.position = targetPos;
		isMoving = false;
	}
}
