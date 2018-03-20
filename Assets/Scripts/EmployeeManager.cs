using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour {

    public ArrayList allEmployees;
	public ArrayList myEmployees;

	void Awake() {
        allEmployees = new ArrayList();

        // At the start of the game, create all possible instances of employees
        Employee employee_one = new Employee();
        employee_one.SetName("Lila");
        employee_one.SetMorale(10);
		employee_one.SetImage ("Icon_Lila");
		employee_one.SetDialogueImage ("Employee_Lila");
		employee_one.tasksNotCompleted = new ArrayList ();
		/* Populate Dialogue Tree for Lila */
		employee_one.start_question = new DTreeNode ("Should I confront my roommate?", "Yes!!!", "No...");
		employee_one.curr_question = employee_one.start_question;

		employee_one.start_question.AddGoodOption ("GOOD", "GOOD-Y", "GOOD-N"); //Did confront roommate
		employee_one.start_question.AddBadOption ("BAD", "BAD-Y", "BAD-N"); //Did not confront roommate

		allEmployees.Add (employee_one);


		Employee employee_two = new Employee();
		employee_two.SetName("Mateo");
		employee_two.SetMorale(10);
		employee_two.SetImage ("Icon_Mateo");
		employee_two.SetDialogueImage ("char2");
		employee_two.tasksNotCompleted = new ArrayList ();
		/* Populate Dialogue Tree for Mateo */
		employee_two.start_question = new DTreeNode ("Should I confront my roommate?", "Yes.", "No.");
		employee_two.curr_question = employee_two.start_question;

		allEmployees.Add (employee_two);


		Employee employee_three = new Employee();
		employee_three.SetName("Bob");
		employee_three.SetMorale(10);
		employee_three.SetImage ("char1");
		employee_three.SetDialogueImage ("char1");
		employee_three.tasksNotCompleted = new ArrayList ();
		/* Populate Dialogue Tree for employee_three */
		employee_three.start_question = new DTreeNode ("Should I confront my roommate?", "Yes.", "No.");
		employee_three.curr_question = employee_three.start_question;

		allEmployees.Add (employee_three);

		// Initialize my employees list
        myEmployees = new ArrayList();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddToMyEmployee(Employee employee) {
        myEmployees.Add(employee);
    }

	public void InitializeEmployeeList() {
		myEmployees = new ArrayList ();
	}

	public void SetFlowchart(DTreeNode currq, List<Fungus.Command> qCommands){

		Fungus.Say question = (Fungus.Say) qCommands[0];
		question.SetStandardText (currq.GetQuestion());

		Fungus.Menu goodopt = (Fungus.Menu)qCommands [2];
		goodopt.SetStandardText (currq.GetGoodOption());

		Fungus.Menu badopt = (Fungus.Menu)qCommands [3];
		badopt.SetStandardText (currq.GetBadOption ());
	}
}
