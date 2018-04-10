using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeeklyTaskPanel : MonoBehaviour {
	public GameObject IncomeBar;
	public GameObject IncomeTotal;

	public GameObject SalaryBar;
	public GameObject IngredientsBar;
	public GameObject LoanBar;
	public GameObject TaxBar;
	public GameObject SpendingTotal;

	public GameObject EmployeeManager;
	public GameObject WeekEmployees;
	public GameObject Store;
	public GameObject GameManager;
	public GameObject IngredientsPanel;

	public GameObject LoanPanel;
	public GameObject LoanPay;
	public GameObject LoanOwed;
	public GameObject LoanPayButton;

	public GameObject TaxPanel;
	public GameObject TaxPay;
	public GameObject TaxOwed;
	public GameObject TaxPayButton;

	public GameObject DoneButton;
	public GameObject InsuranceButton;

	public GameObject IngFeatured;

	private Button loan_pay_button;
	private Button tax_pay_button;
	private Button insurance_button;
	private Button done_button;

	private Text income_total;
	private Text spending_total;

	private int inc_tot = 0;
	private int spend_tot = 0;

	private bool tax_paid = false;
	private bool loan_paid = false;

	private Dictionary<Employee, int> week_employees;
	private ArrayList week_employees_list;
	/* There won't be more than 8 employees working per week */
	private GameObject emp_featured;

	private Dictionary<Ingredient, int> week_ingredients;
	private ArrayList week_ingredients_list;

	private int curr_emp_feat = 0;
	private bool loan_hidden = true;
	private bool tax_hidden = true;

	private int bar_unit = 2;
	// Use this for initialization
	void Start () {
		loan_pay_button = LoanPayButton.GetComponent<Button> ();
		tax_pay_button = TaxPayButton.GetComponent<Button> ();
		insurance_button = InsuranceButton.GetComponent<Button>();
		done_button = DoneButton.GetComponent<Button>();

		spending_total = SpendingTotal.GetComponent<Text> ();
		income_total = IncomeTotal.GetComponent<Text> ();

		LoanOwed.GetComponent<Text> ().text = Store.GetComponent<Store> ().GetLoan ().ToString();

		week_employees = EmployeeManager.GetComponent<EmployeeManager>().weekEmployees;

		week_employees_list = new ArrayList ();
		foreach (KeyValuePair<Employee, int> e in week_employees) {
			week_employees_list.Add (e.Key);
		}
		emp_featured = WeekEmployees.transform.GetChild (0).gameObject;

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
	}		

	public IEnumerator AddSoldtoIncome(int incr, int sold){
		loan_pay_button.interactable = false;
		tax_pay_button.interactable = false;
		insurance_button.interactable = false;
		done_button.interactable = false;
		while (incr < sold) {
			IncreaseBar (IncomeBar, bar_unit*Store.GetComponent<Store>().GetPrice());
			inc_tot += Store.GetComponent<Store>().GetPrice();
			income_total.text = inc_tot.ToString ();

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

			spend_tot++;
			spending_total.text = spend_tot.ToString ();

			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;
	}

	public IEnumerator ShowIngredientsSpending(){
		IngFeatured.GetComponent<CanvasGroup> ().alpha = 1f;
		int incr = 0;
		int total_spent = 0;
		week_ingredients = IngredientsPanel.GetComponent<IngredientsPanel> ().boughtIngredients;
		week_ingredients_list = IngredientsPanel.GetComponent<IngredientsPanel> ().getIngredientsList ();

		foreach(KeyValuePair<Ingredient, int> i in week_ingredients){
			total_spent += i.Value * i.Key.getPrice();
		}

		Debug.Log (week_ingredients.Count);
		total_spent = 4;
		yield return AddIngredientstoSpending (incr, total_spent);
		IngredientsPanel.GetComponent<IngredientsPanel> ().resetBought ();
		IngFeatured.GetComponent<CanvasGroup> ().alpha = 0f;
	}

	public IEnumerator AddIngredientstoSpending(int incr, int total_spent){
		while (incr < total_spent) {
			IncreaseBar (IngredientsBar, bar_unit);

			float loan_new_x = LoanBar.transform.localPosition.x + bar_unit;
			float loan_old_y = LoanBar.transform.localPosition.y;
			LoanBar.transform.localPosition = new Vector3(loan_new_x, loan_old_y, 1f);

			float tax_new_x = TaxBar.transform.localPosition.x + bar_unit;
			float tax_old_y = TaxBar.transform.localPosition.y;
			TaxBar.transform.localPosition = new Vector3(tax_new_x, tax_old_y, 1f);

			spend_tot++;
			spending_total.text = spend_tot.ToString ();

			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		loan_pay_button.interactable = true;
		tax_pay_button.interactable = true;
		insurance_button.interactable = true;
		done_button.interactable = true;
		yield return null;
	}

	public IEnumerator ShowEmployees(){
		foreach (KeyValuePair<Employee, int> e in week_employees) {
			week_employees_list.Add (e.Key);
		}	
		yield return StartCoroutine ("ShowFeaturedEmployee");

		week_employees.Clear ();
	}

	public void HideShowLoanPanel(){
		if (loan_hidden) {
			LoanPanel.GetComponent<CanvasGroup> ().alpha = 1f;
			LoanPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			LoanPanel.GetComponent<CanvasGroup> ().alpha = 0f;
			LoanPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}

		loan_hidden = !loan_hidden;
	}

	public void PayLoanWrapper(){
		StartCoroutine (PayLoan ());
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
			int orig;
			int.TryParse((LoanOwed.GetComponent<Text> ().text).ToString(), out orig);
			LoanOwed.GetComponent<Text> ().text = (orig-1).ToString ();
			spend_tot++;
			spending_total.text = spend_tot.ToString ();

			incr++;
			yield return new WaitForSeconds (0.2f);
		}
		yield return null;

		loan_hidden = false;
		HideShowLoanPanel ();
		loan_pay_button.interactable = false;
	}

	public void HideShowTaxPanel(){
		if (tax_hidden) {
			TaxPanel.GetComponent<CanvasGroup> ().alpha = 1f;
			TaxPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			TaxPanel.GetComponent<CanvasGroup> ().alpha = 0f;
			TaxPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
		tax_hidden = !tax_hidden;
	}
}
