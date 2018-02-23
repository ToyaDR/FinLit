using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
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
}
