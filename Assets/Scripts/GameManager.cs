using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	/* Public and need to be assigned in scene */
	/* Panels */
	public GameObject EmployeeManager;
	public GameObject EmployeeChoose;
	public GameObject TaskAssign;
	public GameObject WeekPanel;
	public GameObject IngredientsPanel;

	public Store store;
	public GameObject door;
	public GameObject BreakRoom;
	public Fungus.Flowchart emp1_flowchart;
	public Fungus.Flowchart emp2_flowchart;
	public GameObject start;
	public GameObject emp1_store;
	public GameObject emp2_store;
	public Slider emp1_energy;
	public Slider emp2_energy;
	public GameObject Breads;
	public GameObject feedback_text_1; // child feedback text for employee 1
	public GameObject feedback_text_2; // child feedback text for employee 2

	/* Public for debugging purposes */
	public int days_since_start;
	public State curr_state;

	/* Private */
	private float shift_length;
	private float lunch_length;

	private float start_time;
	private float time_elapsed;
	private float sell_freq;
	private float emp1_sell_freq;
	private float emp2_sell_freq;
	private float emp1_make_freq;
	private float emp2_make_freq;
	private float sell_freq_elapsed;
	private float emp1_sell_freq_elapsed;
	private float emp2_sell_freq_elapsed;
	private float emp1_make_freq_elapsed;
	private float emp2_make_freq_elapsed;

	private bool shift_started;
	private bool lunch;

	private EmployeeManager employee_manager;
	private TaskAssign task_assign;
	private float currEnergy;
	private float maxEnergy;

	private Dictionary <string, GameObject> bread_list;
	private string curr_bread = "100";

	/* Sound effects */
	private AudioSource collectSound;

	private Vector3 touchPosWorld;

	public enum State{
		DAY_SHIFT,
		LUNCH,
		NIGHT_SHIFT
	}
		
	private int MAX_PRODUCT = 10;

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

		// Initialize energy for sliders
		currEnergy = shift_length;
		maxEnergy = shift_length;

		// Hide sliders before shift starts
		HideSliders();
		HideWeek();
		HideIngredients ();
		HideEmployeeChoose();
		HideTaskAssign ();
	
		bread_list = new Dictionary<string, GameObject>();
		foreach (Transform bread in Breads.transform) {
			string key = Regex.Match(bread.gameObject.name, @"\d+").Value;
			bread_list.Add (key, bread.gameObject);

			//hide all bread first
			if (key != "100") {
				Color col = bread.GetComponent<SpriteRenderer> ().color;
				col.a = 0f;
				bread.GetComponent<SpriteRenderer> ().color = col;
			}
		}

		// SELL
		sell_freq = shift_length / (float)(store.GetReputation ());
		sell_freq_elapsed = sell_freq;

		emp1_sell_freq = sell_freq;
		emp2_sell_freq = sell_freq;
		emp1_sell_freq_elapsed = emp1_sell_freq;
		emp2_sell_freq_elapsed = emp2_sell_freq;

		// MAKE
		emp1_make_freq = 10f; //default
		emp2_make_freq = 10f; //default
		emp1_make_freq_elapsed = emp1_make_freq;
		emp2_make_freq_elapsed = emp2_make_freq;

		// Get audio source component
		collectSound = store.GetComponent<AudioSource>();

	}

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
					ShowIngredients ();
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
		
	/* Shift functions */
	public IEnumerator Shift() {
		Task emp1_curr_task = (Task) ((Employee)employee_manager.myEmployees [0]).tasksNotCompleted[0];
		Task emp2_curr_task = (Task) ((Employee)employee_manager.myEmployees [1]).tasksNotCompleted[0];
		int num_employees_selling = 0;
		if (emp1_curr_task.task_name == "Sell")
			num_employees_selling++;
		if (emp2_curr_task.task_name == "Sell")
			num_employees_selling++;

		// Update energy bar (above employees' heads)
		while (shift_started) {
			if (time_elapsed > 0) {
				currEnergy = time_elapsed;
				emp1_energy.value = currEnergy / maxEnergy;
				emp2_energy.value = currEnergy / maxEnergy;


				if (emp1_curr_task.task_name == "Sell" || emp2_curr_task.task_name == "Sell") {
					// If emp 1 is supposed to sell,
					if (emp1_curr_task.task_name == "Sell") {
						if (emp1_sell_freq_elapsed <= 0) {
							store.Sell ();
							emp1_sell_freq_elapsed = emp1_sell_freq;
							// Display 'cent+1'
						}
					}
					// If emp 2 is supposed to sell,
					if (emp2_curr_task.task_name == "Sell") {
						if (emp2_sell_freq_elapsed <= 0) {
							store.Sell ();
							emp2_sell_freq_elapsed = emp2_sell_freq;
							// Display 'cent+1'

						}
					}
					SwitchBread ();
					emp1_sell_freq_elapsed--;
				}

				// If emp 1 is supposed to make,
				if (emp1_curr_task.task_name == "Make") {
					if (emp1_make_freq_elapsed <= 0) {
						store.Make ();
						emp1_make_freq_elapsed = emp1_make_freq;
						// Display 'bread+1'

					}
					emp1_make_freq_elapsed--;
				}
				// If emp 2 is supposed to make,
				if (emp2_curr_task.task_name == "Make") {
					if (emp2_make_freq_elapsed <= 0) {
						store.Make ();
						emp2_make_freq_elapsed = emp2_make_freq;
						// Display 'bread+1'

					}
					emp2_make_freq_elapsed--;
				}

				yield return new WaitForSeconds(.75f);
			}
			else yield return null;
		}
	}

	public void Lunch() {
	}

	public void StartShift(State state){
		start_time = Time.time;
		shift_started = true;
		curr_state = state;
		emp1_make_freq = shift_length / ((Employee)employee_manager.myEmployees [0]).GetMorale ();
		emp2_make_freq = shift_length / ((Employee)employee_manager.myEmployees [1]).GetMorale ();

		emp1_make_freq_elapsed = emp1_make_freq;
		emp2_make_freq_elapsed = emp2_make_freq;

		//Shift logic
		if (state == State.DAY_SHIFT) {
			collectSound.Play ();
			ShowSliders ();
			time_elapsed = shift_length; //initialize time_elapsed every shift
			StartCoroutine ("Shift");
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
			StartCoroutine ("Shift");
			return;
		}
	}

	public void StopShift(){
		collectSound.Stop ();
		HideSliders ();
		shift_started = false;

		Employee emp1 = (Employee)employee_manager.myEmployees [0];
		Employee emp2 = (Employee)employee_manager.myEmployees [1];
		Task emp1_curr_task = (Task) emp1.tasksNotCompleted [0];
		Task emp2_curr_task = (Task) emp1.tasksNotCompleted [0];

		if (curr_state == State.DAY_SHIFT) {
			//start lunch
			//remove tasks from arraylist for each employee
			emp1.tasksNotCompleted.Remove(emp1_curr_task);
			emp2.tasksNotCompleted.Remove(emp2_curr_task);

			StopCoroutine("Shift");
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
			StopCoroutine("Shift");

			//remove tasks from arraylist for each employee
			emp1.tasksNotCompleted.Remove(emp1_curr_task);
			emp2.tasksNotCompleted.Remove(emp2_curr_task);

			days_since_start++;

			foreach (Employee e in employee_manager.allEmployees) {
				e.days_since_interaction++;

				/* Add employees to the list of employees that worked
				 * during this week and update how often they worked */
				if (!employee_manager.weekEmployees.ContainsKey (e)) {
					employee_manager.weekEmployees.Add (e, 1);
				} else {
					employee_manager.weekEmployees [e]++;
				}
			}

			if (days_since_start % 4 == 0) {
				// weekly tasks
				ShowWeek();
				WeekPanel.GetComponent<WeeklyTaskPanel>().ShowEmployees ();
				return;
			}

			// update days since interaction for all employees

			Debug.Log("Please assign 2 employees");
			ShowEmployeeChoose ();
			return;
		}
	}

	/* Flowchart function */
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



	private void SwitchBread(){
		float percent = ((float)store.GetProductAmount ()) * 100f / ((float) MAX_PRODUCT);
		if (percent < 25f && curr_bread != "0") {
			//Hide old bread
			HideBread();

			//Show new bread and set curr_bread
			curr_bread = "0";
			ShowBread ();
			return;
		}

		if (percent < 50f && percent >= 25f && curr_bread != "25") {
			//Hide old bread
			HideBread();

			//Show new bread and set curr_bread
			curr_bread = "25";
			ShowBread ();
			return;
		}

		if (percent < 75f && percent >= 50f && curr_bread != "50") {
			//Hide old bread
			HideBread();

			//Show new bread and set curr_bread
			curr_bread = "50";
			ShowBread ();
			return;
		}

		if (percent < 100f && percent >= 75 && curr_bread != "75") {
			//Hide old bread
			HideBread ();

			//Show new bread and set curr_bread
			curr_bread = "75";
			ShowBread ();
			return;
		}

		if (percent >= 100f && curr_bread != "100") {
			//Hide old bread
			HideBread();

			//Show new bread and set curr_bread
			curr_bread = "100";
			ShowBread ();
			return;
		}
	}

	/* Show and Hide functions */
	private void ShowBread(){
		Color new_col = bread_list[curr_bread].GetComponent<SpriteRenderer>().color;
		new_col.a = 100f;
		bread_list [curr_bread].GetComponent<SpriteRenderer> ().color = new_col;
	}

	private void HideBread(){
		Color old_col = bread_list[curr_bread].GetComponent<SpriteRenderer>().color;
		old_col.a = 0f;
		bread_list [curr_bread].GetComponent<SpriteRenderer> ().color = old_col;
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
		ShowEmployeeChoose ();
	}

	public void HideSliders() {
		emp1_energy.GetComponent<CanvasGroup> ().alpha = 0f;
		emp1_energy.GetComponent<CanvasGroup>().blocksRaycasts = false;
		emp2_energy.GetComponent<CanvasGroup> ().alpha = 0f;
		emp2_energy.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void HideFeedbackText() {
		feedback_text_1.GetComponent<CanvasGroup> ().alpha = 0f;
		feedback_text_1.GetComponent<CanvasGroup>().blocksRaycasts = false;
		feedback_text_2.GetComponent<CanvasGroup> ().alpha = 0f;
		feedback_text_2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void ShowSliders() {
		emp1_energy.GetComponent<CanvasGroup> ().alpha = 1f;
		emp1_energy.GetComponent<CanvasGroup>().blocksRaycasts = true;
		emp2_energy.GetComponent<CanvasGroup> ().alpha = 1f;
		emp2_energy.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public void ShowFeedbackText() {
		feedback_text_1.GetComponent<CanvasGroup> ().alpha = 1f;
		feedback_text_1.GetComponent<CanvasGroup>().blocksRaycasts = true;
		feedback_text_2.GetComponent<CanvasGroup> ().alpha = 1f;
		feedback_text_2.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Hide task assign panel
	public void HideTaskAssign() {
		TaskAssign.GetComponent<CanvasGroup>().alpha = 0f;
		TaskAssign.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Show employee choose panel
	public void ShowEmployeeChoose() {
		EmployeeChoose.GetComponent<CanvasGroup>().alpha = 1f;
		EmployeeChoose.GetComponent<CanvasGroup>().blocksRaycasts = true;
		EmployeeChoose.GetComponent<EmployeeChoose> ().ResetToggles ();
	}

	// Hide ingredients
	public void HideEmployeeChoose() {
		EmployeeChoose.GetComponent<CanvasGroup>().alpha = 0f;
		EmployeeChoose.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Show ingredients
	public void ShowIngredients() {
		IngredientsPanel.GetComponent<CanvasGroup>().alpha = 1f;
		IngredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Hide ingredients
	public void HideIngredients() {
		IngredientsPanel.GetComponent<CanvasGroup>().alpha = 0f;
		IngredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
}