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
	public GameObject store;

	private Toggle toggle_1;
	private Toggle toggle_2;
	private Toggle toggle_3;

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
		if (toggle_1.isOn && toggle_2.isOn && toggle_3.isOn) {
			toggle.isOn = false;
		}
	}

	void OnClickNext() {
		// Wait for Next button to be clicked: Store which employees user chose, render task assignment screen

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

		// Render next screen
		taskAssignPanel.GetComponent<CanvasGroup>().alpha = 1f;
		taskAssignPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
		store.GetComponent<Store>().HideEmployeeChoose ();
	}
}
