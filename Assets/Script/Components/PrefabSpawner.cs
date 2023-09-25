using UnityEngine;
using UnityEngine.Events;

public class PrefabSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    public event UnityAction<GameObject> OnPrefabSpawn = delegate { };

    public GameObject SpawnPrefab()
    {
        GameObject obj = Instantiate(_prefab);
        OnPrefabSpawn.Invoke(_prefab);
        return obj;
    }

    public T SpawnPrefab<T>()
    {
        T obj = Instantiate(_prefab).GetComponent<T>();
        OnPrefabSpawn.Invoke(_prefab);
        return obj;
    }

    public void SpawnPrefab(Vector3 position)
    {
        Instantiate(_prefab);
        _prefab.transform.position = position;
        OnPrefabSpawn.Invoke(_prefab);
    }
}
