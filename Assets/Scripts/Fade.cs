using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image image;
    public float delaySeconds;

	private static Fade instance;

	private void Awake() {
		DontDestroyOnLoad(this);

		if(instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public void FadeOut() {
		StartCoroutine(FadeCoroutine(Color.clear, Color.black));
	}

	public void FadeIn() {
		StartCoroutine(FadeCoroutine(Color.black, Color.clear));
	}

	public void FadeOutWithCallback(Action action) {
		StartCoroutine(FadeOutCoroutineWithCallback(Color.clear, Color.black, action));
	}

	public void FadeInWithCallback(Action action) {
		StartCoroutine(FadeInCoroutineWithCallback(Color.black, Color.clear, action));
	}

	IEnumerator FadeCoroutine(Color start, Color end) {
		float counter = 0;

		while(counter < delaySeconds) {
			counter += Time.deltaTime;
			image.color = Color.Lerp(start, end, counter / delaySeconds);
			yield return null;
		}
	}

	IEnumerator FadeOutCoroutineWithCallback(Color start, Color end, Action action) {
		image.gameObject.SetActive(true);

		float counter = 0;

		while(counter < delaySeconds) {
			counter += Time.deltaTime;
			image.color = Color.Lerp(start, end, counter / delaySeconds);
			yield return null;
		}

		action.Invoke();
	}

	IEnumerator FadeInCoroutineWithCallback(Color start, Color end, Action action) {
		float counter = 0;

		while(counter < delaySeconds) {
			counter += Time.deltaTime;
			image.color = Color.Lerp(start, end, counter / delaySeconds);
			yield return null;
		}

		action.Invoke();
		image.gameObject.SetActive(false);
	}
}
