using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour, Interactable {
	[SerializeField] private SpriteRenderer slot1;
	[SerializeField] private SpriteRenderer slot2;
	[SerializeField] private SpriteRenderer slot3;
	[SerializeField] private SpriteRenderer slot4;

	public int distance = 1;
	public bool isEmpty = true;

	private Food foodOnShelf;

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
		GameManager.Instance.ConsumeFood(foodOnShelf);
	}
}
