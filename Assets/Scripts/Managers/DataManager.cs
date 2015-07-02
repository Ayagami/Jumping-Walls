using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {
	public static DataManager instance = null;
	public string test = "lala";
	
	private Dictionary<string, bool> m_dBuyable;
	// Use this for initialization
	void Start () {
		if (instance == null){
			instance = this;
			m_dBuyable = new Dictionary<string, bool>();
			m_dBuyable["hello"] = true;
			m_dBuyable["world"] = false;
			load();
		}
	}

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	public void GetData(){
		Debug.Log ("Testing " + test);
	}

	public void Clean(){
		//
	}
	
	private void save(){
		foreach(var pair in m_dBuyable){
			ES2.Save(pair.Value, "file.txt?tag=" + pair.Key);
			Debug.Log("Saving key=>" + pair.Key);
		}
	}

	private void load(){
		if(ES2.Exists("file.txt")){
			foreach(var pair in m_dBuyable){
				bool p = ES2.Load<bool>("file.txt?tag" + pair.Key);
				Debug.Log("key=>" + pair.Key + ", value=>" + pair.Value);
				m_dBuyable[pair.Key] = p;
			}
		}
	}
}
