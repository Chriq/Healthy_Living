using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour {
	[SerializeField] private SpriteRenderer slot1;
	[SerializeField] private SpriteRenderer slot2;
	[SerializeField] private SpriteRenderer slot3;
	[SerializeField] private SpriteRenderer slot4;

	[SerializeField] private int distance = 1;

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
	}
}
