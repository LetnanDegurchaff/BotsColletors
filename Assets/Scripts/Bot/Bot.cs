using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _baggage;
    
    private BotMover _botMover;
    private Transform _baseTransform;
    private Crystal _baggageItem;
    
    public event Action<Crystal, Bot> CristalDelivered;
    
    private void Awake()
    {
        _botMover = GetComponent<BotMover>();
    }

    public Bot Init(Transform baseTransform)
    {
        _baseTransform = baseTransform;
        return this;
    }
    
    public void ReachCristal(Crystal сristal)
    {
        _baggageItem = сristal;
        _botMover.ReachDestination(сristal.transform);
        _botMover.ArrivedDestination += ReturnToBase;
    }

    private void ReturnToBase()
    {
        _botMover.ArrivedDestination -= ReturnToBase;
        _baggageItem.transform.parent = _baggage;
        _baggageItem.transform.localPosition = Vector3.zero;
        
        _botMover.ReachDestination(_baseTransform);
        _botMover.ArrivedDestination += ReportDelive;
    }

    private void ReportDelive()
    {
        _botMover.ArrivedDestination -= ReportDelive;
        
        _baggageItem.transform.parent = null;
        _baggageItem.Disable();
        CristalDelivered?.Invoke(_baggageItem, this);
        _baggageItem = null;

        _botMover.enabled = false;
    }
}