using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Employee {

    private string emp_name;
    private int emp_morale;

	public void setName(string name) {
        emp_name = name;
    }

    public void setMorale(int morale) {
        emp_morale = morale;
    }

    public string getName() {
        return emp_name;
    }

    public int getMorale() {
        return emp_morale;
    }
}
