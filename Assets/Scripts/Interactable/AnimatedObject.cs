using UnityEngine;

public class AnimatedObject : MonoBehaviour, Interactable {
	public AudioClip[] audioClips;
	public string animationState;

	private Animator animator;
	private AudioSource audio;

	private void Awake() {
		animator = GetComponent<Animator>();
		audio = GetComponent<AudioSource>();
	}

	public void Interact() {
		animator.Play(animationState);
		if(audioClips.Length > 1) {
			audio.clip = audioClips[Random.Range(0, audioClips.Length)];
		}
		audio.Play();
	}
}
