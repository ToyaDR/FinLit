using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsPanel : MonoBehaviour {

	private Ingredient[] ingredients;

	// Use this for initialization
	void Start () {

		Ingredient flour = new Ingredient ();
		flour.ing_name = "flour";
		flour.ing_price = 2;

		Ingredient eggs = new Ingredient ();
		eggs.ing_name = "eggs";
		eggs.ing_price = 1;

		ingredients = [flour, eggs];

		for(int i = 0; i < ingredients.Length; i++){
			GameObject ingTextGO = new GameObject(ingredients[i]);
			ingTextGO.transform.SetParent(this.transform);

			Text ingText = ingTextGO.AddComponent<Text>();
			ingText.text = ingredients[i].getName();

			Text ingPrice = ingTextGO.AddComponent<Text>();
			ingPrice.text = ingredients[i].getPrice();

			//TODO: figure out how to load sprites
			/*
			Sprite ingSprite = Resources.Load("",);
			Sprite ingSprite = ingTextGO.AddComponent<Sprite>();
			ingSprite = ingredients[i].getPrice();
			*/
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
