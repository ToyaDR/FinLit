using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour {

    string ing_name;
    int ing_price;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addName(string name) {
        ing_name = name;
    }

    public void addPrice(int price) {
        ing_price = price;
    }
}
