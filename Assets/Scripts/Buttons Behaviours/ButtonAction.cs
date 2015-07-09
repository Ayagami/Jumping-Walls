using UnityEngine;
using UnityEngine.UI;

public class ButtonAction : MonoBehaviour {
	// Generic class.
	public Button bt = null;
	
	void Start(){
		OnStart();
	}
	
	public virtual void OnStart(){
		
		if(bt == null){
			bt = this.GetComponent<Button>();
			if(bt == null){
				Debug.LogError("Not button");
				return;
			}
		}
		
		bt = this.GetComponent<Button>();
		bt.onClick.AddListener( () => { execute(); });
	}
	
	public virtual void execute(){
		
	}
}
