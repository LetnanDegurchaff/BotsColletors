using System;
using System.Collections.Generic;

public class Pool<T> where T : PoolableObject<T>
{
    private List<T> _allObjects = new List<T>();
    private Stack<T> _objects = new Stack<T>();
    
    public int AvailableObjectsCount => 
        _objects.Count;

    public void Release(T @object) => 
        _objects.Push(@object);

    public T GetObject() => 
        _objects.Count > 0 ? _objects.Pop() : null;

    public void BindObject(T @object)
    {
        if (_allObjects.Contains(@object) == false) 
            _allObjects.Add(@object);
        else
            throw new Exception("Object already bound");
    }
}