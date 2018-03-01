﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Employee {

    private string emp_name;
    private int emp_morale;
    private Sprite emp_image;

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
