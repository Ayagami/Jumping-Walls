using UnityEngine;
using System.Collections;

public class SpawnableItem : MonoBehaviour {
	public float timeThatCanBeAlive = 3f;
	private float timeAlive = 0f;

	void OnEnable(){
		timeAlive = 0;
	}

	void Update(){
		timeAlive += Time.deltaTime;
		if (timeAlive >= timeThatCanBeAlive)
			ObjectPool.instance.PoolObject (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject == GameManager.instance.Player.gameObject) {
			Debug.Log ("Hitted a player");
			ObjectPool.instance.PoolObject (this.gameObject);
		}
	}
}
