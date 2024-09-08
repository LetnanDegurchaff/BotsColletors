using System;
using UnityEngine;

[Serializable] 
public class UnlimitPool<T> : Pool<T> where T : MonoBehaviour
{
    public UnlimitPool(Func<T> createObject) : base(createObject) { }
    
    public T Get()
    {
        if (TryGetObject(out T obj))
            return obj;
        
        return CreateObject();
    }
}