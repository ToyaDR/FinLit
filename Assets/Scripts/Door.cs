using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour {

    public Store store;
	Vector3 touchPosWorld;


	// Use this for initialization
	void Start () {
		store = GameObject.Find("Store").GetComponent<Store>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount == 1 
			&& Input.GetTouch (0).phase == TouchPhase.Stationary) {

			// transform touch position to world space
			touchPosWorld = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			Vector2 touchPosWorld2D = new Vector2 (touchPosWorld.x, touchPosWorld.y);

			//raycast
			RaycastHit2D hitInfo = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

			if (hitInfo.collider != null) {
				GameObject touchedObject = hitInfo.transform.gameObject;

				if (touchedObject == gameObject) {
					store.ShowIngredients ();
				}
			}
		}
	}

	/*
    void OnMouseDown() {
        store.ShowIngredients();
    }
    */
}
