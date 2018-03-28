using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	/*   Variables for Store   */

	public int money; // current money
	private bool insurance;
	public int savings_account;
	public int checkings_account;

	private string product;
	private int product_price;

	private int productivity;
	private int reputation;
	private Dictionary<string, int> Stock;

	/*   Variables from other scripts   */

	public GameObject closeButton;
	public GameObject ingredientsPanel;
	public GameObject employeeChoosePanel;

	public GameObject taskAssignPanel;
	public GameObject score;
	public GameObject product_in_stock;

	void Awake() {
		money = 10000;
		insurance = false;
		savings_account = 0;
		checkings_account = 0;
		productivity = 50;
		reputation = 4;

		product = "cupcake";
		product_price = 3;
		Stock = new Dictionary<string, int>();
	}

	// Use this for initialization
	void Start () {
		// Money
		score.GetComponent<Text>().text = money.ToString();

		HideIngredients ();
		HideEmployeeChoose();
		HideTaskAssign ();

		ArrayList ingredients = ingredientsPanel.GetComponent<IngredientsPanel>().getIngredientsList ();

		foreach(Ingredient i in ingredients){
			Stock.Add(i.getName(), 0);
		}

		Stock.Add (product, 4);
		product_in_stock.GetComponent<Text> ().text = Stock[product].ToString();
	

	}
	
	// Update is called once per frame
	void Update () {
		score.GetComponent<Text>().text = money.ToString();
		product_in_stock.GetComponent<Text> ().text = Stock[product].ToString ();
	}

	// Hide task assign panel
	public void HideTaskAssign() {
		taskAssignPanel.GetComponent<CanvasGroup>().alpha = 0f;
		taskAssignPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Show employee choose panel
	public void ShowEmployeeChoose() {
		employeeChoosePanel.GetComponent<CanvasGroup>().alpha = 1f;
		employeeChoosePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Hide ingredients
	public void HideEmployeeChoose() {
		employeeChoosePanel.GetComponent<CanvasGroup>().alpha = 0f;
		employeeChoosePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Show ingredients
	public void ShowIngredients() {
		ingredientsPanel.GetComponent<CanvasGroup>().alpha = 1f;
		ingredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Hide ingredients
	public void HideIngredients() {
		ingredientsPanel.GetComponent<CanvasGroup>().alpha = 0f;
		ingredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void DecMoney(int price) {
		money -= price;
	}

	public void IncMoney(int price) {
		money += price;
	}

	public void AddStock(string ing, int amount){
		Stock[ing] += amount;
	}

	public void RemoveStock(string ing, int amount){
		Stock[ing] -= amount;
	}

	public int GetStockAmount(string ing){
		return Stock [ing]; 
	}

	public int GetProductAmount() {
		return Stock [product];
	}

	public void Make(){
		AddStock (product, 1);
		foreach (Ingredient ing in ingredientsPanel.GetComponent<IngredientsPanel>().getIngredientsList ()) {
			RemoveStock (ing.getName(), 1); //TODO: change this to different values
		}
	}

	public int GetReputation(){
		return reputation;
	}

	public void Sell(){
		if (GetProductAmount () > 0) {
			RemoveStock (product, 1);
			IncMoney (product_price);
		}
	}
}