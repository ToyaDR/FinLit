using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	private ArrayList ingredients;
	public Canvas canvas;
	public Store store;

	// Use this for initialization
	void Start () {
		float canvasLeft = -150;
		float canvasTop = -77;

		ingredients = new ArrayList ();

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
			ingTextGO.GetComponent<Text>().text = i.getName() + " $" + i.getPrice();

			Vector2 anchorPoint = new Vector2 (canvasLeft, canvasTop - offset);

			ingTextGO.GetComponent<Text>().rectTransform.anchoredPosition = anchorPoint;

			ingTextGO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = i.getSprite();

			//GameObject ingImageGO = ingTextGO.transform.GetChild (0).gameObject;
			//ingImageGO.transform.localPosition = new Vector3(ingTextGO.GetComponent<RectTransform>().rect.width, offset, 0);

			ingTextGO.GetComponent<Button>().onClick.AddListener(delegate {
				Decrement(i.getName(), i.getPrice());
			});

			offset += ingTextGO.GetComponent<RectTransform>().rect.height*1.2f;

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
