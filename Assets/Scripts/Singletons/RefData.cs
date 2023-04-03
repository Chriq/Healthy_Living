using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/RefData")]
public class RefData : ScriptableObject {
	public List<Food> foodTypes;

	public List<Food> healthyFood {
		get {
			List<Food> healthy = new List<Food>();
			foreach(Food food in foodTypes) {
				if(food.type == FoodType.HEALTHY) {
					healthy.Add(food);
				}
			}

			return healthy;
		}
	}

	public List<Food> unhealthyFood {
		get {
			List<Food> unhealthy = new List<Food>();
			foreach(Food food in foodTypes) {
				if(food.type == FoodType.JUNK) {
					unhealthy.Add(food);
				}
			}

			return unhealthy;
		}
	}
}
