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
	public GameObject EmployeeManager;
	public GameObject BreakRoom;

	Employee emp1;
	Employee emp2;

	public enum State{
		DAY_SHIFT,
		LUNCH,
		NIGHT_SHIFT
	}

	State curr_state;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;

		curr_state = State.DAY_SHIFT;
		shift_length = 60f*2f; //2 minutes
		lunch_length = 60f*1f; //1 minute
		time_elapsed = shift_length; // count down to end of shift

		StartCoroutine ("Sale");
		/*emp1 = (Employee) EmployeeManager.GetComponent<EmployeeManager> ().myEmployees [0];
		emp2 = (Employee) EmployeeManager.GetComponent<EmployeeManager> ().myEmployees [1];
		*/
	}
	
	// Update is called once per frame
	void Update () {

		//Check if shift has ended
		if (shift_started) {
			//TODO: Check if player wants to pause

			time_elapsed -= Time.deltaTime;
			//Debug.Log ("TIME ELAPSED" + time_elapsed);

			if (time_elapsed <= 0) {;
				StopShift ();
			}
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
	}

	public void Lunch() {
		// Render emp2 in lunch room

		// Move emp1 to lunch room
	}

	public void NightShift() {
	}

	public void StartShift(State state){
		Debug.Log("START SHIFT");
		start_time = Time.time;
		shift_started = true;
		curr_state = state;

		//Shift logic
		if (state == State.DAY_SHIFT) {
			time_elapsed = shift_length;
			DayShift ();
			return;
		}

		if (state == State.LUNCH) {
			time_elapsed = lunch_length;
			Lunch ();
			return;
		}

		if (state == State.NIGHT_SHIFT) {
			time_elapsed = shift_length;
			NightShift ();
			return;
		}
	}

	public void StopShift(){
		shift_started = false;

		if (curr_state == State.DAY_SHIFT) {
			//start lunch

			Debug.Log("Lunch");
			StartShift (State.LUNCH);
			return;
		}

		if (curr_state == State.LUNCH) {
			//start night shift

			Debug.Log("Night shift");
			StartShift(State.NIGHT_SHIFT);
			return;
		} 

		if (curr_state == State.NIGHT_SHIFT) {
			//prompt to choose employees again

			Debug.Log("Please assign 2 employees");
			return;
		}
	}
}