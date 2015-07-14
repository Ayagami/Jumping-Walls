using UnityEngine;
using UnityEngine.UI;

public class PoPUPInstance : MonoBehaviour {

	public static PoPUPInstance instance = null;
	
	public GameObject UI = null;

	public Text text_title = null;
	public Text text_body = null;

	// Use this for initialization
	void Awake(){
		instance = this;
	}
	
	void Start () {
	}
	
	public void showPopUp(string title, string body){
		if(text_title)
			text_title.text = title;
		if(text_body)
			text_body.text  = body;
		if(UI)
			UI.SetActive(true);
	}
}
