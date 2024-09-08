using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private float _scanDelay = 1;
    private WaitForSeconds _delay;

    [SerializeField] private Bot _botPrefab;
    [SerializeField] private int _botsCount = 0;
    
    private Pool<Bot> _botsPool;
    
    private CristalsDataBase _crystalsDataBase = new();
    private CrystalsScanner _crystalsScanner;

    public event Action<ulong> CrystalCountChanged;
    
    public ulong CrystalsCount { get; private set; }
    
    private void OnValidate()
    {
        _delay = new WaitForSeconds(_scanDelay);
        _crystalsScanner = GetComponent<CrystalsScanner>();
    }
    
    private void Awake()
    {
        _botsPool = new Pool<Bot>(
            () => Instantiate(_botPrefab, transform.position, Quaternion.identity, transform).Init(transform), 
            _botsCount);
    }

    private void OnEnable()
    {
        StartCoroutine(ScanCrystals());
    }

    private void ExtractCrystals()
    {
        if (_botsPool.Count == 0)
            return;
        
        var crystals = _crystalsScanner.Scan();
        
        crystals = _crystalsDataBase.GetFreeCrystals(crystals)
            .OrderBy(crystal => Vector3.Distance(crystal.transform.position, transform.position));

        if (crystals.Any() == false)
            return;

        foreach (var crystal in crystals)
        {
            if (_botsPool.TryGetObject(out Bot bot))
            {
                _crystalsDataBase.ReserveCrystal(crystal);
                bot.ReachCristal(crystal);
                bot.CristalDelivered += ReceiveCrystal;
            }
            else
            {
                break;
            }
        }
    }

    private void ReceiveCrystal(Crystal crystal, Bot bot)
    {
        bot.CristalDelivered -= ReceiveCrystal;
        
        _crystalsDataBase.RemoveReservation(crystal);
        CrystalCountChanged.Invoke(++CrystalsCount);
        
        _botsPool.Realease(bot);
    }
    
    private IEnumerator ScanCrystals()
    {
        while (true)
        {
            yield return _delay;

            ExtractCrystals();
        }
    }
}
