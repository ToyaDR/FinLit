using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad {
	public static Game savedGame = new Game();

	private static string save_path = Path.Combine(Application.persistentDataPath, "/savedGame.gd");

	public static void Save() {
		savedGame = Game.current;
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (save_path);
		bf.Serialize (file, SaveLoad.savedGame);
		file.Close ();
	}

	public static void Load() {
		if (File.Exists (save_path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (save_path, FileMode.Open);
			SaveLoad.savedGame = (Game) bf.Deserialize(file);
			file.Close();
		}
	}
}
