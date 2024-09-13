using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : PoolableObject<T>
{
    [SerializeField] private T _objectPrefab;
    private Pool<T> _pool; 
    
    protected int ActiveObjectsCount { get; private set; }

    private void Awake()
    {
        _pool = new Pool<T>();
    }

    public void Spawn(Vector3 position, Quaternion rotation = new())
    {
        T newObject;
        
        if (_pool.AvailableObjectsCount == 0)
        {
            newObject = Instantiate(_objectPrefab, position, rotation);
            _pool.BindObject(newObject);
        }
        else
        {
            newObject = _pool.GetObject();
        }
        
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.Disabled += Release;
        newObject.gameObject.SetActive(true);
        newObject.Init();
        
        ActiveObjectsCount++;
    }

    private void Release(T objectToRelease)
    {
        objectToRelease.Disabled -= Release; 
        ActiveObjectsCount--;
        objectToRelease.gameObject.SetActive(false);
        _pool.Release(objectToRelease);
    }
}
