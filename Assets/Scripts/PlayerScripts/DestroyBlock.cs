using UnityEngine;
using System.Collections;

public class DestroyBlock : Actions {
	// Use this for initialization
    private GameObject target = null;

	public override void OnStart () {
        base.OnStart();
        m_sName = "DestroyBlock";
        m_sTag  = "Hitable";
        actionTagOnData = "DESTROY_BLOCK";
    }
	
	// Update is called once per frame
	public override void OnUpdate () {
        base.OnUpdate();
	}

    public override void Action(){
        //Debug.Log("DestroyBlock bitch!!!");
        if (target) {
            Debug.Log("Pooling gameObject bitch " + target.gameObject.name);
            ObjectPool.instance.PoolObject(target.gameObject);
            target = null;
        }
    }

    void OnTriggerEnter2D(Collider2D obj){
        if (obj.tag.Equals(m_sTag)) {
            target = obj.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D obj){
        if (obj.tag.Equals(m_sTag) && obj.gameObject == target) {
            target = null;
        }
    }
}
