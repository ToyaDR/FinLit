using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour {

    public bool loan_allowed;
    public int loan;
    public int interest_rate;

    void Awake() {
        loan_allowed = true;
        loan = 0;
        interest_rate = 3;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
