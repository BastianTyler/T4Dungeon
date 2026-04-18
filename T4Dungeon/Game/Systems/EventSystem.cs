using T4Dungeon.Game.Events;
using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Systems
{
    public class EventSystem
    {

        private readonly Random _rng = new();

        //public Cell GetNextCell()
        //{
        //    int roll = _rng.Next(0, 4);

        //    IEvent ev = roll switch
        //    {
        //        //0 => new CombatEvent(),
        //        //1 => new TreasureEvent(),
        //        //2 => new ShopEvent(),
        //        //_ => new EmptyEvent()
        //    };

        //    bool isExit = _rng.NextDouble() < 0.1;

        //    return new Cell(ev, isExit);
        //}
    }
    
}
