using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeeklyTaskPanel : MonoBehaviour {
	public GameObject IncomeBar;
	public GameObject SalaryBar;
	public GameObject IngredientsBar;
	public GameObject LoanBar;
	public GameObject TaxBar;

	public GameObject EmployeeManager;
	public GameObject WeekEmployees;
	public GameObject Store;
	public GameObject GameManager;
	public GameObject IngredientsPanel;

	public GameObject LoanPanel;
	public GameObject LoanPay;
	public GameObject LoanOwed;

	public GameObject TaxPanel;
	public GameObject TaxPay;
	public GameObject TaxOwed;

	public GameObject IngFeatured;

	private bool tax_paid = false;
	private bool loan_paid = false;

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

	private Dictionary<Ingredient, int> week_ingredients;
	private ArrayList week_ingredients_list;

	private int curr_emp_feat = 0;
	private bool loan_hidden = true;
	private bool tax_hidden = true;

	private int bar_unit = 5;
	// Use this for initialization
	void Start () {
		LoanOwed.GetComponent<Text> ().text = Store.GetComponent<Store> ().GetLoan ().ToString();

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

		week_ingredients = IngredientsPanel.GetComponent<IngredientsPanel> ().bought ();
		week_ingredients_list = IngredientsPanel.GetComponent<IngredientsPanel> ().getIngredientsList ();
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

	public IEnumerator ShowFeaturedEmployee(){
		emp_featured.GetComponent<CanvasGroup> ().alpha = 1f;

		while (curr_emp_feat < week_employees_list.Count) {
			/* Set graphic of featured employee to employee indicated by curr_emp_feat */
			Employee curr_emp = (Employee) week_employees_list [curr_emp_feat];

			emp_featured.GetComponent<Image>().sprite = curr_emp.GetImage();
			emp_featured.transform.Find ("Salary").GetChild(0).GetComponent<Text>().text = curr_emp.GetSalary().ToString();

			/* Add employee salary to spending */
			int incr = 0; /* total added so far */

			yield return AddSalarytoSpending(incr, curr_emp.GetSalary()*week_employees[curr_emp]);
			curr_emp_feat++;
		}
		curr_emp_feat = 0;
		emp_featured.GetComponent<CanvasGroup> ().alpha = 0f;
		yield return ShowIngredientsSpending();
		ShowEmployeesList ();
	}		

	public IEnumerator AddSoldtoIncome(int incr, int sold){
		/* TODO: change this to total sold rather than per employee */
		while (incr < sold) {
			IncreaseBar (IncomeBar, bar_unit*Store.GetComponent<Store>().GetPrice());
			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return ShowEmployees();
		GameManager.GetComponent<GameManager>().ResetSold ();
	}

	public IEnumerator AddSalarytoSpending(int incr, int salary){
		while (incr < salary) {
			IncreaseBar (SalaryBar, bar_unit);

			float ing_new_x = IngredientsBar.transform.localPosition.x + bar_unit;
			float ing_old_y = IngredientsBar.transform.localPosition.y;
			IngredientsBar.transform.localPosition = new Vector3(ing_new_x, ing_old_y, 1f);

			float loan_new_x = LoanBar.transform.localPosition.x + bar_unit;
			float loan_old_y = LoanBar.transform.localPosition.y;
			LoanBar.transform.localPosition = new Vector3(loan_new_x, loan_old_y, 1f);

			float tax_new_x = TaxBar.transform.localPosition.x + bar_unit;
			float tax_old_y = TaxBar.transform.localPosition.y;
			TaxBar.transform.localPosition = new Vector3(tax_new_x, tax_old_y, 1f);

			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;
	}

	public IEnumerator ShowIngredientsSpending(){
		int incr = 0;

		int total_spent = 0;
		foreach(KeyValuePair<Ingredient, int> i in week_ingredients){
			total_spent += i.Value * i.Key.getPrice();
		}
		yield return AddIngredientstoSpending (incr, total_spent);
		IngredientsPanel.GetComponent<IngredientsPanel> ().resetBought ();
		IngFeatured.GetComponent<CanvasGroup> ().alpha = 0f;
	}

	public IEnumerator AddIngredientstoSpending(int incr, int total_spent){
		IngFeatured.GetComponent<CanvasGroup> ().alpha = 1f;
		while (incr < total_spent) {
			IncreaseBar (IngredientsBar, bar_unit);

			float loan_new_x = LoanBar.transform.localPosition.x + bar_unit;
			float loan_old_y = LoanBar.transform.localPosition.y;
			LoanBar.transform.localPosition = new Vector3(loan_new_x, loan_old_y, 1f);

			float tax_new_x = TaxBar.transform.localPosition.x + bar_unit;
			float tax_old_y = TaxBar.transform.localPosition.y;
			TaxBar.transform.localPosition = new Vector3(tax_new_x, tax_old_y, 1f);

			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;
	}

	public IEnumerator ShowEmployees(){
		foreach (KeyValuePair<Employee, int> e in week_employees) {
			week_employees_list.Add (e.Key);
		}	
		yield return StartCoroutine ("ShowFeaturedEmployee");

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

	public void HideShowLoanPanel(){
		if (loan_hidden) {
			LoanPanel.GetComponent<CanvasGroup> ().alpha = 1f;
			LoanPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			LoanPanel.GetComponent<CanvasGroup> ().alpha = 0f;
			LoanPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	public void PayLoanWrapper(){
		StartCoroutine (PayLoan ());
	}

	public void HideShowTaxPanel(){
		if (tax_hidden) {
			TaxPanel.GetComponent<CanvasGroup> ().alpha = 1f;
			TaxPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			TaxPanel.GetComponent<CanvasGroup> ().alpha = 0f;
			TaxPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	private IEnumerator PayLoan(){
		int pay;
		int.TryParse((LoanPay.GetComponent<Text> ().text).ToString(), out pay);

		int loan_total = Store.GetComponent<Store> ().GetLoan ();
		if (pay > loan_total) { //TODO: maybe change this???
			pay = loan_total;
		}
		Store.GetComponent<Store> ().DecLoan (pay);
		Store.GetComponent<Store> ().DecMoney (pay);

		int incr = 0;
		yield return AddLoantoSpending(incr, pay);
	}

	private IEnumerator AddLoantoSpending(int incr, int pay){
		while (incr < pay) {
			IncreaseBar (LoanBar, bar_unit);
			if (!tax_paid) {
				float tax_new_x = TaxBar.transform.localPosition.x + bar_unit;
				float tax_old_y = TaxBar.transform.localPosition.y;
				TaxBar.transform.localPosition = new Vector3(tax_new_x, tax_old_y, 1f);
			}
			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;
	}
}
