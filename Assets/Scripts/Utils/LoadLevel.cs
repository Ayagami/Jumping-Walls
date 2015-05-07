using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	public GameObject prefab;
	void Awake(){
		if (DataManager.instance == null) {
			GameObject go = Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
			go.name = prefab.name;
		}
	}
	public void Load(string levelName){
		Application.LoadLevel (levelName);
	}
}
