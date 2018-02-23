using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeChoose : MonoBehaviour {

	public GameObject employeeManager;
	private ArrayList allEmployeesList;

	// Use this for initialization
	void Start () {
		// Render all possible employees available
		allEmployeesList = employeeManager.GetComponent<EmployeeManager>().allEmployees;
		int employeeNum = 1;

		foreach (Employee e in allEmployeesList) {
			
			GameObject currEmployee = GameObject.Find ("Employee" + employeeNum.ToString ());
			Text description = currEmployee.transform.Find ("Description").GetComponent<Text> ();
			description.text = "Name: " + e.getName () + "  Morale: " + e.getMorale().ToString();
			Image image = currEmployee.AddComponent<Image> () as Image;
			image.sprite = e.getImage().sprite;

			employeeNum += 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
