using System.Collections.Generic;
using UnityEngine;

public class CrystalsScanner : MonoBehaviour
{
    [SerializeField] private float _radius = 100;
    [SerializeField] private LayerMask _crystalsLayer;
    
    public IEnumerable<Crystal> Scan()
    {
        List<Crystal> crystals = new List<Crystal>();
        
        foreach (var collider in Physics.OverlapSphere(transform.position, _radius, _crystalsLayer))
            if (collider.TryGetComponent(out Crystal cristal))
                crystals.Add(cristal);
        
        return crystals;
    }
}