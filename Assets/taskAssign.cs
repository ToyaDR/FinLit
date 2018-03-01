using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAssign : MonoBehaviour {

	public ArrayList existingTasks;

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


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
