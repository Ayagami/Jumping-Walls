using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public enum GameState{
		INTRO,
		STARTED,
		PAUSED,
		DONE
	};

	public static GameState State = GameState.INTRO; 	// Setting GameState to INTRO!
	public static GameManager instance = null;

	[Header("Rooms")]
	public int roomsPerLevel = 1;
	public int LevelNumber = 0;
	private int roomsLeftToLvlUp = 0;
	bool pendingLevel = true;

	[Header("Prefabs")]
	public GameObject RoomPrefab = null;
	public GameObject playerPrefab = null;

	private static Player player = null;

	public Vector3 spawningPosition = Vector3.zero;
	
	[Header("Punteros a Objetos")]
    public GameObject prevRoom = null;
	public Transform prevTransform = null;
	private float prevY = -1;

	private GameObject currentRoom = null;
	public float currentY = -1;


	// Use this for initialization
	void Start () {
		if (!instance)
			instance = this;

		roomsLeftToLvlUp = roomsPerLevel;

		EventsSystem.onGameChanged += OnGameChanged;

		AddRoom ();

		player = new Player (Instantiate (playerPrefab, spawningPosition, Quaternion.identity) as GameObject);

		Camera.main.GetComponent<CameraBehaviour> ().player = player.transformation;

	}

	void Update(){

		/*
		if (prevRoom) {	// we can lose...
			if(player.transform.position.y <= prevY){
				State = GameState.DONE;
				EventsSystem.sendGameStateChanged(State);
			}
		}*/

		if (player.transformation.position.y <= prevY) {
			EventsSystem.sendGameStateChanged(GameState.DONE);
		}

		if (player != null) {
			player.OnUpdate ();	// Llamo al update del player!
			if(player.applyBlink){	// Checkeo si tengo que hacer animaciones graficas para blinkeo.
				player.applyBlink = false;
				StartCoroutine(player.Blink());
			}
		}
	}

	public void checkPlayerPosition(Vector3 playerPos){
		if (currentRoom != null) {	// Asumo que hay una room activa.
			Bounds r = currentRoom.GetComponentInChildren<Renderer>().bounds;
			float roomY = r.center.y;
			if(playerPos.y >= roomY){
				if(pendingLevel){
					pendingLevel = false;
					AddRoom();
				}
				r = currentRoom.GetComponentInChildren<Renderer>().bounds;
				roomY = r.min.y;
				if(playerPos.y >= roomY){
					checkLvlUp();
				}
			}
		}
	}

	void AddRoom(){
		GameObject go = ObjectPool.instance.GetObjectForType ("Room");

		if (currentRoom == null) {
			go.transform.position = spawningPosition;
			prevY = go.GetComponentInChildren<Renderer>().bounds.min.y;
		}
		else {
			spawningPosition.y = go.GetComponentInChildren<Renderer> ().bounds.size.y / 2 + currentRoom.GetComponentInChildren<Renderer> ().bounds.max.y;
			go.transform.position = spawningPosition;
			prevRoom = currentRoom;
			prevTransform = prevRoom.transform;
			prevY = prevTransform.GetComponentInChildren<Renderer>().bounds.min.y;
		}
		currentRoom = go;
		currentY = currentRoom.GetComponentInChildren<Renderer> ().bounds.max.y;
		roomsLeftToLvlUp--;
	}

	void checkLvlUp(){
		if (roomsLeftToLvlUp <= 0) {
			roomsLeftToLvlUp = roomsPerLevel;
			LevelNumber++;
			if(prevRoom != null){
				ObjectPool.instance.PoolObject(prevRoom);
				prevRoom = null;
				prevTransform = null;
			}
		}

		pendingLevel = true;
	}

	void OnGameChanged(GameState state){
		State = state;
		if(State == GameState.DONE)
			Time.timeScale = 0;
	}

	public Player Player{
		get {
			return player;
		}
	}
}