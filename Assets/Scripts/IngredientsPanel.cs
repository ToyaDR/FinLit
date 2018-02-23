using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	private ArrayList ingredients;
	public Canvas canvas;
	Vector2 anchorPoint;

	// Use this for initialization
	void Start () {
		float canvasLeft = canvas.GetComponent<RectTransform> ().anchorMin.x;
		float canvasTop = canvas.GetComponent<RectTransform> ().anchorMax.y;


		ingredients = new ArrayList ();

		Ingredient flour = new Ingredient ();
		flour.ing_name = "Flour";
		flour.ing_price = 2;

		ingredients.Add(flour);

		Ingredient eggs = new Ingredient ();
		eggs.ing_name = "Eggs";
		eggs.ing_price = 1;

		ingredients.Add(eggs);

		float offset = 0;
		foreach(Ingredient i in ingredients){
			GameObject ingTextGO = Instantiate (Resources.Load ("Ingredients", typeof(GameObject))) as GameObject;
			ingTextGO.transform.SetParent (transform);
			ingTextGO.GetComponent<Text> ().text = i.ing_name + " $" + i.ing_price;


			anchorPoint = new Vector2 (canvasLeft, canvasTop + offset);

			ingTextGO.GetComponent<Text> ().rectTransform.anchoredPosition = new Vector2 (canvasLeft, canvasTop + offset);
			offset += ingTextGO.GetComponent<RectTransform>().rect.height;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
