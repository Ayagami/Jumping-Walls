using UnityEngine;
using System.Collections;

public class EventsSystem {
	
	public delegate void GameStateChanged(GameManager.GameState state);
	public static event GameStateChanged onGameChanged = null;
	
	public delegate void NewInputSystem(InputManager.MainControls button);
	public static event NewInputSystem onNewInputSystem = null;
	
	public static void sendGameStateChanged(GameManager.GameState state){
		if(onGameChanged != null)
			onGameChanged (state);
	}
	
	
	public static void sendNewInputsystem(InputManager.MainControls button){
		if(onNewInputSystem != null)
			onNewInputSystem(button);
	}
	
	
}