﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskAssign : MonoBehaviour {

	public GameObject emp1;
	public GameObject emp2;
	public ArrayList existingTasks;
	public Button doneButton;
	public GameObject store;
	public GameObject employeeManger;

	private Toggle emp1_toggle_1;
	private Toggle emp1_toggle_2;
	private Toggle emp1_toggle_3;
	private Toggle emp2_toggle_1;
	private Toggle emp2_toggle_2;
	private Toggle emp2_toggle_3;
	private Text temp_text;
	private ArrayList my_employees;

	// Create different tasks
	void Awake() {
		// Initialize the entire task list
		existingTasks = new ArrayList();

		// Create and store tasks
		Task task1 = new Task ();
		task1.task_name = "Make a cupcake";
		task1.time = 30;
		existingTasks.Add (task1);

		Task task2 = new Task ();
		task2.task_name = "Sell a cupcake";
		task2.time = 30;
		existingTasks.Add (task2);

		Task task3 = new Task ();
		task3.task_name = "Deliver a cupcake";
		task3.time = 30;
		existingTasks.Add (task3);
	}

	// Use this for initialization
	void Start () {

		// Retrieve myEmployees ArrayList from EmployeeManager.cs - render profile images
		my_employees = employeeManger.GetComponent<EmployeeManager>().myEmployees;
		Image img1 = emp1.GetComponent<Image> ();
		//img1 = my_employees[0].

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
		// Wait for Done Button to be clicked - Store tasks to employees, close screens
		//if (emp1_toggle_1.isOn) emp1.tasksNotCompleted

		store.GetComponent<Store> ().HideTaskAssign ();
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
}
