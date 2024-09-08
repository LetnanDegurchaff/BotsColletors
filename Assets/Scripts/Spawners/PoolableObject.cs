using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PoolableObject<T> : MonoBehaviour where T : PoolableObject<T>
{
    private Collider _collider;
    
    public event Action<T> Disabled;

    private void OnValidate()
    {
        if (this is not T)
            throw new InvalidGenericTypeException();
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Init()
    {
        _collider.enabled = true;
    }
    
    public void Disable()
    {
        gameObject.SetActive(false);
        Disabled.Invoke((T)this);
    }
    
    private class InvalidGenericTypeException : Exception
    {
        public InvalidGenericTypeException() : base("Generic type type must match the type of the class that inherits this class") { }
    }
}