using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	float shift_length;
	float lunch_length;

	float start_time;
	float time_elapsed;
	bool shift_started;
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
		time_elapsed = 0f;

		curr_state = State.DAY_SHIFT;
		shift_length = 60f*2f; //2 minutes
		lunch_length = 60f*1f; //1 minute

		StartCoroutine ("Sale");
	}
	
	// Update is called once per frame
	void Update () {
		//Check if player wants to pause

		//Check if shift has ended
		time_elapsed += Time.time - start_time;

		bool end_lunch = (curr_state == State.LUNCH) && (time_elapsed >= lunch_length);
		bool end_shift = (curr_state == State.DAY_SHIFT || curr_state == State.NIGHT_SHIFT)
							&& (time_elapsed >= shift_length);

		if ((end_lunch || end_shift) && shift_started) {
			StopShift ();
		}

	}

	private IEnumerator Sale(){
		while (true) {
			if (shift_started) {
				float freq = shift_length / (float)(store.GetReputation ());
				yield return new WaitForSeconds (freq);
				store.Sell ();
			}
			yield return null;
		}
	}

	public void DayShift() {
		// Start employees moving
	}

	public void Lunch() {
		// NightEmployee and DayEmployee move to lunch room
	}

	public void NightShift() {
	}

	public void StartShift(State state){
		start_time = Time.time;
		time_elapsed = 0f;
		shift_started = true;
		curr_state = state;

		//Shift logic
		if (state == State.DAY_SHIFT) {
			DayShift ();
			return;
		}

		if (state == State.LUNCH) {
			Lunch ();
			return;
		}

		if (state == State.NIGHT_SHIFT) {
			NightShift ();
			return;
		}
	}

	public void PauseShift(){
		shift_started = false;
		// time already elapsed + elapsed time since last pause
		time_elapsed += Time.time - start_time; 
	}

	public void PlayShift(){
		shift_started = true;
		// start time again
		start_time = Time.time; 
	}

	public void StopShift(){
		shift_started = false;

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