using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Food")]
public class Food : ScriptableObject {
	public string name;
	public Sprite sprite;
	public float restoreAmt;
	public FoodType type;
}

public enum FoodType {
	HEALTHY,
	JUNK
}