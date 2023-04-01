using UnityEngine;

public class Shelf : MonoBehaviour, Interactable {
	[SerializeField] private SpriteRenderer slot1;
	[SerializeField] private SpriteRenderer slot2;
	[SerializeField] private SpriteRenderer slot3;
	[SerializeField] private SpriteRenderer slot4;

	[SerializeField] private AudioClip[] audio;
	private AudioSource audioSource;

	public int distance = 1;
	public bool isEmpty = true;

	private Food foodOnShelf;

	private void Awake() {
		audioSource = GetComponent<AudioSource>();
	}

	private void FillShelves(Sprite sprite) {
		slot1.sprite = sprite;
		slot2.sprite = sprite;
		slot3.sprite = sprite;
		slot4.sprite = sprite;
	}

	public Food getFoodOnShelf() {
		return foodOnShelf;
	}

	public void setFoodOnShelf(Food food) {
		foodOnShelf = food;
		FillShelves(food.sprite);
		isEmpty = false;
	}

	public void Interact() {
		if(GameManager.Instance.ConsumeFood(foodOnShelf)) {
			audioSource.clip = audio[Random.Range(0, audio.Length)];
			audioSource.Play();
		}
	}
}
