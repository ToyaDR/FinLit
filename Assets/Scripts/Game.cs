using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game {

	public static Game current;
	public Store my_store;
	//public Bank my_bank;

	public Game() {
		my_store = new Store ();
		//my_bank = new Bank ();

	}
}
