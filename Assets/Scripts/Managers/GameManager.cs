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

	[Header("Referencias externas")]
	public AudioSource m_dasAudio = null;
	// Use this for initialization
	void Start () {
		State = GameState.INTRO;
		Time.timeScale = 1;

		if (!instance)
			instance = this;

		if(DataManager.instance == null){
			Debug.Log ("There is not DataManager instantiated");
			Application.LoadLevel("menu");
		}else{
			// Load data from DataManager.
		}

		roomsLeftToLvlUp = roomsPerLevel;

		EventsSystem.onGameChanged      += OnGameChanged;
		EventsSystem.onNewInputSystem   += InputManagement;
        EventsSystem.onNewSaveEvent     += onSaveEvent;

		AddRoom ();

		doPlayerInitializations();
		if(!DataManager.ExistsDataOnDictionary(BuyableManager.BUYABLE_NOADS))
			DataManager.enableAds(true);

    	#if UNITY_ANDROID
        	callPlugin();
    	#endif
	
	}
	
	void doPlayerInitializations(){
		player = new Player (Instantiate (playerPrefab, Vector3.zero, Quaternion.identity) as GameObject);
        DestroyBlock component = player.gameObject.GetComponentInChildren<DestroyBlock>();

		component.OnStart();

        component.isEnabled = DataManager.ExistsDataOnDictionary(component.actionTagOnData);
		
        player.addSkill(component.getName(), component);

		Camera.main.GetComponent<CameraBehaviour> ().player = player.transformation;
	}

    void onSaveEvent(){
		DataManager.addCoins(Score);
        DataManager.setHighScore(Score);
    }
	
	void OnApplicationPause(bool isPaused){
		
		if(isPaused && State == GameState.STARTED){
			GraphicsManager.instance.setPause(true);
		}
		
	}
	
	void OnDestroy(){
		DataManager.enableAds(false);
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

		if (State != GameState.DONE) {

			if (player.transformation.position.y <= prevY) {
				EventsSystem.sendGameStateChanged (GameState.DONE);
			}
		
			if (currentRoomTransform) {	
				Vector3 wPoint = currentRoomBounds.max;
				viewportMaxVectorForRoom = Camera.main.WorldToViewportPoint (wPoint);
				if (viewportMaxVectorForRoom.y < 1f) {
					checkLvlUp ();
					if (pendingLevel) {
						AddRoom ();
						pendingLevel = false;
					}
				}
			
			}

			if (player != null) {
				player.OnUpdate ();	// Llamo al update del player!
				if (player.applyBlink) {	// Checkeo si tengo que hacer animaciones graficas para blinkeo.
					player.applyBlink = false;
					StartCoroutine (player.Blink ());
				}
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				EventsSystem.sendGameStateChanged (GameState.PAUSED);
			}

		}
	}

	void AddRoom(){
		GameObject go = ObjectPool.instance.GetObjectForType ("Room");

		if (currentRoom == null) {
			spawningPosition.x = go.transform.position.x;
			go.transform.position = spawningPosition;
			prevY = go.GetComponentInChildren<Renderer>().bounds.min.y;
		}
		else {
			spawningPosition.x = go.transform.position.x;
			spawningPosition.y = currentRoom.GetComponentInChildren<Renderer> ().bounds.max.y - 0.05f;
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
        if (State == GameState.DONE) {
			int currentHighScore = DataManager.getHighScore();
            EventsSystem.sendSaveEvent();
         	DataManager.enableAds(false);
			GraphicsManager.instance.showFinalResult(Score, currentHighScore);
			Time.timeScale = 0;
		    //Application.LoadLevel("menu");
        }
		if (State == GameState.PAUSED)
			OnGamePaused ();
	}
	void OnGamePaused(){
		GraphicsManager.instance.setPause(true);
		EventsSystem.sendSaveEvent();
	}
	
	
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