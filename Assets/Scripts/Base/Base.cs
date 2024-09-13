using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CrystalsScanner))]
public class Base : MonoBehaviour
{
    [SerializeField] private float _scanDelayTime = 1;
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private uint _botsCount = 3;
    
    private BotGaragge _botGaragge;
    
    private CristalsDatabase _crystalsDatabase = new();
    private CrystalsScanner _crystalsScanner;
    
    private WaitForSeconds _scanDelay;

    public event Action<ulong> CrystalCountChanged;
    
    public ulong CrystalsCount { get; private set; }
    
    private void Awake()
    {
        _crystalsScanner = GetComponent<CrystalsScanner>();
        _scanDelay = new WaitForSeconds(_scanDelayTime);
        
        _botGaragge = new BotGaragge(_botPrefab, transform, _botsCount);
    }

    private void OnEnable()
    {
        StartCoroutine(ExtractCrystals());
    }

    private void SendBotsByCrystals()
    {
        if (_botGaragge.HaveFreeBots == false)
            return;
        
        var crystals = _crystalsScanner.Scan();
        
        crystals = _crystalsDatabase.GetFreeCrystals(crystals)
            .OrderBy(crystal => (crystal.transform.position - transform.position).sqrMagnitude);

        if (crystals.Any() == false)
            return;

        foreach (var crystal in crystals)
        {
            if (_botGaragge.TryGetBot(out Bot bot))
            {
                _crystalsDatabase.ReserveCrystal(crystal);
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
        
        _crystalsDatabase.RemoveReservation(crystal);
        CrystalCountChanged.Invoke(++CrystalsCount);
        
        _botGaragge.Realease(bot);
    }
    
    private IEnumerator ExtractCrystals()
    {
        while (true)
        {
            yield return _scanDelay;

            SendBotsByCrystals();
        }
    }
}
