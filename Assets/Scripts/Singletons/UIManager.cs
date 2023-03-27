using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	[SerializeField] Slider currentHealth;
	[SerializeField] Slider blockedHealth;

	[SerializeField] private TextMeshProUGUI clockTime;
	[SerializeField] private TextMeshProUGUI clockTag;

	[SerializeField] private TextMeshProUGUI steps;

	[SerializeField] private List<GrocerySlot> shoppingList;

	private void Awake() {
		DontDestroyOnLoad(this);
	}

	private void Start() {
		UpdateGroceryList();
		UpdateSteps();
	}

	private void OnEnable() {
		TimeManager.Instance.OnHourChanged += UpdateTime;
		TimeManager.Instance.OnMinuteChanged += UpdateTime;

		GameManager.Instance.OnStepTaken += UpdateSteps;

		GameManager.Instance.OnFoodConsumed += UpdateGroceryList;
	}

	private void OnDisable() {
		TimeManager.Instance.OnHourChanged -= UpdateTime;
		TimeManager.Instance.OnMinuteChanged -= UpdateTime;

		GameManager.Instance.OnStepTaken -= UpdateSteps;

		GameManager.Instance.OnFoodConsumed -= UpdateGroceryList;
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

					shoppingList[i].checkmark.SetActive(complete);
					shoppingList[i].text.text = $"{groceries[i]} ({amt}/{GameManager.Instance.groceryList[groceries[i]]})";
					shoppingList[i].gameObject.SetActive(true);
				}
			}
		}
	}

	private void UpdateHealth() {
		//GameManager.Instance.maxEnergyTotal;
		//GameManager.Instance.maxDayEnergy;
		//GameManager.Instance.currentEnergy;

		float energyBlocked = GameManager.Instance.maxEnergyTotal - GameManager.Instance.maxDayEnergy;
		currentHealth.maxValue = GameManager.Instance.maxDayEnergy;
		blockedHealth.maxValue = energyBlocked;

		//currentHealth.value =
		//blockedHealth.value = () / GameManager.Instance.maxEnergyTotal;
	}

	private void ClearGroceryList() {
		foreach(GrocerySlot slot in shoppingList) {
			slot.checkmark.SetActive(false);
			slot.gameObject.SetActive(false);
		}
	}
}
