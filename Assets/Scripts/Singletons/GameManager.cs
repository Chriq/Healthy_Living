using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using System.Collections;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance {
		get {
			
			if (instance == null) {
				if(!(instance = FindObjectOfType<GameManager>())) {
					instance = new GameObject("GameManager").AddComponent<GameManager>();
				}
			}

			return instance; 
		}
	}

	public RefData refData;
	public readonly float maxEnergyTotal = 100;

	public int day = -1;
	public float maxDayEnergy = 100f;
	public int dayStepGoal;
	public float bedTime = 12f;
	public Dictionary<string, int> groceryList = new Dictionary<string, int>();

	public float currentEnergy;
	public int currentSteps;
	public Dictionary<string, int> groceriesPurchased = new Dictionary<string, int>();
	public int extraPurchases = 0;
	public float timeInBed;

	public float currentGameTime;

	Shelf[] dayStoreShelves;

	public Action OnStepTaken;
	public Action OnFoodConsumed;

	public Fade fade;

	private void Awake() {
		dayStoreShelves = new Shelf[0];
		maxDayEnergy = 100f;
		fade = GameObject.Find("Fade").GetComponent<Fade>();
		fade.FadeIn();
		GenerateGroceryList();
		GenerateStepGoal();
		DontDestroyOnLoad(this);
		//PopulateStoreShelves();
	}

	public void GoToBed() {
		IEnumerator coroutine = DayCycle(2f);
		StartCoroutine(coroutine);
	}

	IEnumerator DayCycle(float delay) {
		TimeManager.Instance.PauseTimer();
		fade.FadeOut();
		timeInBed = TimeManager.Instance.GetTimeAsFloat();

		yield return new WaitForSeconds(delay);

		IncrementDay();
		TimeManager.Instance.SetTime(8, 0);
		fade.FadeIn();
		TimeManager.Instance.ResumeTimer();
	}

	public void IncrementDay() {
		ResetDayEnergy();

		if(maxDayEnergy <= 0) {
			SceneManager.LoadScene("End");
		} else {
			GenerateGroceryList();
			GenerateStepGoal();

			ResetPlayerGoals();

			dayStoreShelves = new Shelf[0];
			day++;
		}
	}

	public void ResetDayEnergy() {
		if(groceriesPurchased.Count < groceryList.Count) {
			maxDayEnergy -= groceryList.Count - groceriesPurchased.Count;
		}

		if(currentSteps < dayStepGoal) {
			maxDayEnergy -= dayStepGoal - currentSteps;
		}

		if(timeInBed > bedTime) {
			maxDayEnergy -= Mathf.Ceil(timeInBed - bedTime);
		}

		currentEnergy = maxDayEnergy;
	}

	public void GenerateGroceryList() {
		groceryList.Clear();
		extraPurchases = 0;

		int numItemsTotal = (int) Mathf.Floor(7f * Mathf.Log((float) day) - 3f);
		numItemsTotal = (int) Mathf.Clamp(numItemsTotal, 1f, 30f);

		int numUniqueItems = (int) (2 * Mathf.Log(day));//UnityEngine.Random.Range(1, Mathf.Min(numItemsTotal, 8));
		int itemOffset = UnityEngine.Random.Range(-2, 2);
		numUniqueItems = Mathf.Clamp(numUniqueItems + itemOffset, 1, 8);

		int numPerItem = Mathf.Clamp(numItemsTotal / numUniqueItems, 1, 8);
		
		for(int i = 0; i < numUniqueItems; i++) {
			int index = UnityEngine.Random.Range(0, refData.healthyFood.Count);
			int offset = UnityEngine.Random.Range(-4, 4);
			int amount = Mathf.Clamp(numPerItem + offset, 1, 12);
			bool added = groceryList.TryAdd(refData.healthyFood[index].name, amount);
			for(int j = 0; !added && j < refData.healthyFood.Count; j++) {
				added = groceryList.TryAdd(refData.healthyFood[j].name, amount);
			}
		}

		OnFoodConsumed?.Invoke();
	}

	public void GenerateStepGoal() {
		dayStepGoal = 25 + (30 * (int) Mathf.Log(dayStepGoal));
		OnStepTaken?.Invoke();
	}

	public void ResetPlayerGoals() {
		groceriesPurchased.Clear();
		currentSteps = 0;

	}

	public void PopulateStoreShelves() {
		GameObject shelvesContainer = GameObject.Find("Store Shelves");
		Shelf[] storeShelves = shelvesContainer.GetComponentsInChildren<Shelf>();
		
		if(dayStoreShelves.Length == 0) {
			List<Food> healthyFood = new List<Food>();
			List<Food> unhealthyFood = new List<Food>();

			foreach(Food food in refData.foodTypes) {
				if(food.type == FoodType.HEALTHY) {
					healthyFood.Add(food);
				} else {
					unhealthyFood.Add(food);
				}
			}

			foreach(Shelf shelf in storeShelves) {
				if(shelf.distance <= 2 && unhealthyFood.Count > 0) {
					int index = UnityEngine.Random.Range(0, unhealthyFood.Count - 1);
					shelf.setFoodOnShelf(unhealthyFood[index]);
					unhealthyFood.RemoveAt(index);
				} else {
					int index = UnityEngine.Random.Range(0, healthyFood.Count - 1);
					shelf.setFoodOnShelf(healthyFood[index]);
					healthyFood.RemoveAt(index);
				}

			}
		} else {
			for(int i = 0; i < dayStoreShelves.Length; i++) {
				storeShelves[i] = dayStoreShelves[i];
			}
		}
	}

	public void ConsumeFood(Food food) {
		if(groceriesPurchased.Count + extraPurchases < groceryList.Count) {
			if(groceryList.ContainsKey(food.name)) {
				if(groceriesPurchased.ContainsKey(food.name)) {
					groceriesPurchased[food.name] += 1;
				} else {
					groceriesPurchased[food.name] = 1;
				}
			} else {
				extraPurchases++;
			}
			
			OnFoodConsumed?.Invoke();
		}
	}

	public void TakeStep() {
		currentSteps++;
		OnStepTaken?.Invoke();
	}
}
