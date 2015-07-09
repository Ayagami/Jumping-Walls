using UnityEngine;
using System.Collections;

public class EventsSystem {
	
	public delegate void GameStateChanged(GameManager.GameState state);
	public static event GameStateChanged onGameChanged = null;
	
	public delegate void NewInputSystem(InputManager.MainControls button);
	public static event NewInputSystem onNewInputSystem = null;

    public delegate void SaveEventTrigger();
    public static event SaveEventTrigger onNewSaveEvent = null;
	
	public delegate void MakeNewPurchase(int coinsLeft);
	public static event MakeNewPurchase onNewPurchase = null;
	
	public static void sendGameStateChanged(GameManager.GameState state){
		if(onGameChanged != null)
			onGameChanged (state);
	}
	
	
	public static void sendNewInputsystem(InputManager.MainControls button){
		if(onNewInputSystem != null)
			onNewInputSystem(button);
	}

    public static void sendSaveEvent()
    {
        if (onNewSaveEvent != null)
            onNewSaveEvent();
    }
	
	public static void makeNewPurchaseTrigger(int coinsLeft){
		if(onNewPurchase != null)
			onNewPurchase(coinsLeft);
	}
}