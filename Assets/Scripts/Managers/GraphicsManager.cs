using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour {
	public static GraphicsManager instance;

	public Slider _HPComponent = null;
	public Text   _tScoreComponent = null;
	
	public GameObject pauseMenu = null;
	// Use this for initialization
	void Start () {
		instance = this;
		SetHP (1);
		SetScore (0);
	}

	public void SetHP(float newHP){
		_HPComponent.value = newHP;
	}

	public void SetScore(int score){
		if(_tScoreComponent != null)
			_tScoreComponent.text = "Score: " + score;
	}

	public void setPause(bool pause){
		if(pause){
			pauseMenu.SetActive(true);
			Time.timeScale = 0;
			if(GameManager.State == GameManager.GameState.STARTED)
				GameManager.State = GameManager.GameState.PAUSED;
		}else{
			pauseMenu.SetActive(false);
			Time.timeScale = 1;
			if(GameManager.State == GameManager.GameState.PAUSED)
				GameManager.State = GameManager.GameState.STARTED;
		}
	}
}
