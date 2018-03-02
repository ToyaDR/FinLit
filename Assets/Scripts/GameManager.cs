using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	float shift_length;
	float lunch_length;

	float start_time;
	bool start_shift;
	bool lunch;
	public Store store;

	public enum State{
		DAY_SHIFT,
		LUNCH,
		NIGHT_SHIFT
	}

	State curr_state;
	// Use this for initialization
	void Start () {
		start_shift = false;

		curr_state = State.DAY_SHIFT;
		shift_length = 60f*2f; //2 minutes
		lunch_length = 60f*1f;
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		StartCoroutine ("Sale");
	}
	
	// Update is called once per frame
	void Update () {
		bool end_lunch = (curr_state == State.LUNCH) && ((Time.time - start_time) >= lunch_length);
		bool end_shift = (curr_state == State.DAY_SHIFT || curr_state == State.NIGHT_SHIFT)
		                  && ((Time.time - start_time) >= shift_length);

		if ((end_lunch || end_shift) && start_shift) {
			StopShift ();
		}



	}

	private IEnumerator Sale(){
		while (true) {
			if (start_shift) {
				float freq = shift_length / (float)(store.GetReputation ());
				yield return new WaitForSeconds (freq);
				store.Sell ();
			}
			yield return null;
		}
	}

	public void StartShift(State state){
		start_time = Time.time;
		start_shift = true;
		curr_state = state;
	}

	public void StopShift(){
		start_shift = false;

		if (curr_state == State.DAY_SHIFT) {
			//start lunch
			Debug.Log("Lunch");
			StartShift (State.LUNCH);
		} else if (curr_state == State.LUNCH) {
			//start night shift
			Debug.Log("Night shift");
			StartShift(State.NIGHT_SHIFT);
		} else {
			//prompt to choose employees again
			Debug.Log("Please assign 2 employees");
		}
	}
}