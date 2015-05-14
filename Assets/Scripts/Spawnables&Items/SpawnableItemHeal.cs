using UnityEngine;
using System.Collections;

public class SpawnableItemHeal : SpawnableItem {

	public override void OnEnable ()
	{
		base.OnEnable ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void OnTrigger (Collider2D collider){
		if (player == null)
			player = GameManager.instance.Player;
		if (collider.gameObject == player.gameObject) {;
			player.doDamage(-damage, false);
			ObjectPool.instance.PoolObject (this.gameObject);
		}
	}
}
