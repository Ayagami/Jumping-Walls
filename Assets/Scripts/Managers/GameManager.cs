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
	
	private Transform currentRoomTransform = null;
	private Bounds currentRoomBounds;
	
	private Vector3 viewportMaxVectorForRoom = Vector3.zero;
	public float currentY = -1;    

	private int Score = 0;
	// Use this for initialization
	void Start () {
		State = GameState.INTRO;

		if (!instance)
			instance = this;
			
		//#if DEBUG

		if(DataManager.instance == null){
			Debug.Log ("There is not DataManager instantiated");
			Application.LoadLevel("menu");
		}else{
			// Load data from DataManager.
		}

		//#endif

		roomsLeftToLvlUp = roomsPerLevel;

		EventsSystem.onGameChanged      += OnGameChanged;
		EventsSystem.onNewInputSystem   += InputManagement;
        EventsSystem.onNewSaveEvent     += onSaveEvent;

		AddRoom ();

        

		player = new Player (Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        DestroyBlock component = player.gameObject.GetComponentInChildren<DestroyBlock>();

        component.isEnabled = true;

        player.addSkill(component.getName(), component);

		Camera.main.GetComponent<CameraBehaviour> ().player = player.transformation;

    	#if UNITY_ANDROID
        	callPlugin();
    	#endif
	
	}

    void onSaveEvent(){
        DataManager.setHighScore(Score);
    }
    void callPlugin(){
    #if UNITY_ANDROID
        if (Application.isMobilePlatform) {
            using (AndroidJavaClass ajo = new AndroidJavaClass("com.ligool.plugin.Main")) {
                ajo.CallStatic("Test");
            }
            using (AndroidJavaClass ajo = new AndroidJavaClass("com.ligool.plugin.Main")) {
                int p = ajo.CallStatic<int>("getInt");
                Debug.Log("Callback from getInt : " + p);
            }
            /*
            using (AndroidJavaClass pInstance = new AndroidJavaClass("com.ligool.plugin.MainActivity"))
            {
                //AndroidJavaObject pluginInstance = pInstance.CallStatic<AndroidJavaObject>("instance");
                pInstance.CallStatic("shareText", "Jumping Wall", "Please Share this App to Continue.");
            }
            */
        }
    #endif
    }
    void HelloFromAndroid(string log){
        Debug.Log(log);
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
		
		if(currentRoomTransform){	
			Vector3 wPoint = currentRoomBounds.max;
			viewportMaxVectorForRoom = Camera.main.WorldToViewportPoint(wPoint);
			if(viewportMaxVectorForRoom.y < 1f){
				checkLvlUp();
				if(pendingLevel){
					AddRoom();
					pendingLevel = false;
				}
			}
			
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

	/*public void checkPlayerPosition(Vector3 playerPos){
		if (currentRoom != null) {	// Asumo que hay una room activa.
			Bounds r = currentRoom.GetComponentInChildren<Renderer>().bounds;
			float roomY = currentRoom.transform.position.y;
			if(playerPos.y >= roomY){
				if(pendingLevel){
					pendingLevel = false;
					//AddRoom();
				}
				r = currentRoom.GetComponentInChildren<Renderer>().bounds;
				roomY = r.center.y;
				if(playerPos.y >= roomY){
					checkLvlUp();
				}
			}
		}
	}*/

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
		currentRoomTransform = currentRoom.transform;
		currentRoomBounds = currentRoom.GetComponentInChildren<Renderer>().bounds;
		currentY = currentRoom.GetComponentInChildren<Renderer> ().bounds.max.y;	
		roomsLeftToLvlUp--;
	}

	void checkLvlUp(){
		if (roomsLeftToLvlUp <= 0) {
			roomsLeftToLvlUp = roomsPerLevel;
			LevelNumber++;
			AddScore(10000);

		}

		if(prevRoom != null){
			ObjectPool.instance.PoolObject(prevRoom);
			prevRoom = null;
			prevTransform = null;
		}

		pendingLevel = true;
	}

	void OnGameChanged(GameState state){
		State = state;
        if (State == GameState.DONE)
        {
            EventsSystem.sendSaveEvent();
            Application.LoadLevel("menu");
        }
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