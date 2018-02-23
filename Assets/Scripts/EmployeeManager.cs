using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour {

    public ArrayList allEmployees;
    private ArrayList myEmployees;

    void Awake() {
        allEmployees = new ArrayList();

        // At the start of the game, create all possible instances of employees
        Employee employee_one = new Employee();
        employee_one.setName("Abby");
        employee_one.setMorale(10);
		employee_one.setImage ("char1");
		allEmployees.Add (employee_one);

		Employee employee_two = new Employee();
		employee_two.setName("Bob");
		employee_two.setMorale(10);
		employee_two.setImage ("char2");
		allEmployees.Add (employee_two);

		Employee employee_three = new Employee();
		employee_three.setName("Lila");
		employee_three.setMorale(10);
		employee_three.setImage ("Employee_Lila");
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

    void addToMyEmployee(Employee employee) {
        myEmployees.Add(employee);
    }

	void initializeEmployeeList() {
		myEmployees = new ArrayList ();
	}
}
