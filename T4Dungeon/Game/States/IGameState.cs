using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4Dungeon.Game.States
{
    public interface IGameState
    {
        void Enter(); 
        void Update();
        void Exit();

    }
}
