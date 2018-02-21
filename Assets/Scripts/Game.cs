using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game {

	public static Game current;
	public Store my_store;
	public int day;
	//public Bank my_bank;
	//TODO: add more variables to hold game state

	public Game() {
		my_store = new Store ();
		//my_bank = new Bank ();
		day = 0;
	}
}
