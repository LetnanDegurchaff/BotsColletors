using System.Collections;
using UnityEngine;

public class CrystalSpawner : Spawner<Crystal>
{
    [SerializeField] private float _maxCristalsCount = 10;
    
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider _cristalCollider;
    
    [SerializeField] private float _spawnDelay;
    private WaitForSeconds _delay;
    
    private void OnValidate()
    {
        _delay = new WaitForSeconds(_spawnDelay);
    }

    private void OnEnable()
    {
        StartCoroutine(SpawningRoutine());
    }

    private IEnumerator SpawningRoutine()
    {
        while (true)
        {
            yield return _delay;
            
            if (ActiveObjectsCount < _maxCristalsCount)
                Spawn(GetRandomPosition());
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(_collider.bounds.min.x, _collider.bounds.max.x), 
            0,
            Random.Range(_collider.bounds.min.z, _collider.bounds.max.z));;
    }
}
