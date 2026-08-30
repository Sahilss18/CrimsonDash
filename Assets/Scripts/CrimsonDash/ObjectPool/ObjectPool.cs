using System.Collections;
using System.Collections.Generic;
using CrimsonDash.Collectables;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] List<Pool> m_pools;
    private Dictionary<PoolTag, Queue<Collectable>> m_poolDictionary;
    private Dictionary<PoolTag, Pool> m_poolPrefabDictionary;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_poolDictionary = new Dictionary<PoolTag, Queue<Collectable>>();
        m_poolPrefabDictionary = new Dictionary<PoolTag, Pool>();

        for (int poolIndex = 0; poolIndex < m_pools.Count; poolIndex++)
        {
            Pool pool = m_pools[poolIndex];
            Queue<Collectable> objectPool = new Queue<Collectable>();

            for (int i = 0; i < pool.m_size; i++)
            {
                Collectable obj = Instantiate(pool.m_prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
                obj.transform.SetParent(transform);
            }

            m_poolDictionary.Add(pool.m_tag, objectPool);

            if (!m_poolPrefabDictionary.ContainsKey(pool.m_tag))
            {
                m_poolPrefabDictionary.Add(pool.m_tag, pool);
            }
            else
            {
                Debug.LogWarning($"Duplicate pool tag {pool.m_tag} detected in pool prefabs.");
            }
        }
    }

    public Collectable SpawnFromPool(PoolTag tag, Vector3 position, Quaternion rotation)
    {
        if (!m_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<Collectable> objectPool = m_poolDictionary[tag];
        Collectable objectToSpawn = null;

        if (objectPool.Count > 0)
        {
            objectToSpawn = objectPool.Dequeue();
        }
        else
        {
            if (m_poolPrefabDictionary.TryGetValue(tag, out Pool pool))
            {
                objectToSpawn = Instantiate(pool.m_prefab);
                objectToSpawn.transform.SetParent(transform);
            }
            else
            {
                Debug.LogWarning($"Pool prefab for tag {tag} not found.");
            }
        }

        if (objectToSpawn != null)
        {
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }

        return objectToSpawn;
    }

    public void ReturnToPool(PoolTag tag, Collectable objectToReturn, float delay)
    {
        StartCoroutine(ReturnToPoolAfterDelay(tag, objectToReturn, delay));
    }

    private IEnumerator ReturnToPoolAfterDelay(PoolTag tag, Collectable objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!m_poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            yield break;
        }

        objectToReturn.gameObject.SetActive(false);
        objectToReturn.transform.SetParent(transform);
        m_poolDictionary[tag].Enqueue(objectToReturn);
    }
}

[System.Serializable]
public class Pool
{
    public PoolTag m_tag;
    public Collectable m_prefab;
    public int m_size;
}

public enum PoolTag
{
    None = 0,
    Coins = 1
}
