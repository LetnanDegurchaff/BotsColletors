using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : PoolableObject<Bot>
{
    [SerializeField] private Transform _baggage;
    
    private BotMover _mover;
    private Transform _baseTransform;
    private Crystal _baggageItem;
    
    public event Action<Crystal, Bot> CristalDelivered;
    
    private void Awake()
    {
        _mover = GetComponent<BotMover>();
    }

    public Bot Init(Transform baseTransform)
    {
        _baseTransform = baseTransform;
        return this;
    }
    
    public void ReachCristal(Crystal сristal)
    {
        _baggageItem = сristal;
        _mover.ReachDestination(сristal.transform);
        _mover.ArrivedDestination += ReturnToBase;
    }

    private void ReturnToBase()
    {
        _mover.ArrivedDestination -= ReturnToBase;
        _baggageItem.transform.parent = _baggage;
        _baggageItem.transform.localPosition = Vector3.zero;
        
        _mover.ReachDestination(_baseTransform);
        _mover.ArrivedDestination += ReportDelive;
    }

    private void ReportDelive()
    {
        _mover.ArrivedDestination -= ReportDelive;
        
        _baggageItem.transform.parent = null;
        _baggageItem.Disable();
        CristalDelivered?.Invoke(_baggageItem, this);
        _baggageItem = null;

        _mover.enabled = false;
    }
}