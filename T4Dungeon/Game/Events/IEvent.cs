using T4Dungeon.Game.Models;

namespace T4Dungeon.Game.Events
{
    public interface IEvent
    {
        void Execute(Player player);
    }
}
