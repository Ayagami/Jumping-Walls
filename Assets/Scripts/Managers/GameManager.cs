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

	private int Score = 0;
	// Use this for initialization
	void Start () {
		State = GameState.INTRO;

		if (!instance)
			instance = this;

		roomsLeftToLvlUp = roomsPerLevel;

		EventsSystem.onGameChanged += OnGameChanged;
		EventsSystem.onNewInputSystem += InputManagement;
		

		AddRoom ();

		player = new Player (Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        DestroyBlock component = player.gameObject.GetComponentInChildren<DestroyBlock>();

        player.addSkill(component.getName(), component);

		Camera.main.GetComponent<CameraBehaviour> ().player = player.transformation;

	//#if DEBUG

		if(DataManager.instance == null){
			Debug.Log ("There is not DataManager instantiated");
			Application.LoadLevel("menu");
		}else{
			// Load data from DataManager.
		}

	//#endif

	}
	
	void InputManagement(InputManager.MainControls control){
		switch(control){
			case InputManager.MainControls.DESTROY_BLOCK_ACTION:
				player.getSkill("DestroyBlock").execute();
				break;
		}
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

		if (Input.GetKeyDown (KeyCode.Escape)) {
			EventsSystem.sendGameStateChanged(GameState.PAUSED);
		}
		/*
        if (InputManager.isTriggeringDown(InputManager.DESTROY_BLOCK_ACTION)){
            player.getSkill("DestroyBlock").execute();
        }*/
	}

	public void checkPlayerPosition(Vector3 playerPos){
		if (currentRoom != null) {	// Asumo que hay una room activa.
			Bounds r = currentRoom.GetComponentInChildren<Renderer>().bounds;
			float roomY = currentRoom.transform.position.y;
			if(playerPos.y >= roomY){
				if(pendingLevel){
					pendingLevel = false;
					AddRoom();
				}
				r = currentRoom.GetComponentInChildren<Renderer>().bounds;
				roomY = r.center.y;
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
			spawningPosition.y = currentRoom.GetComponentInChildren<Renderer> ().bounds.max.y;
			go.transform.position = spawningPosition;
			prevRoom = currentRoom;
			prevTransform = prevRoom.transform;
			prevY = prevTransform.GetComponentInChildren<Renderer>().bounds.min.y;
			AddScore(1000);
		}
		currentRoom = go;
		currentY = currentRoom.GetComponentInChildren<Renderer> ().bounds.max.y;
		roomsLeftToLvlUp--;
	}

	void checkLvlUp(){
		if (roomsLeftToLvlUp <= 0) {
			Debug.Log("LLego...");
			roomsLeftToLvlUp = roomsPerLevel;
			LevelNumber++;
			AddScore(10000);

		}

		if(prevRoom != null){
			Debug.Log("Reciclo");
			ObjectPool.instance.PoolObject(prevRoom);
			prevRoom = null;
			prevTransform = null;
		}

		pendingLevel = true;
	}

	void OnGameChanged(GameState state){
		State = state;
		if (State == GameState.DONE)
			Application.LoadLevel ("menu");
		if (State == GameState.PAUSED)
			OnGamePaused ();
	}

	void OnGamePaused(){
		Application.LoadLevel ("menu");	// For Now...
	}

	/*void OnApplicationPause(bool PauseStatus){
		Time.timeScale = PauseStatus == true ? 1 : 0;
	}*/

	public void AddScore(int plus){
		Score += plus;
		GraphicsManager.instance.SetScore (Score);
	}

	public Player Player{
		get {
			return player;
		}
	}
}