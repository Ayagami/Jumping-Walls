using UnityEngine;
using System.Collections;

public class SpawnableItem : MonoBehaviour {
	public float timeThatCanBeAlive = 10f;
	protected float timeAlive = 0f;
	public float damage = 25f;


	protected Player player = null; // Referencia al player.
	public virtual void OnEnable(){
		timeAlive = 0;
	}

	public virtual void Update(){
		timeAlive += Time.deltaTime;
		if (timeAlive >= timeThatCanBeAlive)
			ObjectPool.instance.PoolObject (this.gameObject);
	}

	public virtual void OnTriggerEnter2D(Collider2D collider){
		OnTrigger (collider);
	}

	public virtual void OnTrigger(Collider2D collider){
		if (player == null)
			player = GameManager.instance.Player;
		if (player.isVulnerable() && collider.gameObject == player.gameObject) {;
			player.doDamage(damage);
			ObjectPool.instance.PoolObject (this.gameObject);
		}
	}

}
