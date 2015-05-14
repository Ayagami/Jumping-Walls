using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour {
	public static GraphicsManager instance;

	public Slider _HPComponent = null;
	public Text   _tScoreComponent = null;
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

}
