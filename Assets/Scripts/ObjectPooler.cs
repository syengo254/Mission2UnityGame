using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPooler: MonoBehaviour
{  
    public GameObject objectPrefab;
    public int poolSize = 2;

    private List<GameObject> objectPool;

    public static ObjectPooler Instance;

    private void Awake() {
        Instance = this;
    }
    
    private void Start() {
        objectPool = new List<GameObject>(poolSize);

        if(objectPrefab != null)
        {
            for(int i = 0; i < poolSize; i++)
            {
                GameObject item = Instantiate(objectPrefab);
                item.SetActive(false);
                item.transform.SetParent(this.transform);
                objectPool.Add(item);
            }
        }
    }

    public GameObject GetItem()
    {
        for(int i = 0; i < poolSize; i++)
        {
            if(!objectPool[i].activeInHierarchy){
                return objectPool[i];
            }
        }

        return null;
    }
}