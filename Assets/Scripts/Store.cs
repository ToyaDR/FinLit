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
	public int tax_percent;

	private string product;
	private int product_price;

	private int productivity;
	private int reputation;
	private int fee;
	private int curr_month;
	private int total_loan = 1200;
	private int[] loan_due;
	private int penalty;
	private Dictionary<string, int> Stock;

	/*   Variables from other scripts   */

	public GameObject ingredientsPanel;
	public GameObject employeeChoosePanel;

	public GameObject taskAssignPanel;
	public GameObject score;
	public GameObject product_in_stock;

	void Awake() {
		loan_due = new int[12];
		fee = 10;
		penalty = 0;
		money = 100;
		curr_month = 0;
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

		foreach (int i in loan_due) {
			loan_due[i] = total_loan/12;
		}
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

	public bool Make(){
		//first check there's enough in stock to make something
		bool enough = true;
		foreach (Ingredient ing in ingredientsPanel.GetComponent<IngredientsPanel>().getIngredientsList ()) {
			if(ing.getAmountInRecipe() > GetStockAmount(ing.getName())){
				enough = false;
			}
		}

		if (enough) {
			foreach (Ingredient ing in ingredientsPanel.GetComponent<IngredientsPanel>().getIngredientsList ()) {
				RemoveStock (ing.getName(), ing.getAmountInRecipe()); //TODO: change this to different values
			}
			AddStock (product, 1);
		}
		return enough;
	}

	public int GetReputation(){
		return reputation;
	}

	public bool Sell(){
		if (GetProductAmount () > 0) {
			RemoveStock (product, 1);
			IncMoney (product_price);
			return true;
		}
		return false;
	}

	public int DecLoan(int amount){
		total_loan -= amount;

		int remaining = loan_due [curr_month];
		if (amount >= remaining) {
			/* Player has (over)paid loan due */
			loan_due [curr_month] = 0;
			curr_month++;
			loan_due [curr_month] -= (amount - remaining);
		} else {
			/* Player has not met minimum loan due */
			penalty++;
			if (penalty >= 3 || (curr_month == 11)) {
				// TODO: Loss condition
			} else {
				/* Current months loan payment overflows into next month */
				curr_month++;
				loan_due [curr_month] += (remaining + fee);
			}
		}
		PrintLoanDue ();
		return penalty;
	}

	public int GetLoan(){
		return loan_due[curr_month];
	}

	public void PrintLoanDue(){
		foreach (int i in loan_due) {
			Debug.Log (loan_due [i] + " ");
		}
	}

	public void DecTax(){
		/* Player should always be able to pay tax */
		int tax_pay = money / tax_percent; // this rounds, we're ok with that
		money -= tax_pay;
	}
}