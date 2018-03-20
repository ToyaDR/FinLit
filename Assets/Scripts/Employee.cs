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
	public string[] issues;
	public int salary;
	public ArrayList tasksNotCompleted; // Tasks that are assigned but not completed. Immediately delete tasks that are completed.

	//Dialogue Tree
	public DTreeNode start_question;
	public DTreeNode curr_question;

	public void SetName(string name) {
        emp_name = name;
    }

    public void SetMorale(int morale) {
        emp_morale = morale;
    }

	public void SetImage(string imageName) {
		emp_image = Resources.Load<Sprite>(imageName);
    }

	public void SetDialogueImage(string imageName) {
		emp_dialogue_image = Resources.Load<Sprite>(imageName);
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

	public void SetCurrQuestion(string option){
		if (curr_question.IsLeaf ()) {
			return;
		}

		if (option == "bad") {
			curr_question = curr_question.GetBadOptionNode ();
		} else if (option == "good") {
			curr_question = curr_question.GetGoodOptionNode ();
		}
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
