using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {
	public static DataManager instance = null;
	public string test = "lala";
	// Use this for initialization
	void Start () {
		if (instance == null)
			instance = this;
	}

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	public void GetData(){
		Debug.Log ("Testing " + test);
	}
}
