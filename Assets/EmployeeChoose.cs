using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeChoose : MonoBehaviour {

	public GameObject employeeManager;
	private ArrayList allEmployeesList;

	// Use this for initialization
	void Start () {
		// Render all possible employees available
		allEmployeesList = employeeManager.GetComponent<EmployeeManager>().allEmployees;
		foreach (Employee e in allEmployeesList) {
			e.getName ();
			e.getMorale();
			e.getImage();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
