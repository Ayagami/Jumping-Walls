using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	[Header("SpawningProperties")]
	public Vector3[] SpawnPositions;
	public float SpawningFrequency = 1.3f;
	public string[] SpawningPrefabs;

	private float spawnTime = 0;
	public bool canSpawn0 = false;

	private int MinSpawnCount = 1;
	// Use this for initialization
	void Start () {
		SpawningFrequency = SpawningFrequency <= 0 ? 1 : SpawningFrequency;
		spawnTime = SpawningFrequency;

		MinSpawnCount = canSpawn0 ? 0 : 1;

	}
	
	// Update is called once per frame
	void Update () {
		spawnTime += Time.deltaTime;
		if (spawnTime >= SpawningFrequency) {
			spawnTime = 0;
			SpawnItems();
		}
	}

	public void SpawnItems(){
		int Quantity = Random.Range (MinSpawnCount, 3); // Esto devuelve 1 o 2. Quizá hacer que devuelva 0 si quiero?

		if (Quantity == 0)
			return;

		if (Quantity == 1){
			Vector3 newPosition = SpawnPositions[getSpawningPosition(1)];
			newPosition.y = GameManager.instance.currentY;

			GameObject go = ObjectPool.instance.GetObjectForType(getRandomPrefab());
			go.transform.position = newPosition;
			return;
		}

		if (Quantity == 2) {
			int r = getSpawningPosition(1);
			int r2 = getSpawningPosition(2, r);

			if( (r==0 || r==2) && (r2==0 || r2==2) ){

				if(r == 0){
					r = 1;
				}else{
					r2 = 1;
				}

			}

			Vector3 newPosition = SpawnPositions[r];
			GameObject go = ObjectPool.instance.GetObjectForType(getRandomPrefab());
			newPosition.y = GameManager.instance.currentY;
			go.transform.position = newPosition;

			newPosition = SpawnPositions[r2];
			go = ObjectPool.instance.GetObjectForType(getRandomPrefab());
			newPosition.y = GameManager.instance.currentY;
			go.transform.position = newPosition;
		}
	}

	private int getSpawningPosition(int Quantity, int exception = -1){
		switch (Quantity) {
			case 1:{
				int r = Random.Range (0,3);
				return r;
				break;
			}
			case 2:{
				
				int r = exception;
				do{
					r = Random.Range(0,3);
				}while( r == exception );

				return r;
				break;
			}
		}

		return 0;
	}

	private string getRandomPrefab(){
		return SpawningPrefabs[Random.Range (0, SpawningPrefabs.Length)] ;
	}
}
