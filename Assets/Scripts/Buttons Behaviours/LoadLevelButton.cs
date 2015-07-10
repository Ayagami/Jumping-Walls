using UnityEngine;
using System.Collections;

public class LoadLevelButton : ButtonAction {
	public string levelName = "";
	public override void execute(){
		Application.LoadLevel(levelName);
	}
}
