using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	int days_since_start;
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
	public GameObject WeekPanel;
	public Fungus.Flowchart emp1_flowchart;
	public Fungus.Flowchart emp2_flowchart;
	public GameObject start;
	// public SimpleHealthBar healthBar; //using SimpleHealthBar plugin
	public GameObject emp1_store;
	public GameObject emp2_store;
	public Slider emp1_energy;
	public Slider emp2_energy;

	private EmployeeManager employee_manager;
	private TaskAssign task_assign;
	private float currEnergy;
	private float maxEnergy;

	// Sound effects
	AudioSource collectSound;

	Vector3 touchPosWorld;

	public enum State{
		DAY_SHIFT,
		LUNCH,
		NIGHT_SHIFT
	}

	State curr_state;

	// Use this for initialization
	void Start () {
		days_since_start = 0;
		Time.timeScale = 1.0f;

		curr_state = State.DAY_SHIFT;
		shift_length = 60f*2f; //2 minutes
		lunch_length = 60f*1f; //1 minute
		time_elapsed = shift_length; // count down to end of shift
		shift_started = false;

		employee_manager = EmployeeManager.GetComponent<EmployeeManager> ();
		task_assign = TaskAssign.GetComponent<TaskAssign> ();

		StartCoroutine ("Sale");

		// Sliders for 2 employees, and players can't change these
		//emp1_energy.enabled = false;
		//emp2_energy.enabled = false;

		// Initialize energy for sliders
		currEnergy = shift_length;
		maxEnergy = shift_length;

		// Hide sliders before shift starts
		HideSliders();

		HideWeek();

		// Get audio source component
		collectSound = store.GetComponent<AudioSource>();

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
				}
			}
		}

		// Check if shift has ended
		if (shift_started) {
			time_elapsed -= Time.deltaTime;
			if (time_elapsed <= 0) {
				StopShift ();
			}
			//TODO: Check if player wants to pause

			HideDoor ();
			HideEmployee (task_assign.emp1_store);
			UpdateFlowchart(emp1_flowchart,(Employee)employee_manager.myEmployees [0]);
			ShowEmployee (task_assign.emp1_store);

			HideEmployee (task_assign.emp2_store);
			UpdateFlowchart(emp2_flowchart,(Employee)employee_manager.myEmployees [1]);
			ShowEmployee (task_assign.emp2_store);

			ShowDoor ();
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

	public void HideSliders() {
		emp1_energy.GetComponent<CanvasGroup> ().alpha = 0f;
		emp1_energy.GetComponent<CanvasGroup>().blocksRaycasts = false;
		emp2_energy.GetComponent<CanvasGroup> ().alpha = 0f;
		emp2_energy.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void ShowSliders() {
		emp1_energy.GetComponent<CanvasGroup> ().alpha = 1f;
		emp1_energy.GetComponent<CanvasGroup>().blocksRaycasts = true;
		emp2_energy.GetComponent<CanvasGroup> ().alpha = 1f;
		emp2_energy.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public IEnumerator DayShift() {
		
		//healthBar.UpdateBar( currEnergy, maxEnergy ); - using healthBar

		// Update energy bar (above employees' heads)
		while (true) {
			if (time_elapsed > 0) {
				Debug.Log (time_elapsed);
				currEnergy = time_elapsed;
				emp1_energy.value = currEnergy / maxEnergy;
				emp2_energy.value = currEnergy / maxEnergy;
				yield return new WaitForSeconds(1);
			}
			else yield return null;
		}

		//emp1_energy.transform.position = emp1_store.transform.position;
		//emp2_energy.transform.position = emp2_store.transform.position;
	}

	public void Lunch() {
	}

	public IEnumerator NightShift() {

		// Update energy bar (above employees' heads)
		while (true) {
			if (time_elapsed > 0) {
				Debug.Log (time_elapsed);
				currEnergy = time_elapsed;
				emp1_energy.value = currEnergy / maxEnergy;
				emp2_energy.value = currEnergy / maxEnergy;
				yield return new WaitForSeconds(1);
			}
			else yield return null;
		}
	}

	public void StartShift(State state){
		start_time = Time.time;
		shift_started = true;
		curr_state = state;

		//Shift logic
		if (state == State.DAY_SHIFT) {
			collectSound.Play ();
			ShowSliders ();
			time_elapsed = shift_length; //initialize time_elapsed every shift
			StartCoroutine ("DayShift");
			return;
		}

		if (state == State.LUNCH) {
			time_elapsed = lunch_length;
			Lunch ();
			return;
		}

		if (state == State.NIGHT_SHIFT) {
			collectSound.Play ();
			ShowSliders ();
			time_elapsed = shift_length;
			StartCoroutine ("NightShift");
			return;
		}
	}

	public void StopShift(){
		collectSound.Stop ();
		HideSliders ();
		shift_started = false;

		if (curr_state == State.DAY_SHIFT) {
			//start lunch

			StartShift (State.LUNCH);
			return;
		}

		if (curr_state == State.LUNCH) {
			//start night shift

			StartShift(State.NIGHT_SHIFT);
			return;
		} 

		if (curr_state == State.NIGHT_SHIFT) {
			//prompt to choose employees again
			days_since_start++;

			if (days_since_start % 7 == 0) {
				// weekly tasks
				ShowWeek();
			}

			// update days since interaction for all employees
			foreach (Employee e in employee_manager.allEmployees) {
				e.days_since_interaction++;
			}

			Debug.Log("Please assign 2 employees");
			return;
		}
	}

	public void UpdateFlowchart(Fungus.Flowchart flowchart, Employee emp){
		Fungus.StringVariable option = (Fungus.StringVariable) flowchart.GetVariable ("option");
		Fungus.BooleanVariable done = (Fungus.BooleanVariable) flowchart.GetVariable ("done");
		Fungus.BooleanVariable lunch = (Fungus.BooleanVariable) flowchart.GetVariable ("lunch");

		if (curr_state == State.LUNCH) {
			lunch.Value = true;
		}
		bool can_interact = (emp.GetMinDaysBetweenInteraction () >= emp.days_since_interaction);

		if(option.Evaluate(Fungus.CompareOperator.NotEquals, "default") && can_interact){
			bool not_done = true;
			if (option.Evaluate (Fungus.CompareOperator.Equals, "bad")) {
				not_done = emp.SetCurrQuestion ("bad");
			} else if (option.Evaluate (Fungus.CompareOperator.Equals, "good")) {
				not_done = emp.SetCurrQuestion ("good");
			}
			if (not_done) {
				employee_manager.SetFlowchart (emp.curr_question, flowchart.FindBlock ("Question").CommandList);
			}
			option.Value = "default";
			done.Value = !not_done;
			emp.days_since_interaction = 0;
		}
	}

	public void ShowEmployee(GameObject emp){
		emp.GetComponent<Fungus.Clickable2D> ().ClickEnabled = true;
	}
	public void HideEmployee(GameObject emp){
		emp.GetComponent<Fungus.Clickable2D> ().ClickEnabled = false;
	}

	public void ShowDoor(){
		door.GetComponent<BoxCollider2D> ().enabled = true;
	}
	public void HideDoor(){
		door.GetComponent<BoxCollider2D> ().enabled = false;
	}
		
	public void ShowWeek() {
		WeekPanel.GetComponent<CanvasGroup>().alpha = 1f;
		WeekPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
	public void HideWeek() {
		WeekPanel.GetComponent<CanvasGroup>().alpha = 0f;
		WeekPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
}