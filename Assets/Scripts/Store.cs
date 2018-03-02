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
	private int product_stock;

	private string product;
	private int product_price;

	private int productivity;
	private int reputation;
	private Dictionary<string, int> Stock;

	Vector3 touchPosWorld;

	/*   Variables from other scripts   */

	public GameObject door;
	public GameObject closeButton;
	public GameObject ingredientsPanel;
	public GameObject employeeChoosePanel;
	public GameObject start;
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
		product_stock = 4;
		product_price = 3;
		Stock = new Dictionary<string, int>();
	}

	// Use this for initialization
	void Start () {
		// Money
		score.GetComponent<Text>().text = money.ToString();
		product_in_stock.GetComponent<Text> ().text = product_stock.ToString();

		HideIngredients ();
		HideEmployeeChoose();
		HideTaskAssign ();

		ArrayList ingredients = ingredientsPanel.GetComponent<IngredientsPanel>().getIngredientsList ();

		foreach(Ingredient i in ingredients){
			Stock.Add(i.getName(), 0);
		}

		Stock.Add (product, product_stock);
	}
	
	// Update is called once per frame
	void Update () {
		//money = 10;
		score.GetComponent<Text>().text = money.ToString();

		/* Check for touch input */
		if (Input.touchCount == 1 
			&& Input.GetTouch (0).phase == TouchPhase.Stationary) {

			// transform touch position to world space
			touchPosWorld = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			Vector2 touchPosWorld2D = new Vector2 (touchPosWorld.x, touchPosWorld.y);

			//raycast
			RaycastHit2D hitInfo = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

			if (hitInfo.collider != null) {
				GameObject touchedObject = hitInfo.transform.gameObject;

				if (touchedObject == door) {
					ShowIngredients ();
				}
				else if (touchedObject == start) {
					ShowEmployeeChoose();
				}
			}
		}
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

	public int GetReputation(){
		return reputation;
	}

	public int Sell(){
		if (GetProductAmount () > 0) {
			RemoveStock (product, 1);
			return money += product_price;
		}
		return 0;
	}
}