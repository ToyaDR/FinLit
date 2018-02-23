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
	bool ingPanelActive = false;

	/*   Variables from other scripts   */

	Text score;
	public GameObject door;
	public GameObject closeButton;
	public GameObject ingredientsPanel;

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

				Debug.Log ("close button pos = " + closeButton.transform.localPosition);
				Debug.Log ("tap pos = " + touchedObject.transform.localPosition);
				if (touchedObject == door) {
					ShowIngredients ();
				} else if (touchedObject == closeButton) {
					HideIngredients ();
				}
			}
		}
	}

	// Show ingredients
	public void ShowIngredients() {
		if (!ingPanelActive) {
			ingredientsPanel.GetComponent<CanvasGroup>().alpha = 1f;
			ingredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

			ingPanelActive = true;
			Debug.Log ("active");
		}
	}

	// Hide ingredients
	public void HideIngredients() {
		ingredientsPanel.GetComponent<CanvasGroup>().alpha = 0f;
		ingredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

		ingPanelActive = false;
		Debug.Log ("inactive");
	}
}
