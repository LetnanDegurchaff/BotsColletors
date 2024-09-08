using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pool<T> where T : MonoBehaviour
{
    private List<T> _allObjects = new List<T>();
    private Stack<T> _availableObjects = new Stack<T>();

    public Pool(Func<T> createObject, int startAmount)
    {
        CreateObject = () =>
        {
            T newObject = createObject();
            _allObjects.Add(newObject);
            return newObject;
        };

        for (int i = 0; i < startAmount; i++)
            _availableObjects.Push(CreateObject());
    }
    
    public Pool(Func<T> createObject) : this(createObject, 0) { }

    protected Func<T> CreateObject { get; private set; }
    
    public IEnumerable<T> AllObjects => _allObjects;
    
    public int Count => _availableObjects.Count;
    
    public void AddObject() => 
        _availableObjects.Push(CreateObject());

    public bool TryGetObject(out T @object) => 
        _availableObjects.TryPop(out @object);

    public void Realease(T @object) => 
        _availableObjects.Push(@object);
}