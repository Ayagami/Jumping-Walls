using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
	[HideInInspector]
	public Transform player = null;
	private Transform my = null;

	public Color ColorDay = Color.cyan;
	public Color ColorLate = Color.gray;

	public float speedLerp = 1f;

	// Use this for initialization
	void Start () {
		/*if (!player) {
			Debug.LogError(" <camera> no tiene player ");
			this.enabled = false;
		}*/
		my = transform;
	}
	void Update(){
		Camera.main.backgroundColor = Color.Lerp(ColorDay, ColorLate, Mathf.PingPong(Time.time * speedLerp, 1.0f) );
	}
	// Update is called once per frame
	void LateUpdate () {
		if (player) {
			Vector3 pos = my.position;
			pos.y = player.position.y + 3f;
			my.position = Vector3.Lerp (my.position, pos, Time.deltaTime);
		}
	}
}
