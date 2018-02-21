using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {

	private bool new_game;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		/* Load saved Game state if it exists */
		if (SaveLoad.Load () == -1) {
			Game.current = new Game ();
			new_game = true;
			Debug.Log ("new game");
		} else {
			new_game = false;
			Debug.Log ("loaded game");
		}			
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (LoadNewScene());
	}

	IEnumerator LoadNewScene() {
		yield return new WaitForSeconds (10);

		// =====> This part to be removed later
		AsyncOperation async = Application.LoadLevelAsync(0);
		// <=====

		if (new_game) {
			//load customization scene
			//AsyncOperation async = Application.LoadLevelAsync(customization_scene);
		} else {
			//load storefront
			//AsyncOperation async = Application.LoadLevelAsync(storefront_scene);
		}

		while (!async.isDone) {
			yield return null;
		}
	}
}
