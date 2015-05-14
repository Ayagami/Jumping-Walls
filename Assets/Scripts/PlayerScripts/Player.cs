using UnityEngine;
using System.Collections;


public class Player{

	private GameObject go;
	private Transform tr;
	private Renderer r;

	private Material mat;

	private Color[] _colors = { Color.yellow, Color.red };

	private bool vulnerable = true;

	private float hp = 100f;
	private float maxHP = 100f;

	public  float invulnTime = 1f;

	private float m_fVulnTime;

	public bool applyBlink = false;

	public Player(GameObject _go, float iTime = 1f){

		this.gameObject = _go;
		this.transformation = this.gameObject.transform;
		this.renderer = this.gameObject.GetComponent<Renderer> ();
		this.mat = this.renderer.material;
		this.invulnTime = iTime;
		this.m_fVulnTime = invulnTime;
		this.maxHP = hp;
	}

	public void OnUpdate(){
		if (!isVulnerable()) {
			m_fVulnTime -= Time.deltaTime;

			if(m_fVulnTime <= 0){
				vulnerable = true;
				m_fVulnTime = invulnTime;
			}

		}
	}

	// -------------------------- GETTERS Y SETTERS
	public GameObject gameObject{
		get {
			return go;
		}
		set {
			go = value;
		}
	}
	
	public Transform transformation{
		get {
			return tr;
		}
		set{
			tr = value;
		}
	}

	public Renderer renderer{
		get {
			return r;
		}
		set {
			r = value;
		}
	}

	public bool isVulnerable(){
		return vulnerable;
	}

	// -------------------------- GETTERS Y SETTERS

	public void doDamage(float damage, bool makeVulnerable = true){
		hp -= damage;
		hp = Mathf.Clamp (hp, 0, maxHP);
		GraphicsManager.instance.SetHP ( hp / maxHP );
		vulnerable = !makeVulnerable;
		applyBlink = makeVulnerable;
		checkLoseCondition ();
	}

	private void checkLoseCondition(){
		if (hp <= 0) {
			EventsSystem.sendGameStateChanged(GameManager.GameState.DONE);
		}
	}

	public IEnumerator Blink(float time = -1, float intervalTime = 0.1f){
		time = time == -1 ? m_fVulnTime : time;
		float elapsedTime = 0f;
		int index = 0;
		while (elapsedTime < time) {
			mat.color = _colors[index % 2];
			elapsedTime += Time.deltaTime + intervalTime;
			index++;
			yield return new WaitForSeconds(intervalTime);
		}
		mat.color = Color.white;
	}
}