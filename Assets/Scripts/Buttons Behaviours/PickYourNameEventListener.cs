using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PickYourNameEventListener : MonoBehaviour {

	public Text data = null;
	private Button bt = null;

	// Use this for initialization
	void Start () {
		bt = this.GetComponent<Button>();
		bt.onClick.AddListener( () => { setNickName(data.text); });
	}
	
	void setNickName(string s){
		DataManager.setNickName(s);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
