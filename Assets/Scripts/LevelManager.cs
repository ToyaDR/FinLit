using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager: MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadMenu() {
		SceneManager.LoadScene (0);	
	}

	public void LoadBizworks(){
		/* Called on start press */
		SceneManager.LoadScene (1);
	}

	public void LoadCredits(){
		/* Called on credits press */
		SceneManager.LoadScene (2);
	}

	public void Exit(){
		Application.Quit ();
	}
}
