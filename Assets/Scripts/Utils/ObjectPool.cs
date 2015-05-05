using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour{
	
	public static ObjectPool instance;

	///  Arrays de prefabs que van a utilizarse para Pools.
	public GameObject[] objectPrefabs;

	/// Objetos disponibles para usar...
	[SerializeField]
	public List<GameObject>[] pooledObjects;

	/// Cantidad a buffear por cada tipo de GameObject
	public int[] amountToBuffer;
	
	public int defaultBufferAmount = 3;
	
	protected GameObject containerObject;
	
	void Awake (){
		instance = this;
	}
	
	// Use this for initialization
	void Start (){
		containerObject = new GameObject("ObjectPool");
	
		pooledObjects = new List<GameObject>[objectPrefabs.Length];
		
		int i = 0;
		foreach ( GameObject objectPrefab in objectPrefabs )
		{
			pooledObjects[i] = new List<GameObject>(); 
			
			int bufferAmount;
			
			if(i < amountToBuffer.Length) bufferAmount = amountToBuffer[i];
			else
				bufferAmount = defaultBufferAmount;
			
			for ( int n=0; n<bufferAmount; n++)
			{
				GameObject newObj = Instantiate(objectPrefab) as GameObject;
				newObj.name = objectPrefab.name;
				PoolObject(newObj);
			}
			
			i++;
		}
	}
	
	/// Retorno un GameObject de un determinado tipo.
	/// El tipo se determina por el nombre del Prefab.
	/// onlyPooled: 
	/// 	if(false) Crea el objeto en el pool.
	/// 	else      No lo crea, y no lo devuelve.
	/// Ejemplo: .GetObjectForType("Room", false);
	public GameObject GetObjectForType ( string objectType , bool onlyPooled = false){
		for(int i=0; i<objectPrefabs.Length; i++)
		{
			GameObject prefab = objectPrefabs[i];
			if(prefab.name == objectType)
			{
				
				if(pooledObjects[i].Count > 0)
				{
					GameObject pooledObject = pooledObjects[i][0];
					pooledObjects[i].RemoveAt(0);
					pooledObject.transform.parent = null;
					pooledObject.SetActiveRecursively(true);
					
					return pooledObject;
					
				} else if(!onlyPooled) {
					GameObject go = Instantiate(objectPrefabs[i]) as GameObject;
					go.name = prefab.name;
					return go; 
				}
				
				break;
				
			}
		}
		
		//Devuelvo NULL por defecto...
		return null;
	}

	public void PoolObject ( GameObject obj ){
		for ( int i=0; i<objectPrefabs.Length; i++)
		{
			if(objectPrefabs[i].name == obj.name)
			{
				obj.SetActiveRecursively(false);
				obj.transform.parent = containerObject.transform;
				pooledObjects[i].Add(obj);
				return;
			}
		}
	}
	
}