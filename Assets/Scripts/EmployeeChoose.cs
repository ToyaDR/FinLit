using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeChoose : MonoBehaviour {

	public GameObject employeeManager;
	public GameObject GameManager;
	private ArrayList allEmployeesList;
	public Button nextButton;
	public GameObject emp1;
	public GameObject emp2;
	public GameObject emp3;
	public GameObject taskAssignPanel;
	public GameObject store;
	public GameObject chosen_emp1_object;
	public GameObject chosen_emp2_object;

	private Toggle toggle_1;
	private Toggle toggle_2;
	private Toggle toggle_3;
	private ArrayList my_employees;

	// Use this for initialization
	void Start () {
		nextButton.interactable = false;
		allEmployeesList = employeeManager.GetComponent<EmployeeManager>().allEmployees;
		int employeeNum = 1;

		// Render all possible employees available
		foreach (Employee e in allEmployeesList) {
			
			GameObject currEmployee = GameObject.Find ("Employee" + employeeNum.ToString ());
			Text description = currEmployee.transform.Find ("Description").GetComponent<Text> ();
			description.text = "Name: " + e.GetName () + "  Morale: " + e.GetMorale().ToString();
			Image img = currEmployee.GetComponent<Image> ();
			img.sprite = e.GetImage ();

			employeeNum += 1;
		}

		// Add next button listener
		nextButton.onClick.AddListener(OnClickNext);

		// Add toggle change listeners - whenever toggle is changed, call ToggleChanged()
		toggle_1 = emp1.transform.Find("Toggle").GetComponent<Toggle> ();
		toggle_1.onValueChanged.AddListener(delegate {
			ToggleChanged(toggle_1);
		});
		toggle_2 = emp2.transform.Find("Toggle").GetComponent<Toggle> ();
		toggle_2.onValueChanged.AddListener(delegate {
			ToggleChanged(toggle_2);
		});
		toggle_3 = emp3.transform.Find("Toggle").GetComponent<Toggle> ();
		toggle_3.onValueChanged.AddListener(delegate {
			ToggleChanged(toggle_3);
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ToggleChanged(Toggle toggle) {
		// Count the number of chosen employees - if it's 3, turn the toggle back off
		int count = 0;
		if (toggle_1.isOn)
			count++;
		if (toggle_2.isOn)
			count++;
		if (toggle_3.isOn)
			count++;

		if (count == 3) {
			toggle.isOn = false;
		}

		if (count == 2) {
			nextButton.interactable = true;
		}
	}

	void OnClickNext() {
		// Wait for Next button to be clicked: Store which employees user chose into employee manager, 
		// render task assignment screen

		// Find out which employees user chose
		if (toggle_1.isOn) {
			employeeManager.GetComponent<EmployeeManager>().AddToMyEmployee ((Employee)(allEmployeesList [0]));
		}
		if (toggle_2.isOn) {
			employeeManager.GetComponent<EmployeeManager>().AddToMyEmployee ((Employee)(allEmployeesList [1]));
		}
		if (toggle_3.isOn) {
			employeeManager.GetComponent<EmployeeManager>().AddToMyEmployee ((Employee)(allEmployeesList [2]));
		}

		// Render next screen - Task Assignment
		GameManager.GetComponent<GameManager> ().HideEmployeeChoose (); // close current panel (employee choose panel)
		GameManager.GetComponent<GameManager>().ShowTaskAssign();

		// Retrieve myEmployees ArrayList from EmployeeManager.cs - assign profile sprite of chosen employees
		// to TaskAssignPanel
		my_employees = employeeManager.GetComponent<EmployeeManager>().myEmployees;
		Image img1 = chosen_emp1_object.GetComponent<Image> ();
		img1.sprite = ((Employee)(my_employees [0])).GetImage ();
		Image img2 = chosen_emp2_object.GetComponent<Image> ();
		img2.sprite = ((Employee)(my_employees [1])).GetImage ();
	}

	public void ResetToggles(){
		toggle_1.isOn = false;
		toggle_2.isOn = false;
		toggle_3.isOn = false;

		employeeManager.GetComponent<EmployeeManager>().myEmployees.Clear ();
	}
}
