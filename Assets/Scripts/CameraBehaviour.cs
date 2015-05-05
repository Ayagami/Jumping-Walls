using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
	[HideInInspector]
	public Transform player = null;

	private Transform my = null;

	// Use this for initialization
	void Start () {
		/*if (!player) {
			Debug.LogError(" <camera> no tiene player ");
			this.enabled = false;
		}*/
		my = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (player) {
			Vector3 pos = my.position;
			pos.y = player.position.y + 3f;
			my.position = pos;
		}
	}
}
