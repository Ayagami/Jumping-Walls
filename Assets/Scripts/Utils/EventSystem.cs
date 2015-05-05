using UnityEngine;
using System.Collections;

public class EventsSystem {
	
	public delegate void GameStateChanged(GameManager.GameState state);
	public static event GameStateChanged onGameChanged = null;
	
	public static void sendGameStateChanged(GameManager.GameState state){
		if(onGameChanged != null)
			onGameChanged (state);
	}
}