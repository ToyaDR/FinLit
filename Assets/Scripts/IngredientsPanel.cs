using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	public gameObject.alpha alpha;
	private ArrayList ingredients;

	// Use this for initialization
	void Start () {
		ingredients = new ArrayList ();

		alpha = 300;

		Ingredient flour = new Ingredient ();
		flour.ing_name = "flour";
		flour.ing_price = 2;

		ingredients.Add(flour);

		Ingredient eggs = new Ingredient ();
		eggs.ing_name = "eggs";
		eggs.ing_price = 1;

		ingredients.Add(eggs);

		foreach(Ingredient i in ingredients){
			GameObject ingTextGO = new GameObject();
			ingTextGO.transform.SetParent(this.transform);

			Text ingText = ingTextGO.AddComponent<Text>();
			ingText.text = i.getName();

			/*
			Text ingPrice = ingTextGO.AddComponent<Text>();
			ingPrice.text = i.getPrice().ToString();
			*/

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
