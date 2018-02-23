using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	private ArrayList ingredients;

	// Use this for initialization
	void Start () {
		ingredients = new ArrayList ();

		Ingredient flour = new Ingredient ();
		flour.ing_name = "flour";
		flour.ing_price = 2;

		ingredients.Add(flour);

		Ingredient eggs = new Ingredient ();
		eggs.ing_name = "eggs";
		eggs.ing_price = 1;

		ingredients.Add(eggs);

		foreach(Ingredient i in ingredients){
			GameObject ingTextGO = Instantiate (Resources.Load ("Ingredients", typeof(GameObject))) as GameObject;
			ingTextGO.transform.SetParent (transform);
			ingTextGO.GetComponent<Text> ().text = i.ing_name + " " + i.ing_price;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
