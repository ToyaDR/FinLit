using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour {

    private ArrayList allEmployees;
    private ArrayList myEmployees;

    void Awake() {
        allEmployees = new ArrayList();
        // At the start of the game, create all possible instances of employees
        Employee employee_one = new Employee();
        employee_one.setName("Abby");
        employee_one.setMorale(10);
        //employee_one.setImage()
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
}
