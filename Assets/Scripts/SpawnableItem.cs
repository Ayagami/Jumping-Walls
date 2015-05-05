using UnityEngine;
using System.Collections;

public class SpawnableItem : MonoBehaviour {
	public float timeThatCanBeAlive = 3f;
	private float timeAlive = 0f;
	public float damage = 25f;


	private Player player = null; // Referencia al player.
	void OnEnable(){
		timeAlive = 0;
	}

	void Update(){
		timeAlive += Time.deltaTime;
		if (timeAlive >= timeThatCanBeAlive)
			ObjectPool.instance.PoolObject (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (player == null)
			player = GameManager.instance.Player;
		if (player.isVulnerable() && collider.gameObject == player.gameObject) {
			Debug.Log ("Hitted a player");
			player.doDamage(damage);
			ObjectPool.instance.PoolObject (this.gameObject);
		}
	}
}
