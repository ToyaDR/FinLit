using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Employee {

    private string emp_name;
    private int emp_morale;
    private Sprite emp_image;

	public void setName(string name) {
        emp_name = name;
    }

    public void setMorale(int morale) {
        emp_morale = morale;
    }

	public void setImage(string imageName) {
		emp_image = Resources.Load<Sprite>(imageName);
    }

    public string getName() {
        return emp_name;
    }

    public int getMorale() {
        return emp_morale;
    }

	public Sprite getImage() {
		return emp_image;
	}
}
