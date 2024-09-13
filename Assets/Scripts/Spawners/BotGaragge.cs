using UnityEngine;
using Object = UnityEngine.Object;

public class BotGaragge
{
    private Pool<Bot> _bots;
    private Bot _botPrefab;

    public BotGaragge(Bot botPrefab, Transform spawnPoint, uint startBotCount)
    {
        _bots = new Pool<Bot>();
        _botPrefab = botPrefab;

        for (int i = 0; i < startBotCount; i++)
        {
            Bot newBot = 
                Object.Instantiate(_botPrefab, spawnPoint.position, Quaternion.identity)
                .Init(spawnPoint);
            
            _bots.BindObject(newBot);
            _bots.Release(newBot);
        }
    }

    public bool HaveFreeBots =>
        _bots.AvailableObjectsCount != 0;

    public bool TryGetBot(out Bot bot)
    {
        if (HaveFreeBots)
        {
            bot = _bots.GetObject();
            return true;
        }
        
        bot = null;
        return false;
    }

    public void Realease(Bot bot) => 
        _bots.Release(bot);
}