using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeChoose : MonoBehaviour {

	public GameObject employeeManager;
	private ArrayList allEmployeesList;
	public Button nextButton;
	public GameObject emp1;
	public GameObject emp2;
	public GameObject emp3;
	public GameObject taskAssignPanel;

	// Use this for initialization
	void Start () {
		
		allEmployeesList = employeeManager.GetComponent<EmployeeManager>().allEmployees;
		int employeeNum = 1;

		// Render all possible employees available
		foreach (Employee e in allEmployeesList) {
			
			GameObject currEmployee = GameObject.Find ("Employee" + employeeNum.ToString ());
			Text description = currEmployee.transform.Find ("Description").GetComponent<Text> ();
			description.text = "Name: " + e.GetName () + "  Morale: " + e.GetMorale().ToString();

			employeeNum += 1;
		}

		// Add next button listener
		nextButton.onClick.AddListener(OnClickNext);

		// Add toggle change listener
		//Toggle toggle_1 = emp1.GetComponent<Toggle> ();
		//toggle_1.onValueChanged.AddListener(delegate {
		//	ToggleChanged(toggle_1);
		//});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//void ToggleChanged(Toggle toggle) {
		// Render square around the employee when user clicks on them
		// if (toggle.isOn()) toggle.gameObject.GetComponent<
	//}

	void OnClickNext() {
		// Wait for Next button to be clicked: Store which employees user chose, render task assignment screen

		// Find out which employees user chose
		if (emp1.GetComponent<Toggle> ().isOn) {
			employeeManager.GetComponent<EmployeeManager>().AddToMyEmployee ((Employee)(allEmployeesList [0]));
		}
		if (emp2.GetComponent<Toggle> ().isOn) {
			employeeManager.GetComponent<EmployeeManager>().AddToMyEmployee ((Employee)(allEmployeesList [1]));
		}
		if (emp3.GetComponent<Toggle> ().isOn) {
			employeeManager.GetComponent<EmployeeManager>().AddToMyEmployee ((Employee)(allEmployeesList [2]));
		}

		// Render next screen
		taskAssignPanel.GetComponent<CanvasGroup>().alpha = 1f;
		taskAssignPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}
}
