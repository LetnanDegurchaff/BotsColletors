using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : PoolableObject<T>
{
    [SerializeField] private T _objectPrefab;
    private UnlimitPool<T> _pool;
    
    protected int ActiveObjectsCount { get; private set; }

    private void Awake()
    {
        _pool = new UnlimitPool<T>(() =>
            Instantiate(_objectPrefab, transform.position, Quaternion.identity, transform));

        foreach (var @object in _pool.AllObjects) 
            @object.gameObject.SetActive(false);
    }

    public T Spawn(Vector3 position, Quaternion rotation = new())
    {
        ActiveObjectsCount++;
        T newObject = _pool.Get();
        
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.Disabled += Release;
        newObject.gameObject.SetActive(true);
        newObject.Init();
        
        return newObject;
    }

    private void Release(T objectToRelease)
    {
        objectToRelease.Disabled -= Release; 
        ActiveObjectsCount--;
        objectToRelease.gameObject.SetActive(false);
        _pool.Realease(objectToRelease);
    }
}
