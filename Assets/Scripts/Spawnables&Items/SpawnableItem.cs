using UnityEngine;
using System.Collections;

public class SpawnableItem : MonoBehaviour {
	public float timeThatCanBeAlive = 10f;
	protected float timeAlive = 0f;
	public float damage = 25f;
	protected Player player = null; // Referencia al player.
	
	public Rigidbody2D rb2d = null;

	private float rotFactor = 1;
	public bool canRotate = false;
	public virtual void OnEnable(){
		timeAlive = 0;
		if(rb2d){
			rb2d.gravityScale = Mathf.Abs(0.1f + 0.1f * ( 1 / 50-(GameManager.instance.LevelNumber+1)));
		}else{
			rb2d = this.GetComponent<Rigidbody2D>();
		}
		if(canRotate)
			rotFactor *= Mathf.Sin (Time.time);
	}
	
	void Start(){
		//rb2d = this.GetComponent<Rigidbody2D>();
	}

	public virtual void Update(){
		timeAlive += Time.deltaTime;
		if (timeAlive >= timeThatCanBeAlive)
			ObjectPool.instance.PoolObject (this.gameObject);

		if(canRotate)
			transform.Rotate (Vector3.forward * rotFactor);
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
