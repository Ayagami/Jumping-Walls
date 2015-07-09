using UnityEngine;
using System.Collections;

public class ShowObject : ButtonAction {

	public GameObject target = null;

	public override void execute(){
		target.SetActive( true );
	}
}
