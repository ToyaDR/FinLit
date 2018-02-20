using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    Store store;

	// Use this for initialization
	void Start () {
		store = GameObject.Find("Store").GetComponent<Store>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown() {
        store.ShowIngredients();
    }
}
