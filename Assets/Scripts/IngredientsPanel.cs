using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientsPanel : MonoBehaviour {

	private ArrayList ingredients;
	public Canvas canvas;
	public Store store;
	public Dictionary<Ingredient, int> boughtIngredients;
	public GameObject WarningPanel;

	// Use this for initialization
	void Start () {
		float canvasRight = -440;
		float canvasTop = -130;

		ingredients = new ArrayList ();
		boughtIngredients = new Dictionary<Ingredient, int>();

		Ingredient flour = new Ingredient ();
		flour.addName("Flour");
		flour.addPrice(2);
		flour.addSprite ("Flour"); 
		flour.setAmountInRecipe (1);

		ingredients.Add(flour);

		Ingredient eggs = new Ingredient ();
		eggs.addName("Eggs");
		eggs.addPrice(1);
		eggs.addSprite ("Eggs");
		eggs.setAmountInRecipe (2);

		ingredients.Add(eggs);

		Ingredient sugar = new Ingredient ();
		sugar.addName("Sugar");
		sugar.addPrice(1);
		sugar.addSprite ("Sugar");
		sugar.setAmountInRecipe (3);

		ingredients.Add(sugar);

		float offset = 0;
		foreach(Ingredient i in ingredients){
			GameObject ingTextGO = Instantiate (Resources.Load ("Ingredients", typeof(GameObject))) as GameObject;
			ingTextGO.transform.SetParent (transform);
			ingTextGO.transform.localScale = new Vector3(1f,1f,1f);
			ingTextGO.GetComponent<Text>().text = "$" + i.getPrice();

			boughtIngredients.Add(i,0);
			Vector2 anchorPoint = new Vector2 (canvasRight, canvasTop - offset);

			ingTextGO.GetComponent<Text>().rectTransform.anchoredPosition = anchorPoint;

			ingTextGO.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = i.getSprite();

			ingTextGO.transform.GetChild (1).gameObject.GetComponent<Text> ().text = "x0";

			Button ingButton = ingTextGO.transform.GetChild (2).GetComponent<Button> ();
			ingButton.onClick.AddListener(delegate {
				HideWarning();

				int amount = 0;
				Text input = ingButton.transform.GetChild(1).Find("Text").GetComponent<Text>();
				int.TryParse((input.text).ToString(), out amount);
				if(amount*i.getPrice() > store.GetComponent<Store>().money){
					ShowWarning();

					/* Reset TextInput box */
					ingButton.transform.GetChild(1).gameObject.GetComponent<InputField>().text = "";
					return;
				}

				Decrement(i.getName(), i.getPrice(), amount);
				ingTextGO.transform.GetChild (1).gameObject.GetComponent<Text> ().text = "x" + store.GetStockAmount(i.getName()).ToString();
				boughtIngredients[i]+=amount;
				/* Reset TextInput box */
				ingButton.transform.GetChild(1).gameObject.GetComponent<InputField>().text = "";
			});

			offset += ingTextGO.GetComponent<RectTransform>().rect.height*1.2f;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void Decrement(string ing, int price, int amount){
		store.DecMoney (price*amount);
		store.AddStock (ing, amount);
	}

	public ArrayList getIngredientsList(){
		return ingredients;
	}

	public void resetBought(){
		foreach (Ingredient i in ingredients) {
			boughtIngredients [i] = 0;
		}
	}

	private void ShowWarning(){
		WarningPanel.GetComponent<CanvasGroup>().alpha = 1f;
		WarningPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

	public void HideWarning(){
		WarningPanel.GetComponent<CanvasGroup>().alpha = 0f;
		WarningPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
}
