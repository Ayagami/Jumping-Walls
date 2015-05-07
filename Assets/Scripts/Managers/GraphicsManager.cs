using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour {
	public static GraphicsManager instance;

	public Slider _HPComponent = null;

	// Use this for initialization
	void Start () {
		instance = this;
		SetHP (1);
	}

	public void SetHP(float newHP){
		_HPComponent.value = newHP;
	}

}
