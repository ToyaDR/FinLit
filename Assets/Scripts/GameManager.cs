using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	float shift_length;
	float lunch_length;
	public Store store;

	// Use this for initialization
	void Start () {
		shift_length = 60f*2f; //2 minutes
		lunch_length = 60f*1f;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		StartCoroutine ("Sale");
	}
	
	// Update is called once per frame
	void Update () {
		// frequency of customers depends on store reputation
	}

	private IEnumerator Sale(){
		while (true) {
			float freq = shift_length / (float)(store.GetReputation ());
			yield return new WaitForSeconds (freq);
			store.Sell ();
		}
	}
}