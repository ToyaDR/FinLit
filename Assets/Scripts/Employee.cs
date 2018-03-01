using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Employee {

	// Set in the beginning of the game
    private string emp_name;
    private int emp_morale;
    private Sprite emp_image;

	// Not yet dealt with
	public int productivity;
	public string[] issues;
	public int salary;
	public Task[] tasksNotCompleted; // Tasks that are assigned but not completed. Immediately delete tasks that are completed.

	public void SetName(string name) {
        emp_name = name;
    }

    public void SetMorale(int morale) {
        emp_morale = morale;
    }

	public void SetImage(string imageName) {
		emp_image = Resources.Load<Sprite>(imageName);
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
}
