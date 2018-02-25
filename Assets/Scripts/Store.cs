using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour {

	/*   Variables for Store   */

	public int money; // current money
	public bool insurance;
	public int savings_account;
	public int checkings_account;
	public int productivity;
	public int reputation;
	//public List Items;
	//public List Equipment;
	//public string name;
	//public string type;

	Vector3 touchPosWorld;

	/*   Variables from other scripts   */

	Text score;
	public GameObject door;
	public GameObject closeButton;
	public GameObject ingredientsPanel;
	public GameObject employeeChoosePanel;
	public GameObject start;

	void Awake() {
		money = 10000;
		insurance = false;
		savings_account = 0;
		checkings_account = 0;
		productivity = 50;
		reputation = 0;
	}

	// Use this for initialization
	void Start () {
		
		// Money
		score = GameObject.Find("Money").GetComponent<Text>();
		score.text = money.ToString();
		HideIngredients ();
		HideEmployeeChoose();

	}
	
	// Update is called once per frame
	void Update () {
		//money = 10;
		score.text = money.ToString();

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

	// Show employee choose panel
	public void ShowEmployeeChoose() {
		employeeChoosePanel.GetComponent<CanvasGroup>().alpha = 1f;
		employeeChoosePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
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

	// Hide ingredients
	public void HideEmployeeChoose() {
		employeeChoosePanel.GetComponent<CanvasGroup>().alpha = 0f;
		employeeChoosePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

	public void DecMoney(int price) {
		money -= price;
	}

	public void IncMoney(int price) {
		money += price;
	}
}
