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
	private int loan;
	private Dictionary<string, int> Stock;

	/*   Variables from other scripts   */

	//public GameObject closeButton;
	public GameObject ingredientsPanel;
	public GameObject employeeChoosePanel;

	public GameObject taskAssignPanel;
	public GameObject score;
	public GameObject product_in_stock;

	void Awake() {
		loan = 1000;
		money = 100;
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

	public void DecMoney(int price) {
		money -= price;
	}

	public void IncMoney(int price) {
		money += price;
	}

	public int GetPrice(){
		return product_price;
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

	public void DecLoan(int amount){
		loan -= amount;
	}

	public int GetLoan(){
		return loan;
	}
}