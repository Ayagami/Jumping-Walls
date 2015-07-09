using UnityEngine;
using System.Collections;

public class ToggleThis : ButtonAction {
	
	private GameObject me = null;
	public bool disableOnStart = true;
	
	public override void OnStart(){
		base.OnStart();
		me = this.gameObject.transform.parent.gameObject;
		if(disableOnStart)
			me.SetActive(false);
	}
	public override void execute(){
		me.SetActive( !me.activeInHierarchy );
	}
}
