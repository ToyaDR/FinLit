using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeeklyTaskPanel : MonoBehaviour {
	public GameObject IncomeBar;
	public GameObject SpendingBar;
	public GameObject EmployeeManager;

	private GameObject WeekEmployees;

	public bool inc;


	private Dictionary<Employee, int> week_employees;
	/* There won't be more than 8 employees working per week */
	private GameObject emp1;
	private GameObject emp2;
	private GameObject emp3;
	private GameObject emp4;
	private GameObject emp5;
	private GameObject emp6;
	private GameObject emp7;
	private GameObject emp8;

	// Use this for initialization
	void Start () {
		inc = false;
		week_employees = EmployeeManager.GetComponent<EmployeeManager>().weekEmployees;
		emp1 = WeekEmployees.transform.GetChild (0).gameObject;
		emp2 = WeekEmployees.transform.GetChild (1).gameObject;
		emp3 = WeekEmployees.transform.GetChild (2).gameObject;
		emp4 = WeekEmployees.transform.GetChild (3).gameObject;
		emp5 = WeekEmployees.transform.GetChild (4).gameObject;
		emp6 = WeekEmployees.transform.GetChild (5).gameObject;
		emp7 = WeekEmployees.transform.GetChild (6).gameObject;
		emp8 = WeekEmployees.transform.GetChild (7).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void IncreaseBar(GameObject bar, float inc_width){
		float orig_height = bar.GetComponent<RectTransform> ().sizeDelta.y;
		float orig_width = bar.GetComponent<RectTransform> ().sizeDelta.x;
		bar.GetComponent<RectTransform> ().sizeDelta = new Vector2(orig_width+inc_width, orig_height);
		inc = false;
	}

	public void DecreaseBar(GameObject bar, float inc_width){
		float orig_height = bar.GetComponent<RectTransform> ().sizeDelta.y;
		float orig_width = bar.GetComponent<RectTransform> ().sizeDelta.x;
		bar.GetComponent<RectTransform> ().sizeDelta = new Vector2(orig_width-inc_width, orig_height);
		inc = false;
	}

	public void ShowEmployees(){
		int i = 0;
		foreach (KeyValuePair<Employee,int> e in week_employees) {
			switch (i) {
				case 0:
					emp1.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;
				case 1:
					emp2.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;
				case 2:
					emp3.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;
				case 3:
					emp4.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;
				case 4:
					emp5.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;
				case 5:
					emp6.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;
				case 6:
					emp7.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
					break;	
				case 7:
					emp8.GetComponent<SpriteRenderer> ().sprite = e.Key.GetImage ();
				break;
			}
			i++;
		}
	}
}
