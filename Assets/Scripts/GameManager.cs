using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public int days_since_start;
	float shift_length;

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

	public GameObject CashRegister;
	public GameObject WorkTable;
	public GameObject LunchTable;

	public Store Store;
	public GameObject Door;
	public GameObject BreakRoom;
	public GameObject EmployeeManager;
	public GameObject EmployeeChoose;
	public GameObject TaskAssign;
	public GameObject EmployeePanel;
	public GameObject IngredientsPanel;
	public GameObject WeekPanel;
	public Fungus.Flowchart Emp1Flowchart;
	public Fungus.Flowchart Emp2Flowchart;
	public GameObject StartButton;
	public Button FinishLunchButton;
	// public SimpleHealthBar healthBar; //using SimpleHealthBar plugin
	public GameObject Emp1Store;
	public GameObject Emp2Store;
	public Slider Emp1Energy;
	public Slider Emp2Energy;
	public GameObject Breads;
	public GameObject FeedbackText1; // child feedback text for employee 1
	public GameObject FeedbackText2; // child feedback text for employee 2
	public GameObject CurrStateText;
	public Camera MainCamera;

	private EmployeeManager employee_manager;
	private TaskAssign task_assign;
	private float curr_energy;
	private float max_energy;
	private Task emp1_curr_task;
	private Task emp2_curr_task;

	private Dictionary <string, GameObject> bread_list;
	private string curr_bread = "100";
	private int sold = 0;
	private bool show = true;

	// Sound effects
	//private AudioSource collect_sound;

	private Vector3 touch_pos_world;

	public enum State{
		DAY_SHIFT,
		LUNCH,
		NIGHT_SHIFT
	}

	public State curr_state;
	private int MAX_PRODUCT = 10;

	// Use this for initialization
	void Start() {
		days_since_start = 0;
		Time.timeScale = 1.0f;

		curr_state = State.DAY_SHIFT;
		shift_length = 5f;//30f;//60f*2f; //2 minutes
		time_elapsed = shift_length; // count down to end of shift
		shift_started = false;

		employee_manager = EmployeeManager.GetComponent<EmployeeManager> ();
		task_assign = TaskAssign.GetComponent<TaskAssign> ();

		// Sliders for 2 employees, and players can't change these
		//emp1_energy.enabled = false;
		//emp2_energy.enabled = false;

		// Initialize energy for sliders
		curr_energy = shift_length;
		max_energy = shift_length;

		// Hide sliders before shift starts
		HideSliders();
		HideFeedbackText ();
		HideEmployeePanel ();
		HideTaskAssign ();
		HideIngredients ();
		HideEmployeeChoose ();
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
		sell_freq = shift_length / (float)(Store.GetReputation ());
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
		//collect_sound = Store.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
		/* Check for touch input */
		if (Input.touchCount == 1 
			&& Input.GetTouch (0).phase == TouchPhase.Stationary) {

			// transform touch position to world space
			touch_pos_world = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
			Vector2 touchPosWorld2D = new Vector2 (touch_pos_world.x, touch_pos_world.y);

			//raycast
			RaycastHit2D hitInfo = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

			if (hitInfo.collider != null) {
				GameObject touchedObject = hitInfo.transform.gameObject;

				if (touchedObject == Door) {
					ShowIngredients ();
				}
			}
		}

		// Check if shift has ended
		if (shift_started) {
			time_elapsed -= Time.deltaTime;
			if (time_elapsed <= 0 && (curr_state != State.LUNCH)) {
				StopShift ();
			}
			//TODO: Check if player wants to pause

			if(employee_manager.myEmployees.Count > 0)
				UpdateFlowchart(Emp1Flowchart,(Employee)employee_manager.myEmployees [0]);
			if(employee_manager.myEmployees.Count > 1)
				UpdateFlowchart(Emp2Flowchart,(Employee)employee_manager.myEmployees [1]);
		}

	}

	public void HideSliders() {
		Emp1Energy.GetComponent<CanvasGroup> ().alpha = 0f;
		Emp1Energy.GetComponent<CanvasGroup>().blocksRaycasts = false;
		Emp2Energy.GetComponent<CanvasGroup> ().alpha = 0f;
		Emp2Energy.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void HideFeedbackText() {
		FeedbackText1.GetComponent<CanvasGroup> ().alpha = 0f;
		FeedbackText1.GetComponent<CanvasGroup>().blocksRaycasts = false;
		FeedbackText2.GetComponent<CanvasGroup> ().alpha = 0f;
		FeedbackText2.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void ShowSliders() {
		Emp1Energy.GetComponent<CanvasGroup> ().alpha = 1f;
		Emp1Energy.GetComponent<CanvasGroup>().blocksRaycasts = true;
		Emp2Energy.GetComponent<CanvasGroup> ().alpha = 1f;
		Emp2Energy.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
		
	public void ShowFeedbackText() {
		FeedbackText1.GetComponent<CanvasGroup> ().alpha = 1f;
		FeedbackText1.GetComponent<CanvasGroup>().blocksRaycasts = true;
		FeedbackText2.GetComponent<CanvasGroup> ().alpha = 1f;
		FeedbackText2.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public IEnumerator Shift() {
		Task emp1_curr_task = (Task) ((Employee)employee_manager.myEmployees [0]).tasksNotCompleted[0];
		Task emp2_curr_task = (Task) ((Employee)employee_manager.myEmployees [1]).tasksNotCompleted[0];

		while (shift_started) {

			CurrStateText.transform.GetChild(0).gameObject.GetComponent<Text>().text = ((int) time_elapsed).ToString();
			if (time_elapsed > 0) {

				// Update energy bar (above employees' heads)
				curr_energy = time_elapsed;
				Emp1Energy.value = curr_energy / max_energy;
				Emp2Energy.value = curr_energy / max_energy;

				// Make feedback texts blink
				// TODO: only blink when items are actually sold or made
				/*
				if (show) {
					HideFeedbackText ();
					show = false;
				} else {
					ShowFeedbackText ();
					show = true;
				}
				*/

				if (emp1_curr_task.task_name == "Sell" || emp2_curr_task.task_name == "Sell") {
					// If emp 1 is supposed to sell,
					if (emp1_curr_task.task_name == "Sell") {
						if (emp1_sell_freq_elapsed <= 0) {
							bool enough = Store.Sell ();
							if (enough) {
								emp1_sell_freq_elapsed = emp1_sell_freq;
								sold++;
								StartCoroutine ("Blink");
							}
						}
						emp1_sell_freq_elapsed--;
					}
					// If emp 2 is supposed to sell,
					if (emp2_curr_task.task_name == "Sell") {
						if (emp2_sell_freq_elapsed <= 0) {
							bool enough = Store.Sell ();
							if (enough) {
								emp2_sell_freq_elapsed = emp2_sell_freq;
								sold++;
								StartCoroutine ("Blink");
							}
						}
						emp2_sell_freq_elapsed--;
					}
					SwitchBread ();
				}

				// If emp 1 is supposed to make,
				if (emp1_curr_task.task_name == "Make") {
					if (emp1_make_freq_elapsed <= 0) {
						bool enough = Store.Make ();
						if (enough) {
							emp1_make_freq_elapsed = emp1_make_freq;
							StartCoroutine ("Blink");
						}
					}
					emp1_make_freq_elapsed--;
				}
				// If emp 2 is supposed to make,
				if (emp2_curr_task.task_name == "Make") {
					if (emp2_make_freq_elapsed <= 0) {
						bool enough = Store.Make ();
						if (enough) {
							emp2_make_freq_elapsed = emp2_make_freq;
							StartCoroutine ("Blink");
						}
					}
					emp2_make_freq_elapsed--;
				}

				yield return new WaitForSeconds(.75f);
			}
			else yield return null;
		}
	}

	public IEnumerator Blink(){
		ShowFeedbackText ();
		yield return new WaitForSeconds (0.50f);
		HideFeedbackText ();
	}

	public void Lunch() {
		//CurrStateText.transform.GetChild (0).gameObject.GetComponent<Text> ().text = ((int) time_elapsed).ToString ();
	}
		
	public IEnumerator PanCameraToLunch() {
		while (MainCamera.transform.position.y < 11.85f) {
			Vector3 oldPos = MainCamera.transform.position;
			MainCamera.transform.position = new Vector3 (oldPos.x, oldPos.y + 0.2f, oldPos.z);

			Vector3 oldPos_emp1 = Emp1Store.transform.position;
			Emp1Store.transform.position = new Vector3 (oldPos_emp1.x, oldPos_emp1.y - 0.2f, oldPos_emp1.z);

			Vector3 oldPos_emp2 = Emp2Store.transform.position;
			Emp2Store.transform.position = new Vector3 (oldPos_emp2.x, oldPos_emp2.y - 0.2f, oldPos_emp2.z);

			Vector3 oldPos_cashregister = CashRegister.transform.position;
			CashRegister.transform.position = new Vector3 (oldPos_cashregister.x, oldPos_cashregister.y - 0.2f, oldPos_cashregister.z);

			Vector3 oldPos_worktable = WorkTable.transform.position;
			WorkTable.transform.position = new Vector3 (oldPos_worktable.x, oldPos_worktable.y - 0.2f, oldPos_worktable.z);

			FinishLunchButton.interactable = true;
			Vector3 oldPos_lunchbutton = FinishLunchButton.transform.position;
			FinishLunchButton.transform.position = new Vector3 (oldPos_lunchbutton.x, oldPos_lunchbutton.y - 0.2f, oldPos_lunchbutton.z);

			Vector3 oldPos_lunchtable = LunchTable.transform.position;
			LunchTable.transform.position = new Vector3 (oldPos_lunchtable.x, oldPos_lunchtable.y - 0.2f, oldPos_lunchtable.z);
			yield return new WaitForSeconds(0.05f);
		}
		Emp1Store.transform.position = LunchTable.transform.position;
		Emp2Store.transform.position = new Vector3 (LunchTable.transform.position.x + 5f, LunchTable.transform.position.y + 5f, 1f);;
		yield return null;
	}

	public IEnumerator PanCameraToWork() {
		while (MainCamera.transform.position.y > 0) {
			Vector3 oldPos = MainCamera.transform.position;
			MainCamera.transform.position = new Vector3 (oldPos.x, oldPos.y - 0.2f, oldPos.z);

			Vector3 oldPos_emp1 = Emp1Store.transform.position;
			Emp1Store.transform.position = new Vector3 (oldPos_emp1.x, oldPos_emp1.y + 0.2f, oldPos_emp1.z);

			Vector3 oldPos_emp2 = Emp2Store.transform.position;
			Emp2Store.transform.position = new Vector3 (oldPos_emp2.x, oldPos_emp2.y + 0.2f, oldPos_emp2.z);

			Vector3 oldPos_cashregister = CashRegister.transform.position;
			CashRegister.transform.position = new Vector3 (oldPos_cashregister.x, oldPos_cashregister.y + 0.2f, oldPos_cashregister.z);

			Vector3 oldPos_worktable = WorkTable.transform.position;
			WorkTable.transform.position = new Vector3 (oldPos_worktable.x, oldPos_worktable.y + 0.2f, oldPos_worktable.z);

			FinishLunchButton.interactable = false;
			Vector3 oldPos_lunchbutton = FinishLunchButton.transform.position;
			FinishLunchButton.transform.position = new Vector3 (oldPos_lunchbutton.x, oldPos_lunchbutton.y + 0.2f, oldPos_lunchbutton.z);

			Vector3 oldPos_lunchtable = LunchTable.transform.position;
			LunchTable.transform.position = new Vector3 (oldPos_lunchtable.x, oldPos_lunchtable.y + 0.2f, oldPos_lunchtable.z);
			yield return new WaitForSeconds(0.05f);
		}
		yield return null;
	}

	// What should happen before each shift starts - position and render everything
	public void PrepareShift() {
		// Retrieve the task each employee has to complete
		emp1_curr_task = (Task) ((Employee)employee_manager.myEmployees [0]).tasksNotCompleted[0];
		emp2_curr_task = (Task) ((Employee)employee_manager.myEmployees [1]).tasksNotCompleted[0];

		Vector3 sell_pos = CashRegister.transform.localPosition;
		Vector3 make_pos = WorkTable.transform.localPosition;

		// If emp1 has to SELL
		if (emp1_curr_task.task_name == "Sell") {
			// Position emp1 at Cash Register
			Emp1Store.transform.localPosition = new Vector3(sell_pos.x+10f, sell_pos.y+30f, sell_pos.z);
			// Display 'money+1'
			Image money_1 = Emp1Store.transform.Find("PlusOne").GetComponent<Image>();
			money_1.sprite = Resources.Load<Sprite> ("UI_1Dollar");
		}
		// If emp1 has to MAKE
		if (emp1_curr_task.task_name == "Make") {
			// Position emp1 at Bread Table
			Emp1Store.transform.localPosition = new Vector3(make_pos.x-20f, make_pos.y, make_pos.z);
			// Display 'bread+1'
			Image bread_1 = Emp1Store.transform.Find("PlusOne").GetComponent<Image>();
			bread_1.sprite = Resources.Load<Sprite> ("UI_BakedGood");
		}
		// If emp2 has to SELL
		if (emp2_curr_task.task_name == "Sell") {
			// Position emp2 at Cash Register
			Emp2Store.transform.localPosition = new Vector3(sell_pos.x+40f, sell_pos.y+30f, sell_pos.z);
			// Display 'money+1'
			Image money_2 = Emp2Store.transform.Find("PlusOne").GetComponent<Image>();
			money_2.sprite = Resources.Load<Sprite> ("UI_1Dollar");
		}
		// If emp1 has to MAKE
		if (emp2_curr_task.task_name == "Make") {
			// Position emp2 at Bread Table
			Emp2Store.transform.localPosition = new Vector3(make_pos.x-60f, make_pos.y, make_pos.z);
			// Display 'bread+1'
			Image bread_2 = Emp2Store.transform.Find("PlusOne").GetComponent<Image>();
			bread_2.sprite = Resources.Load<Sprite> ("UI_BakedGood");
		}

		//collect_sound.Play ();
		ShowSliders ();
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
			StartCoroutine ("PanCameraToWork");
			CurrStateText.GetComponent<Text> ().text = "It's Morning Shift time!";
			PrepareShift ();
			StartCoroutine ("Shift");
			return;
		}

		if (state == State.LUNCH) {
			// Camera pan
			StartCoroutine("PanCameraToLunch");
			CurrStateText.GetComponent<Text> ().text = "It's Lunch Break time!";
			Lunch ();
			return;
		}

		if (state == State.NIGHT_SHIFT) {
			StartCoroutine ("PanCameraToWork");
			CurrStateText.GetComponent<Text> ().text = "It's Afternoon Shift time!";
			PrepareShift ();
			StartCoroutine ("Shift");
			return;
		}
	}

	public void StopShift(){
		//collect_sound.Stop ();
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
			//StopCoroutine("Lunch");
			StartShift(State.NIGHT_SHIFT);
			return;
		} 

		if (curr_state == State.NIGHT_SHIFT) {
			StopCoroutine ("Shift");
			//prompt to choose employees again
		
			//remove tasks from arraylist for each employee
			emp1.tasksNotCompleted.Remove(emp1_curr_task);
			emp2.tasksNotCompleted.Remove(emp2_curr_task);

			days_since_start++;

			foreach (Employee e in employee_manager.allEmployees) {
				e.days_since_interaction++;
			}

			foreach (Employee e in employee_manager.myEmployees) {
				if (!employee_manager.weekEmployees.ContainsKey (e)) {
					employee_manager.weekEmployees.Add (e, 1);
				} else {
					employee_manager.weekEmployees [e]++;
				}
			}


			if (days_since_start % 4 == 0) {
				// weekly tasks
				HideEmployeePanel();
				ShowWeek();
				/* TODO Call sold first */
				int curr_add = 0;
				StartCoroutine (WeekPanel.GetComponent<WeeklyTaskPanel>().AddSoldtoIncome(curr_add, sold));
				sold = 0;
				return;
			}

			// update days since interaction for all employees

			Debug.Log("Please assign 2 employees");
			ShowEmployeeChoose ();
			return;
		}
	}
		
	public void UpdateFlowchart(Fungus.Flowchart flowchart, Employee emp){
		Fungus.StringVariable option = (Fungus.StringVariable) flowchart.GetVariable ("option");
		Fungus.BooleanVariable done = (Fungus.BooleanVariable) flowchart.GetVariable ("done");
		Fungus.BooleanVariable lunch = (Fungus.BooleanVariable) flowchart.GetVariable ("lunch");

		lunch.Value = false;
		if (curr_state == State.LUNCH) {
			lunch.Value = true;
		}

		done.Value = false;
		if (emp.curr_question.IsLeaf ()) {
			done.Value = true;
		}
			
		bool can_interact = (emp.GetMinDaysBetweenInteraction () <= emp.days_since_interaction);

		if(option.Evaluate(Fungus.CompareOperator.NotEquals, "default") && can_interact 
			&& !emp.curr_question.IsLeaf()){
			if (option.Evaluate (Fungus.CompareOperator.Equals, "bad")) {
				emp.SetCurrQuestion ("bad");
			} else if (option.Evaluate (Fungus.CompareOperator.Equals, "good")) {
				emp.SetCurrQuestion ("good");
			} 

			employee_manager.SetFlowchart (emp.curr_question, flowchart.FindBlock ("Question").CommandList);

			option.Value = "default";
			done.Value = emp.curr_question.IsLeaf ();
			emp.days_since_interaction = 0;
		}
	}

	private void SwitchBread(){
		float percent = ((float)Store.GetProductAmount ()) * 100f / ((float) MAX_PRODUCT);
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

	public void ShowEmployeePanel(){
		EmployeePanel.GetComponent<CanvasGroup>().alpha = 1f;
		EmployeePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;

		Emp1Store.GetComponent<Fungus.Clickable2D> ().ClickEnabled = true;
		Emp2Store.GetComponent<Fungus.Clickable2D> ().ClickEnabled = true;
	}
	public void HideEmployeePanel(){
		EmployeePanel.GetComponent<CanvasGroup>().alpha = 0f;
		EmployeePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;

		Emp1Store.GetComponent<Fungus.Clickable2D> ().ClickEnabled = false;
		Emp2Store.GetComponent<Fungus.Clickable2D> ().ClickEnabled = false;
	}

	public void ShowDoor(){
		Door.GetComponent<BoxCollider2D> ().enabled = true;
	}
	public void HideDoor(){
		Door.GetComponent<BoxCollider2D> ().enabled = false;
	}

	public void ShowWeek() {
		WeekPanel.GetComponent<CanvasGroup>().alpha = 1f;
		WeekPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
	public void HideWeek() {
		WeekPanel.GetComponent<CanvasGroup>().alpha = 0f;
		WeekPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	public void ShowTaskAssign() {
		TaskAssign.GetComponent<CanvasGroup>().alpha = 1f;
		TaskAssign.GetComponent<CanvasGroup>().blocksRaycasts = true;

		TaskAssign.GetComponent<TaskAssign> ().ResetToggles ();
	}

	// Hide task assign panel
	public void HideTaskAssign() {
		TaskAssign.GetComponent<CanvasGroup>().alpha = 0f;
		TaskAssign.GetComponent<CanvasGroup>().blocksRaycasts = false;
		ShowEmployeePanel ();
	}

	// Show employee choose panel
	public void ShowEmployeeChoose() {
		EmployeeChoose.GetComponent<CanvasGroup>().alpha = 1f;
		EmployeeChoose.GetComponent<CanvasGroup>().blocksRaycasts = true;

		HideEmployeePanel ();

		EmployeeChoose.GetComponent<EmployeeChoose> ().ResetToggles ();
	}

	// Hide ingredients
	public void HideEmployeeChoose() {
		EmployeeChoose.GetComponent<CanvasGroup>().alpha = 0f;
		EmployeeChoose.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	// Show ingredients
	public void ShowIngredients() {
		StartButton.GetComponent<CanvasGroup> ().alpha = 0f;
		StartButton.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		IngredientsPanel.GetComponent<CanvasGroup>().alpha = 1f;
		IngredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	// Hide ingredients
	public void HideIngredients() {
		IngredientsPanel.GetComponent<CanvasGroup>().alpha = 0f;
		IngredientsPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
		IngredientsPanel.GetComponent<IngredientsPanel> ().HideWarning ();
		ShowEmployeeChoose ();
	}

	public void ResetSold(){
		sold = 0;
	}
}
