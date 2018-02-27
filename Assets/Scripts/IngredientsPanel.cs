﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	private ArrayList ingredients;
	public Canvas canvas;
	public Store store;

	// Use this for initialization
	void Start () {
		float canvasLeft = canvas.GetComponent<RectTransform> ().anchorMin.x;
		float canvasTop = canvas.GetComponent<RectTransform> ().anchorMax.y;

		ingredients = new ArrayList ();

		Ingredient flour = new Ingredient ();
		flour.addName("Flour");
		flour.addPrice(2);
		//flour.addSprite ("Flour"); 

		ingredients.Add(flour);

		Ingredient eggs = new Ingredient ();
		eggs.addName("Eggs");
		eggs.addPrice(1);
		//eggs.addSprite ("Eggs");

		ingredients.Add(eggs);

		float offset = 0;
		foreach(Ingredient i in ingredients){
			GameObject ingTextGO = Instantiate (Resources.Load ("Ingredients", typeof(GameObject))) as GameObject;
			ingTextGO.transform.SetParent (transform);
			ingTextGO.GetComponent<Text>().text = i.getName() + " $" + i.getPrice();

			Vector2 anchorPoint = new Vector2 (canvasLeft, canvasTop + offset);

			ingTextGO.GetComponent<Text>().rectTransform.anchoredPosition = anchorPoint;

			ingTextGO.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = i.getSprite();
			/*
			GameObject ingSpriteGO = ingTextGO.transform.GetChild (0).gameObject;
			ingSpriteGO.transform.localPosition = new Vector3(ingTextGO.GetComponent<RectTransform>().rect.width, offset, 0);
			*/

			int price = i.getPrice ();
			ingTextGO.GetComponent<Button>().onClick.AddListener(delegate {
				Decrement(i.getName(), i.getPrice());
			});

			offset += ingTextGO.GetComponent<RectTransform>().rect.height*1.5f;

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
}
