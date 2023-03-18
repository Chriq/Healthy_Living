using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager instance;
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = new GameObject("GameManager").AddComponent<GameManager>();
			}

			return instance; 
		}
	}

	public int day = 0;
	public float maxDayEnergy;
	public int dayStepGoal;
	public float bedTime;
	public Dictionary<string, int> groceryList = new Dictionary<string, int>();

	public float currentGameTime;
	public PlayerGoalsController player;

	public void IncrementDay() {
		ResetDayEnergy();
		GenerateGroceryList();
		GenerateStepGoal();

		player.ResetGoals();

		day++;
	}

	public void ResetDayEnergy() {
		// check for missed goals
	}

	public void GenerateGroceryList() {

	}

	public void GenerateStepGoal() {

	}

	public void PopulateStoreShelves() {

	}

	public void PauseGameTime() {

	}

	public void ResumeGameTime() {

	}
}
