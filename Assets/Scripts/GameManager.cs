using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	float shift_length;
	float lunch_length;
	bool start_day;
	public Store store;

	// Use this for initialization
	void Start () {
		start_day = false;

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
		while (start_day) {
			float freq = shift_length / (float)(store.GetReputation ());
			yield return new WaitForSeconds (freq);
			store.Sell ();
		}
	}

	public void StartDay(){
		start_day = true;
	}

	public void Pause(){
		start_day = false;
	}
}