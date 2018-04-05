using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Employee {

	// Set in the beginning of the game
    private string emp_name;
    private int emp_morale;
    private Sprite emp_image;
	private Sprite emp_dialogue_image;

	// Not yet dealt with
	public int productivity;
	public int days_since_interaction;
	public string[] issues;
	private int emp_salary;
	public ArrayList tasksNotCompleted; // Tasks that are assigned but not completed. Immediately delete tasks that are completed.
	private int DaysBetweenInteraction;
	private int sold = 0;

	//Dialogue Tree
	public DTreeNode start_question;
	public DTreeNode curr_question;

	public Employee(string name, int morale, string imageName, string dialogueImage, int days, int salary){
		emp_name = name;
		emp_morale = morale;
		emp_image = Resources.Load<Sprite> (imageName);
		emp_dialogue_image = Resources.Load<Sprite>(dialogueImage);
		DaysBetweenInteraction = days;
		tasksNotCompleted = new ArrayList ();
		productivity = morale;
		emp_salary = salary;
	}

	public void SetSalary(int salary){
		emp_salary = salary;
	}

	public int GetSalary(){
		return emp_salary;
	}

	public void SetName(string name) {
        emp_name = name;
    }

    public void SetMorale(int morale) {
        emp_morale = morale;
		productivity = morale;
    }

	public void ResetSold(){
		sold = 0;
	}

	public void AddSold(int i) {
		sold += i;
	}

	public int GetSold(){
		return sold;
	}

	public void SetImage(string imageName) {
		emp_image = Resources.Load<Sprite>(imageName);
    }

	public void SetDialogueImage(string imageName) {
		emp_dialogue_image = Resources.Load<Sprite>(imageName);
	}

	public void SetDaysBetweenInteraction(int days){
		DaysBetweenInteraction = days;
	}

	public int GetMinDaysBetweenInteraction(){
		return DaysBetweenInteraction;
	}

    public string GetName() {
        return emp_name;
    }

    public int GetMorale() {
        return emp_morale;
    }

	public Sprite GetImage() {
		return emp_image;
	}

	public Sprite GetDialogueImage() {
		return emp_dialogue_image;
	}

	public bool SetCurrQuestion(string option){
		if (curr_question.IsLeaf ()) {
			return false;
		}

		if (option == "bad") {
			curr_question = curr_question.GetBadOptionNode ();
		} else if (option == "good") {
			curr_question = curr_question.GetGoodOptionNode ();
		}
		return true;
	}

	public void PrintDTree(DTreeNode curr){
		Debug.Log ("QUESTION:" + curr.GetQuestion());
		if (curr.IsLeaf ()) {
			Debug.Log ("DONE");
			return;
		}
		PrintDTree (curr.GetGoodOptionNode ());
		PrintDTree (curr.GetBadOptionNode ());
	}
}
