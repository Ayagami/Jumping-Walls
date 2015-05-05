using UnityEngine;
using System.Collections;


public class Player{
	private GameObject go;
	private Transform tr;
	
	public Player(GameObject _go){
		this.gameObject = _go;
		this.transform = this.gameObject.transform;
	}
	
	public GameObject gameObject{
		get {
			return go;
		}
		set {
			go = value;
		}
	}
	
	public Transform transform{
		get {
			return tr;
		}
		set{
			tr = value;
		}
	}
	
}