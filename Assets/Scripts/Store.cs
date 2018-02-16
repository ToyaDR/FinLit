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

	/*   Variables from other scripts   */

	Text score;

	void Awake() {
		money = 0;
		insurance = false;
		savings_account = 0;
		checkings_account = 0;
		productivity = 50;
		reputation = 0;
	}

	// Use this for initialization
	void Start () {
		// Initialize and display money
		score = GameObject.Find("Money").GetComponent<Text>();
		score.text = money.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		money = 10;
		score.text = money.ToString();
	}
}
