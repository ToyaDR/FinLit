using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public int days_since_start;
	float shift_length;
	float lunch_length;

	float start_time;
	float time_elapsed;
	float sell_freq;
	float emp1_sell_freq;
	float emp2_sell_freq;
	float emp1_make_freq;
	float emp2_make_freq;
	float sell_freq_elapsed;
	float emp1_sell_freq_elapsed;
	float emp2_sell_freq_elapsed;
	float emp1_make_freq_elapsed;
	float emp2_make_freq_elapsed;

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
	public GameObject Breads;
	public GameObject feedback_text_1; // child feedback text for employee 1
	public GameObject feedback_text_2; // child feedback text for employee 2

	private EmployeeManager employee_manager;
	private TaskAssign task_assign;
	private float currEnergy;
	private float maxEnergy;
	private Task emp1_curr_task;
	private Task emp2_curr_task;

	private Dictionary <string, GameObject> bread_list;
	private string curr_bread = "100";
	private bool show = true;
	// Sound effects
	AudioSource collectSound;

	Vector3 touchPosWorld;

	public enum State{
		DAY_SHIFT,
		LUNCH,
		NIGHT_SHIFT
	}

	public State curr_state;
	private int MAX_PRODUCT = 10;

	// Use this for initialization
	void Start () {
		days_since_start = 0;
		Time.timeScale = 1.0f;

		curr_state = State.DAY_SHIFT;
		shift_length = 10f;//60f*2f; //2 minutes
		lunch_length = 60f*1f; //1 minute
		time_elapsed = shift_length; // count down to end of shift
		shift_started = false;

		employee_manager = EmployeeManager.GetComponent<EmployeeManager> ();
		task_assign = TaskAssign.GetComponent<TaskAssign> ();

		// Sliders for 2 employees, and players can't change these
		//emp1_energy.enabled = false;
		//emp2_energy.enabled = false;

		// Initialize energy for sliders
		currEnergy = shift_length;
		maxEnergy = shift_length;

		// Hide sliders before shift starts
		HideSliders();
		HideFeedbackText ();

		HideWeek();
	
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

	public IEnumerator Shift() {
		int num_employees_selling = 0;
		if (emp1_curr_task.task_name == "Sell")
			num_employees_selling++;
		if (emp2_curr_task.task_name == "Sell")
			num_employees_selling++;
		
		//healthBar.UpdateBar( currEnergy, maxEnergy ); - using healthBar

		while (shift_started) {
			if (time_elapsed > 0) {
				//Debug.Log (time_elapsed);
				Debug.Log("sellfreq " + sell_freq + " sellfreqelapsed " + sell_freq_elapsed);

				// Update energy bar (above employees' heads)
				currEnergy = time_elapsed;
				emp1_energy.value = currEnergy / maxEnergy;
				emp2_energy.value = currEnergy / maxEnergy;

				// Make feedback texts blink
				if (show) {
					HideFeedbackText ();
					show = false;
				} else {
					ShowFeedbackText ();
					show = true;
				}
				/*
				// If at least one employee is selling,
				if (num_employees_selling > 0) {
					if (sell_freq_elapsed <= 0) {
						Debug.Log ("Here");
						store.Sell ();
						// If both employees are selling, SELL one more time
						if (num_employees_selling == 2) {
							store.Sell ();
						}
						SwitchBread ();
						sell_freq_elapsed = sell_freq;
					}
					sell_freq_elapsed--;
				}
				*/

				// If emp 1 is supposed to sell,
				if (emp1_curr_task.task_name == "Sell") {
					if (emp1_sell_freq_elapsed <= 0) {
						store.Sell ();
						emp1_sell_freq_elapsed = emp1_sell_freq;
					}
					emp1_sell_freq_elapsed--;
				}
				// If emp 2 is supposed to sell,
				if (emp2_curr_task.task_name == "Sell") {
					if (emp2_sell_freq_elapsed <= 0) {
						store.Sell ();
						emp2_sell_freq_elapsed = emp2_sell_freq;
					}
					emp2_sell_freq_elapsed--;
				}

				// If emp 1 is supposed to make,
				if (emp1_curr_task.task_name == "Make") {
					if (emp1_make_freq_elapsed <= 0) {
						store.Make ();
						emp1_make_freq_elapsed = emp1_make_freq;
					}
					emp1_make_freq_elapsed--;
				}
				// If emp 2 is supposed to make,
				if (emp2_curr_task.task_name == "Make") {
					if (emp2_make_freq_elapsed <= 0) {
						store.Make ();
						emp2_make_freq_elapsed = emp2_make_freq;
					}
					emp2_make_freq_elapsed--;
				}

				yield return new WaitForSeconds(.75f);
			}
			else yield return null;
		}

		//emp1_energy.transform.position = emp1_store.transform.position;
		//emp2_energy.transform.position = emp2_store.transform.position;
	}

	public void Lunch() {
	}

	// What should happen before each shift starts - position and render everything
	public void PrepareShift() {
		// Retrieve the task each employee has to complete
		emp1_curr_task = (Task) ((Employee)employee_manager.myEmployees [0]).tasksNotCompleted[0];
		emp2_curr_task = (Task) ((Employee)employee_manager.myEmployees [1]).tasksNotCompleted[0];
		// If emp1 has to SELL
		if (emp1_curr_task.task_name == "Sell") {
			// Position emp1 at Cash Register
			emp1_store.transform.localPosition = new Vector3(107.7f, 25.7f, 0.0f);
			// Display 'money+1'
			Image money_1 = emp1_store.transform.Find("PlusOne").GetComponent<Image>();
			money_1.sprite = Resources.Load<Sprite> ("UI_1Dollar");
		}
		// If emp1 has to MAKE
		if (emp1_curr_task.task_name == "Make") {
			// Position emp1 at Bread Table
			emp1_store.transform.localPosition = new Vector3(-182.71f, -87.7f, 0.0f);
			// Display 'bread+1'
			Image bread_1 = emp1_store.transform.Find("PlusOne").GetComponent<Image>();
			bread_1.sprite = Resources.Load<Sprite> ("UI_BakedGood");
		}
		// If emp2 has to SELL
		if (emp2_curr_task.task_name == "Sell") {
			// Position emp2 at Cash Register
			emp2_store.transform.localPosition = new Vector3(107.7f, 25.7f, 0.0f);
			// Display 'money+1'
			Image money_2 = emp2_store.transform.Find("PlusOne").GetComponent<Image>();
			money_2.sprite = Resources.Load<Sprite> ("UI_1Dollar");
		}
		// If emp1 has to MAKE
		if (emp2_curr_task.task_name == "Make") {
			// Position emp2 at Bread Table
			emp2_store.transform.localPosition = new Vector3(-182.71f, -87.7f, 0.0f);
			// Display 'bread+1'
			Image bread_2 = emp2_store.transform.Find("PlusOne").GetComponent<Image>();
			bread_2.sprite = Resources.Load<Sprite> ("UI_BakedGood");
		}

		collectSound.Play ();
		ShowSliders ();
		ShowFeedbackText ();
		time_elapsed = shift_length; //initialize time_elapsed every shift
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
			PrepareShift ();
			StartCoroutine ("Shift");
			return;
		}

		if (state == State.LUNCH) {
			time_elapsed = lunch_length;
			Lunch ();
			return;
		}

		if (state == State.NIGHT_SHIFT) {
			PrepareShift ();
			StartCoroutine ("Shift");
			return;
		}
	}

	public void StopShift(){
		collectSound.Stop ();
		HideSliders ();
		HideFeedbackText ();
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
		
			//remove tasks from arraylist for each employee
			emp1.tasksNotCompleted.Remove(emp1_curr_task);
			emp2.tasksNotCompleted.Remove(emp2_curr_task);

			days_since_start++;

			foreach (Employee e in employee_manager.allEmployees) {
				e.days_since_interaction++;
			}

			if (days_since_start % 7 == 0) {
				// weekly tasks
				ShowWeek();
				return;
			}

			// update days since interaction for all employees

			Debug.Log("Please assign 2 employees");
			store.ShowEmployeeChoose ();
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
		store.ShowEmployeeChoose ();
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
			HideBread();

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
}
