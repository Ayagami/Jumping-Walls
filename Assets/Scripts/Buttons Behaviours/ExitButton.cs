using UnityEngine;
using System.Collections;

public class ExitButton : ButtonAction {

	public override void execute(){
		Application.Quit();
	}
}
