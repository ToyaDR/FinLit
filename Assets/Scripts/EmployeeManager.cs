using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour {

    public ArrayList allEmployees;
	public ArrayList myEmployees;

    void Awake() {
        allEmployees = new ArrayList();

        // At the start of the game, create all possible instances of employees
        Employee employee_one = new Employee();
        employee_one.SetName("Abby");
        employee_one.SetMorale(10);
		employee_one.SetImage ("char2");
		employee_one.tasksNotCompleted = new ArrayList ();
		allEmployees.Add (employee_one);

		Employee employee_two = new Employee();
		employee_two.SetName("Bob");
		employee_two.SetMorale(10);
		employee_two.SetImage ("char1");
		employee_two.tasksNotCompleted = new ArrayList ();
		allEmployees.Add (employee_two);

		Employee employee_three = new Employee();
		employee_three.SetName("Lila");
		employee_three.SetMorale(10);
		employee_three.SetImage ("Employee_Lila");
		employee_three.tasksNotCompleted = new ArrayList ();
		allEmployees.Add (employee_three);

		// Initialize my employees list
        myEmployees = new ArrayList();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToMyEmployee(Employee employee) {
        myEmployees.Add(employee);
    }

	public void InitializeEmployeeList() {
		myEmployees = new ArrayList ();
	}
}
