using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerScript : MonoBehaviour {

	private static PoolManagerScript mInstance;
	public static PoolManagerScript Instance
	{
		get{ return mInstance; }
	}

	public List<GameObject> objectsToPool;
	public List<int> numberOfObjectsToPool;
	Dictionary<string,Stack<GameObject>> pool;

	void Awake()
	{
		if(mInstance == null) mInstance = this;
		else if(mInstance != this) Destroy(this.gameObject);
	}

	void Start()
	{
		InitializePoolManager();
	}

	void InitializePoolManager(){
		pool = new Dictionary<string,Stack<GameObject>>();

		for(int i = 0; i < objectsToPool.Count; i++){
			pool.Add( objectsToPool[i].name, new Stack<GameObject>() );
			for(int f = 0; f < numberOfObjectsToPool.Count; f++){
				GameObject go = Instantiate(objectsToPool[i]);
				go.transform.SetParent(this.transform);
				go.gameObject.SetActive(false);
				go.name = objectsToPool[i].name;

				pool[objectsToPool[i].name].Push( go );
			}
		}
	}

	public GameObject GetObject(string name)
	{
		if(pool[name].Count > 0)
		{
			GameObject go = pool[name].Pop();
			return go;
		}
		else
			return null;
	}

//	public GameObject Spawn(string objectName,Vector3 newPosition, Quaternion newRotation){
//		if(pool[objectName].Count > 0)
//		{
//			GameObject go = pool[objectName].Pop();
//			go.transform.position = newPosition;
//			go.transform.rotation = newRotation;
//			go.SetActive(true);
//			return go;
//		}
//		return null;
//	}
//		
//	public void SpawnMuliple(string objectName,Vector3 newPosition, Quaternion newRotation,int amount,float offsetY, float offset,bool isHorizontal)
//	{
//		if(!isHorizontal)
//		{
//			newPosition.x -= offset;
//			//newPosition.z -= offset;
//		}
//		else
//		{
//			newPosition.z -= offset;
//		}
//		for(int i=0; i<amount ; i++)
//		{
//			GameObject newObject = Spawn(objectName, newPosition + new Vector3(0.0f, offsetY, 0.0f), newRotation);
//			if(pool[objectName].Count > 0){
//				GameObject go = pool[objectName].Pop();
//				go.transform.position = newPosition;
//				go.transform.rotation = newRotation;
//				go.transform.Translate(new Vector3(0.0f, offsetY, 0.0f), Space.Self);
//				go.SetActive(true);
//			}
//
//			if(!isHorizontal)
//			{
//				newPosition.x += offset;
//			}
//			else
//			{
//				newPosition.z += offset;
//			}
//		}
//	}

	public void Despawn(GameObject objectToDespawn){
		objectToDespawn.SetActive(false);
		pool[objectToDespawn.name].Push( objectToDespawn );
	}
}
