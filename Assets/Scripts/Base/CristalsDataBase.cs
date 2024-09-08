using System.Collections.Generic;
using System.Linq;

public class CristalsDataBase
{
    private readonly HashSet<Crystal> _crystals = new();
    
    public void ReserveCrystal(Crystal crystal) => 
        _crystals.Add(crystal);
    
    public IEnumerable<Crystal> GetFreeCrystals(IEnumerable<Crystal> cristals) => 
        cristals.Where(cristal => _crystals.Contains(cristal) == false);

    public void RemoveReservation(Crystal crystal) => 
        _crystals.Remove(crystal); 
}