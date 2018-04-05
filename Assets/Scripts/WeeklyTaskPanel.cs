using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeeklyTaskPanel : MonoBehaviour {
	public GameObject IncomeBar;
	public GameObject SpendingBar;
	public GameObject EmployeeManager;
	public GameObject WeekEmployees;
	public GameObject Store;

	private Dictionary<Employee, int> week_employees;
	private ArrayList week_employees_list;
	/* There won't be more than 8 employees working per week */
	private GameObject emp_featured;

	private GameObject emp1;
	private GameObject emp2;
	private GameObject emp3;
	private GameObject emp4;
	private GameObject emp5;
	private GameObject emp6;
	private GameObject emp7;
	private GameObject emp8;

	private int curr_emp_feat = 0;

	// Use this for initialization
	void Start () {
		week_employees = EmployeeManager.GetComponent<EmployeeManager>().weekEmployees;

		week_employees_list = new ArrayList ();

		emp1 = WeekEmployees.transform.GetChild (0).gameObject;
		emp2 = WeekEmployees.transform.GetChild (1).gameObject;
		emp3 = WeekEmployees.transform.GetChild (2).gameObject;
		emp4 = WeekEmployees.transform.GetChild (3).gameObject;
		emp5 = WeekEmployees.transform.GetChild (4).gameObject;
		emp6 = WeekEmployees.transform.GetChild (5).gameObject;
		emp7 = WeekEmployees.transform.GetChild (6).gameObject;
		emp8 = WeekEmployees.transform.GetChild (7).gameObject;
		emp_featured = WeekEmployees.transform.GetChild (8).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void IncreaseBar(GameObject bar, float inc_width){
		float orig_height = bar.GetComponent<RectTransform> ().sizeDelta.y;
		float orig_width = bar.GetComponent<RectTransform> ().sizeDelta.x;
		bar.GetComponent<RectTransform> ().sizeDelta = new Vector2(orig_width+inc_width, orig_height);
	}

	public void DecreaseBar(GameObject bar, float inc_width){
		float orig_height = bar.GetComponent<RectTransform> ().sizeDelta.y;
		float orig_width = bar.GetComponent<RectTransform> ().sizeDelta.x;
		bar.GetComponent<RectTransform> ().sizeDelta = new Vector2(orig_width-inc_width, orig_height);
	}

	public IEnumerator ShowFeatured(){
		emp_featured.GetComponent<CanvasGroup> ().alpha = 1f;

		while (curr_emp_feat < week_employees_list.Count) {
			/* Set graphic of featured employee to employee indicated by curr_emp_feat */
			Employee curr_emp = (Employee) week_employees_list [curr_emp_feat];

			emp_featured.GetComponent<Image>().sprite = curr_emp.GetImage();
			emp_featured.transform.Find ("Salary").GetChild(0).GetComponent<Text>().text = curr_emp.GetSalary().ToString();
			emp_featured.transform.Find ("Shifts Worked").GetChild (0).GetComponent<Text> ().text = week_employees [curr_emp].ToString();
			emp_featured.transform.Find ("Sold").GetChild(0).GetComponent<Text>().text = curr_emp.GetSold().ToString();

			Debug.Log (curr_emp.GetName());
			/* Add employee salary to spending */
			int curr_add = 0; /* total added so far */

			yield return AddSalarytoSpending(curr_add, curr_emp.GetSalary()*week_employees[curr_emp]);

			curr_add = 0;
			yield return AddSoldtoIncome (curr_add, curr_emp.GetSold());
			/* Increment curr_emp_feat */
			curr_emp_feat++;
		}
		curr_emp_feat = 0;
		emp_featured.GetComponent<CanvasGroup> ().alpha = 0f;

		yield return null;
		ShowEmployeesList ();
	}		

	public IEnumerator AddSoldtoIncome(int curr_add, int sold){
		while (curr_add < sold) {
			IncreaseBar (IncomeBar, curr_add*Store.GetComponent<Store>().GetPrice());
			curr_add++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;
	}

	public IEnumerator AddSalarytoSpending(int curr_add, int salary){
		while (curr_add < salary) {
			IncreaseBar (SpendingBar, curr_add);
			curr_add++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;
	}

	public IEnumerator ShowEmployees(){
		foreach (KeyValuePair<Employee, int> e in week_employees) {
			week_employees_list.Add (e.Key);
		}
		yield return StartCoroutine ("ShowFeatured");


		foreach (KeyValuePair<Employee, int> e in week_employees) {
			e.Key.ResetSold ();
		}

		week_employees.Clear ();
	}

	public void ShowEmployeesList(){
		int i = 0;
		foreach (Employee e in week_employees_list) {
			switch (i) {
			case 0:
				emp1.GetComponent<Image> ().sprite = e.GetImage ();
				emp1.GetComponent<Image> ().color = Color.white;
				break;
				case 1:
				emp2.GetComponent<Image> ().sprite = e.GetImage ();
				emp2.GetComponent<Image> ().color = Color.white;
				break;
				case 2:
				emp3.GetComponent<Image> ().sprite = e.GetImage ();
				emp3.GetComponent<Image> ().color = Color.white;
				break;
				case 3:
				emp4.GetComponent<Image> ().sprite = e.GetImage ();
				emp4.GetComponent<Image> ().color = Color.white;
				break;
				case 4:
				emp5.GetComponent<Image> ().sprite = e.GetImage ();
				emp5.GetComponent<Image> ().color = Color.white;
				break;
				case 5:
				emp6.GetComponent<Image> ().sprite = e.GetImage ();
				emp6.GetComponent<Image> ().color = Color.white;
				break;
				case 6:
				emp7.GetComponent<Image> ().sprite = e.GetImage ();
				emp7.GetComponent<Image> ().color = Color.white;
				break;	
				case 7:
				emp8.GetComponent<Image> ().sprite = e.GetImage ();
				emp8.GetComponent<Image> ().color = Color.white;
				break;
			}
			i++;
		}
	}
}
