using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskAssign : MonoBehaviour {

	public GameObject emp1;
	public GameObject emp2;
	public ArrayList existingTasks;
	public Button doneButton;
	public GameObject store;
	public GameObject employeeManager;
	public GameObject game_manager;
	public int start_task_time;
	public bool taskAssignClosed;

	private Toggle emp1_toggle_1;
	private Toggle emp1_toggle_2;
	private Toggle emp1_toggle_3;
	private Toggle emp2_toggle_1;
	private Toggle emp2_toggle_2;
	private Toggle emp2_toggle_3;
	private Text temp_text;
	private ArrayList my_employees;
	private Task task1;
	private Task task2;
	private Task task3;

	public GameObject emp1_store;
	public GameObject emp2_store;

	// Create different tasks
	void Awake() {
		// Initialize the entire task list
		existingTasks = new ArrayList();

		// Create and store tasks
		task1 = new Task ();
		task1.task_name = "Make";
		task1.time = 30;
		existingTasks.Add (task1);

		task2 = new Task ();
		task2.task_name = "Sell";
		task2.time = 30;
		existingTasks.Add (task2);

		task3 = new Task ();
		task3.task_name = "Deliver";
		task3.time = 30;
		existingTasks.Add (task3);

		taskAssignClosed = false;
	}

	// Use this for initialization
	void Start () {

		// Retrieve all the objects that are needed
		my_employees = employeeManager.GetComponent<EmployeeManager>().myEmployees;

		// Add toggle change listeners - whenever toggle is changed, call ToggleChanged()

		// Employee_1
		emp1_toggle_1 = emp1.transform.Find("Toggle_1").GetComponent<Toggle> ();
		emp1_toggle_1.onValueChanged.AddListener(delegate {
			ToggleChanged_1(emp1_toggle_1);
		});
		emp1_toggle_2 = emp1.transform.Find("Toggle_2").GetComponent<Toggle> ();
		emp1_toggle_2.onValueChanged.AddListener(delegate {
			ToggleChanged_1(emp1_toggle_2);
		});
		emp1_toggle_3 = emp1.transform.Find("Toggle_3").GetComponent<Toggle> ();
		emp1_toggle_3.onValueChanged.AddListener(delegate {
			ToggleChanged_1(emp1_toggle_3);
		});

		// Employee_2
		emp2_toggle_1 = emp2.transform.Find("Toggle_1").GetComponent<Toggle> ();
		emp2_toggle_1.onValueChanged.AddListener(delegate {
			ToggleChanged_2(emp2_toggle_1);
		});
		emp2_toggle_2 = emp2.transform.Find("Toggle_2").GetComponent<Toggle> ();
		emp2_toggle_2.onValueChanged.AddListener(delegate {
			ToggleChanged_2(emp2_toggle_2);
		});
		emp2_toggle_3 = emp2.transform.Find("Toggle_3").GetComponent<Toggle> ();
		emp2_toggle_3.onValueChanged.AddListener(delegate {
			ToggleChanged_2(emp2_toggle_3);
		});

		// Add next button listener
		doneButton.onClick.AddListener(OnClickDone);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnClickDone() {
		// Wait for Done Button to be clicked - Add tasks to employees, close screens
		if (emp1_toggle_1.isOn) ((Employee)(my_employees[0])).tasksNotCompleted.Add(task1);
		if (emp1_toggle_2.isOn) ((Employee)(my_employees[0])).tasksNotCompleted.Add(task2);
		if (emp1_toggle_3.isOn) ((Employee)(my_employees[0])).tasksNotCompleted.Add(task3);
		if (emp2_toggle_1.isOn) ((Employee)(my_employees[1])).tasksNotCompleted.Add(task1);
		if (emp2_toggle_2.isOn) ((Employee)(my_employees[1])).tasksNotCompleted.Add(task2);
		if (emp2_toggle_3.isOn) ((Employee)(my_employees[1])).tasksNotCompleted.Add(task3);

		GameManager gm = game_manager.GetComponent<GameManager> ();
		gm.StartShift (GameManager.State.DAY_SHIFT);
		game_manager.GetComponent<GameManager>().HideTaskAssign ();
		game_manager.GetComponent<GameManager> ().ShowEmployeePanel();

		// Render the 2 chosen employees

		/* Render employee 1 and set initial flowchart information */
		SpriteRenderer img1 = emp1_store.GetComponent<SpriteRenderer>();
		img1.sprite = ((Employee)(my_employees[0])).GetImage ();
		//Image emp1_img = emp1_store.GetComponent<Image>();
		//emp1_img.sprite = ((Employee)(my_employees[0])).GetImage ();

		/* TODO: uncomment this once dialogue bugs are fixed
		SpriteRenderer img1_dialogue = emp1_store.transform.Find ("DialogueSprite").GetComponent<SpriteRenderer>();
		img1_dialogue.sprite = ((Employee)(my_employees[0])).GetDialogueImage ();
		*/

		DTreeNode currq_emp1 = ((Employee) my_employees[0]).curr_question;
		List<Fungus.Command> qCommands_emp1 = gm.emp1_flowchart.FindBlock("Question").CommandList;
		employeeManager.GetComponent<EmployeeManager>().SetFlowchart(currq_emp1, qCommands_emp1);

		/* Render employee 2 */
		SpriteRenderer img2 = emp2_store.GetComponent<SpriteRenderer>();
		img2.sprite = ((Employee)(my_employees[1])).GetImage ();
		/*
		SpriteRenderer img2_dialogue = emp2_store.transform.Find ("DialogueSprite").GetComponent<SpriteRenderer>();
		img2_dialogue.sprite = ((Employee)(my_employees[1])).GetDialogueImage ();
		*/

		DTreeNode currq_emp2 = ((Employee) my_employees[1]).curr_question;
		List<Fungus.Command> qCommands_emp2 = gm.emp2_flowchart.FindBlock("Question").CommandList;
		employeeManager.GetComponent<EmployeeManager>().SetFlowchart(currq_emp2, qCommands_emp2);


		taskAssignClosed = true;
	}

	void ToggleChanged_1(Toggle toggle) {
		// Count the number of chosen employees - if it's 3, turn the toggle back off
		if (emp1_toggle_1.isOn && emp1_toggle_2.isOn && emp1_toggle_3.isOn) {
			toggle.isOn = false;
		}
	}

	void ToggleChanged_2(Toggle toggle) {
		// Count the number of chosen employees - if it's 3, turn the toggle back off
		if (emp2_toggle_1.isOn && emp2_toggle_2.isOn && emp2_toggle_3.isOn) {
			toggle.isOn = false;
		}
	}

	public void ResetToggles(){
		emp1_toggle_1.isOn = false;
		emp1_toggle_2.isOn = false;
		emp1_toggle_3.isOn = false;
		emp2_toggle_1.isOn = false;
		emp2_toggle_2.isOn = false;
		emp2_toggle_3.isOn = false;
	}
}
