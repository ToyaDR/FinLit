using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour {

	public GameObject employee1; // employee objects in the store
	public GameObject employee2;
	public GameObject employeeManager;
	public GameObject taskAssignPanel;

	private Text timer_1;
	private Text timer_2;
	private ArrayList my_employees;
	private int start_task_time;
	private int timer;
	private bool taskAssigned;

	// Use this for initialization
	void Start () {
		// Retrieve UI timer text from employee objects
		timer_1 = employee1.transform.Find("Text").GetComponent<Text>();
		timer_2 = employee2.transform.Find("Text").GetComponent<Text>();

		// Retrieve my employees information
		my_employees = employeeManager.GetComponent<EmployeeManager> ().myEmployees;

	}
	
	// Update is called once per frame
	void Update () {

		// Update task timer on top of each employee - again, only if the employees are already assigned
		if (taskAssignPanel.GetComponent<TaskAssign> ().taskAssignClosed) {
			// Start the timer
			start_task_time = (int)(Time.time);

			if ((Time.time - start_task_time) < 30) {
				timer = (int)(Time.time) - start_task_time;
				timer_1.text = timer.ToString ();
				timer_2.text = timer.ToString ();
			} else {
				timer_1.text = "";
				timer_2.text = "";
			}
		}
	}
}
