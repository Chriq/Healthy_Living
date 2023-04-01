using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	private static UIManager instance;

	public static UIManager Instance {
		get {
			if(instance == null) {
				if(!(instance = FindObjectOfType<UIManager>())) {
					instance = new GameObject("UIManager").AddComponent<UIManager>();
				}
			}

			return instance;
		}
	}

	[SerializeField] Slider currentHealth;
	[SerializeField] Slider blockedHealth;

	[SerializeField] private TextMeshProUGUI clockTime;
	[SerializeField] private TextMeshProUGUI clockTag;

	[SerializeField] private TextMeshProUGUI steps;

	[SerializeField] private TextMeshProUGUI budget;
	[SerializeField] private List<GrocerySlot> shoppingList;

	private void Awake() {
		UIManager[] objs = FindObjectsOfType<UIManager>();

		if(objs.Length > 1) {
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this);
	}

	private void Start() {
		UpdateTime();
		UpdateGroceryList();
		UpdateSteps();
		UpdateHealth();
	}

	private void OnEnable() {
		TimeManager.Instance.OnHourChanged += UpdateTime;
		TimeManager.Instance.OnMinuteChanged += UpdateTime;

		GameManager.Instance.OnStepTaken += UpdateSteps;
		GameManager.Instance.OnFoodConsumed += UpdateGroceryList;
		GameManager.Instance.OnEnergyChanged += UpdateHealth;
	}

	private void OnDisable() {
		TimeManager.Instance.OnHourChanged -= UpdateTime;
		TimeManager.Instance.OnMinuteChanged -= UpdateTime;

		GameManager.Instance.OnStepTaken -= UpdateSteps;
		GameManager.Instance.OnFoodConsumed -= UpdateGroceryList;
		GameManager.Instance.OnEnergyChanged -= UpdateHealth;
	}

	private void UpdateTime() {
		clockTime.text = $"{TimeManager.Instance.Hour:00} : {TimeManager.Instance.Minute:00}";
		clockTag.text = TimeManager.Instance.timeTag.ToString();
	}

	private void UpdateSteps() {
		steps.text = $"{GameManager.Instance.currentSteps} / {GameManager.Instance.dayStepGoal}";
	}

	private void UpdateGroceryList() {
		List<string> groceries = GameManager.Instance.groceryList.Keys.ToList();
		if(groceries.Count == 0) {
			ClearGroceryList();
		} else {
			ClearGroceryList();
			for(int i = 0; i < groceries.Count; i++) {
				if(i < shoppingList.Count) {
					bool complete = false;
					int amt = 0;
					GameManager.Instance.groceriesPurchased.TryGetValue(groceries[i], out amt);
					if(amt >= GameManager.Instance.groceryList[groceries[i]]) {
						complete = true;
					}

					budget.text = $"Budget: {GameManager.Instance.numGroceriesPurchased + GameManager.Instance.extraPurchases}/{GameManager.Instance.numGroceriesOnList}";
					if(GameManager.Instance.numGroceriesPurchased + GameManager.Instance.extraPurchases >= GameManager.Instance.numGroceriesOnList) {
						budget.color = Color.red;
					} else {
						budget.color = Color.black;
					}
					
					shoppingList[i].checkmark.SetActive(complete);
					shoppingList[i].text.text = $"{groceries[i]} ({amt}/{GameManager.Instance.groceryList[groceries[i]]})";
					shoppingList[i].gameObject.SetActive(true);
				}
			}
		}
	}

	private void UpdateHealth() {
		currentHealth.maxValue = GameManager.Instance.maxEnergyTotal;
		blockedHealth.maxValue = GameManager.Instance.maxEnergyTotal;

		currentHealth.value = GameManager.Instance.currentEnergy;
		blockedHealth.value = GameManager.Instance.maxEnergyTotal - GameManager.Instance.maxDayEnergy;
	}

	private void ClearGroceryList() {
		foreach(GrocerySlot slot in shoppingList) {
			slot.checkmark.SetActive(false);
			slot.gameObject.SetActive(false);
		}
	}
}
