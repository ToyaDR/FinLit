﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	private ArrayList ingredients;
	public Canvas canvas;
	public Store store;
	private Dictionary<Ingredient, int> boughtIngredients;

	// Use this for initialization
	void Start () {
		float canvasRight = -350;
		float canvasTop = -120;

		ingredients = new ArrayList ();
		boughtIngredients = new Dictionary<Ingredient, int>();

		Ingredient flour = new Ingredient ();
		flour.addName("Flour");
		flour.addPrice(2);
		flour.addSprite ("Flour"); 

		ingredients.Add(flour);

		Ingredient eggs = new Ingredient ();
		eggs.addName("Eggs");
		eggs.addPrice(1);
		eggs.addSprite ("Eggs");

		ingredients.Add(eggs);

		Ingredient sugar = new Ingredient ();
		sugar.addName("Sugar");
		sugar.addPrice(1);
		sugar.addSprite ("Sugar");

		ingredients.Add(sugar);

		float offset = 0;
		foreach(Ingredient i in ingredients){
			GameObject ingTextGO = Instantiate (Resources.Load ("Ingredients", typeof(GameObject))) as GameObject;
			ingTextGO.transform.SetParent (transform);
			ingTextGO.transform.localScale = new Vector3(0.88f,0.88f,0.88f);
			ingTextGO.GetComponent<Text>().text = i.getName() + " $" + i.getPrice();

			Vector2 anchorPoint = new Vector2 (canvasRight, canvasTop - offset);

			ingTextGO.GetComponent<Text>().rectTransform.anchoredPosition = anchorPoint;

			ingTextGO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = i.getSprite();

			ingTextGO.transform.GetChild (1).gameObject.GetComponent<Text> ().text = "x0";

			ingTextGO.GetComponent<Button>().onClick.AddListener(delegate {
				Decrement(i.getName(), i.getPrice());
				ingTextGO.transform.GetChild (1).gameObject.GetComponent<Text> ().text = "x" + store.GetStockAmount(i.getName()).ToString();
				boughtIngredients[i]++;
			});

			offset += ingTextGO.GetComponent<RectTransform>().rect.height*1.2f;
			boughtIngredients [i] = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void Decrement(string ing, int price){
		store.DecMoney (price);
		store.AddStock (ing, 1);
	}

	public ArrayList getIngredientsList(){
		return ingredients;
	}

	public Dictionary<Ingredient, int> bought(){
		return boughtIngredients;
	}

	public void resetBought(){
		foreach (Ingredient i in ingredients) {
			boughtIngredients [i] = 0;
		}
	}
}
