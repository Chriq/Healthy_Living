using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = GameObject.Find("GameManager").GetComponent<GameManager>();
			}

			return instance; 
		}
	}

	public Vector3 playerPositionOnLoad = Vector3.zero;

	public RefData refData;
	public readonly float maxEnergyTotal = 100;

	public int day = 1;
	public float maxDayEnergy = 100f;
	public int dayStepGoal = 0;
	public float bedTime = 12f;
	public Dictionary<string, int> groceryList = new Dictionary<string, int>();

	public float currentEnergy;
	public int currentSteps;
	public Dictionary<string, int> groceriesPurchased = new Dictionary<string, int>();
	public int numGroceriesPurchased = 0;
	public int numGroceriesOnList = 0;
	public int extraPurchases = 0;
	public float timeInBed;

	public float currentGameTime;

	Shelf[] dayStoreShelves;

	public Action OnStepTaken;
	public Action OnFoodConsumed;
	public Action OnEnergyChanged;

	public Fade fade;

	private void Awake() {
		GameManager[] objs = FindObjectsOfType<GameManager>();

		if(objs.Length > 1) {
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this);

		dayStoreShelves = new Shelf[0];
		maxDayEnergy = maxEnergyTotal;
		currentEnergy = maxDayEnergy;
		fade = GameObject.Find("Fade").GetComponent<Fade>();
		fade.FadeIn();
		GenerateGroceryList();
		GenerateStepGoal();

	}

	private void OnEnable() {
		TimeManager.Instance.OnHourChanged += Exhaustion;
		TimeManager.Instance.OnMinuteChanged += AdjustEnergy;
	}

	private void OnDisable() {
		TimeManager.Instance.OnHourChanged -= Exhaustion;
		TimeManager.Instance.OnMinuteChanged -= AdjustEnergy;
	}

	public void GoToBed() {

		IEnumerator coroutine = ShowDayNumber(1.2f);

		TimeManager.Instance.PauseTimer();
		fade.FadeOutWithCallback(delegate {
			if(SceneManager.GetActiveScene().name != "Home") {
				SceneManager.LoadScene("Home");
			}

			timeInBed = TimeManager.Instance.GetTimeAsFloat();
			IncrementDay();
			TimeManager.Instance.SetTime(8, 0, TimeTag.AM);

			if(maxDayEnergy > 0) {
				StartCoroutine(coroutine);
			}
		});

		
	}

	IEnumerator ShowDayNumber(float delay) {
		GameObject dayText = GameObject.Find("Day").transform.GetChild(0).gameObject;
		dayText.GetComponent<TextMeshProUGUI>().text = $"Day {day}";
		dayText.SetActive(true);
		yield return new WaitForSeconds(delay);
		dayText.SetActive(false);

		fade.FadeInWithCallback(delegate {
			TimeManager.Instance.ResumeTimer();
		});
	}

	public void IncrementDay() {
		ResetDayEnergy();

		if(maxDayEnergy <= 0) {
			SceneManager.LoadScene("End");
			Destroy(GameObject.Find("UIManager"));
			OnDisable();
			Destroy(TimeManager.Instance);
		} else {
			day++;
			ResetPlayerGoals();
			GenerateGroceryList();
			GenerateStepGoal();

			dayStoreShelves = new Shelf[0];
		}
	}

	public void ResetDayEnergy() {
		if(numGroceriesPurchased < numGroceriesOnList) {
			maxDayEnergy -= numGroceriesOnList - numGroceriesPurchased;
		}

		if(currentSteps < dayStepGoal) {
			maxDayEnergy -= dayStepGoal - currentSteps;
		}

		if(timeInBed < 8f && TimeManager.Instance.timeTag == TimeTag.AM) {
			maxDayEnergy -= Mathf.Ceil(timeInBed);
		}

		currentEnergy = maxDayEnergy;
		OnEnergyChanged?.Invoke();
	}

	public void GenerateGroceryList() {
		groceryList.Clear();
		numGroceriesOnList = 0;
		numGroceriesPurchased = 0;
		extraPurchases = 0;

		int numItemsTotal = (int) Mathf.Floor(7f * Mathf.Log((float) day) - 3f);
		numItemsTotal = (int) Mathf.Clamp(numItemsTotal, 1f, 30f);

		int numUniqueItems = (int) Mathf.Clamp(2 * Mathf.Log(day), 1, 8);
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

		foreach(string key in groceryList.Keys) {
			numGroceriesOnList += groceryList[key];
		}

		OnFoodConsumed?.Invoke();
	}

	public void GenerateStepGoal() {
		dayStepGoal = 25 + (int) (21f * Mathf.Log(day));
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

	public bool ConsumeFood(Food food) {
		if(numGroceriesPurchased + extraPurchases < numGroceriesOnList) {
			if(groceryList.ContainsKey(food.name)) {
				if(groceriesPurchased.ContainsKey(food.name)) {
					groceriesPurchased[food.name] += 1;
				} else {
					groceriesPurchased[food.name] = 1;
				}
				numGroceriesPurchased++;
			} else {
				extraPurchases++;
			}

			
			currentEnergy = Mathf.Clamp(currentEnergy + food.restoreAmt, 0, maxDayEnergy);
			OnFoodConsumed?.Invoke();
			OnEnergyChanged?.Invoke();
			return true;
		}

		return false;
	}

	public void TakeStep() {
		currentSteps++;
		OnStepTaken?.Invoke();
	}

	private void Exhaustion() {
		if(TimeManager.Instance.Hour == 2 && TimeManager.Instance.timeTag == TimeTag.AM) {
			GoToBed();
		}
	}

	private void AdjustEnergy() {
		if(numGroceriesPurchased + extraPurchases < numGroceriesOnList) {
			currentEnergy -= (1f + 0.01f * day);
			OnEnergyChanged?.Invoke();
			if(currentEnergy <= 0) {
				GoToBed();
			}
		}
	}
}
