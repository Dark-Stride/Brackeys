using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Systems
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance;
        private Dictionary<string, Queue<GameObject>> poolDictionary = new();

        void Awake() => Instance = this;

        public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            string key = prefab.name;
            if (!poolDictionary.ContainsKey(key)) poolDictionary.Add(key, new Queue<GameObject>());

            if (poolDictionary[key].Count == 0)
            {
                GameObject obj = Instantiate(prefab);
                obj.name = key;
                return obj;
            }

            GameObject objectToSpawn = poolDictionary[key].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.SetPositionAndRotation(position, rotation);
            return objectToSpawn;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            poolDictionary[obj.name].Enqueue(obj);
        }
    }
}
