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
	public GameObject door;
	public GameObject BreakRoom;
	public GameObject EmployeeManager;
	public GameObject TaskAssign;
	public Fungus.Flowchart emp1_flowchart;
	public Fungus.Flowchart emp2_flowchart;
	public GameObject start;

	private EmployeeManager employee_manager;
	private TaskAssign task_assign;

	Vector3 touchPosWorld;

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

		employee_manager = EmployeeManager.GetComponent<EmployeeManager> ();
		task_assign = TaskAssign.GetComponent<TaskAssign> ();

		StartCoroutine ("Sale");

	}
	
	// Update is called once per frame
	void Update () {
		/* Check for touch input */
		if (Input.touchCount == 1 
			&& Input.GetTouch (0).phase == TouchPhase.Stationary) {

			// transform touch position to world space
			touchPosWorld = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			Vector2 touchPosWorld2D = new Vector2 (touchPosWorld.x, touchPosWorld.y);

			//raycast
			RaycastHit2D hitInfo = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

			if (hitInfo.collider != null) {
				GameObject touchedObject = hitInfo.transform.gameObject;

				if (touchedObject == door) {
					store.ShowIngredients ();
				} else if (touchedObject == start) {
					store.ShowEmployeeChoose ();
				} else if (touchedObject == TaskAssign.GetComponent<TaskAssign> ().emp1_store) {

					// TODO: Check if null
					// Render bigger sprite

					task_assign.emp1_store.GetComponent<Fungus.Clickable2D> ().ClickEnabled = false;

					Employee emp1 = (Employee)employee_manager.myEmployees [0];
					Fungus.StringVariable option = (Fungus.StringVariable) emp1_flowchart.GetVariable("option");

					if (option.Evaluate (Fungus.CompareOperator.Equals, "bad")) {
						emp1.SetCurrQuestion ("bad");
					} else if (option.Evaluate (Fungus.CompareOperator.Equals, "good")) {
						emp1.SetCurrQuestion ("good");
					} else{
						emp1.SetCurrQuestion("");
						// TODO: get financial advisor
					} 

					DTreeNode currq_emp1 = ((Employee) employee_manager.myEmployees[0]).curr_question;
					List<Fungus.Command> qCommands_emp1 = emp1_flowchart.FindBlock("Question").CommandList;	
					employee_manager.SetFlowchart (currq_emp1, qCommands_emp1);
					task_assign.emp1_store.GetComponent<Fungus.Clickable2D> ().ClickEnabled = true;

				} else if (touchedObject == TaskAssign.GetComponent<TaskAssign> ().emp2_store) {
				}
			}
		}

		//Check if shift has ended
		if (shift_started) {
			//TODO: Check if player wants to pause


			time_elapsed -= Time.deltaTime;

			if (time_elapsed <= 0) {
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
	}

	public void NightShift() {
	}

	public void StartShift(State state){
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