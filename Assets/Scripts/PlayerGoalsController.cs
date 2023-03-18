using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoalsController : MonoBehaviour {
	public float currentEnergy;
	public int currentSteps;
	public Dictionary<string, int> groceriesPurchased = new Dictionary<string, int>();
	public float timeInBed;

	public void ResetGoals() {

    }
}
