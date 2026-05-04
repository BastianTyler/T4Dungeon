using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4Dungeon.Game.States;

namespace T4Dungeon.Game.Core
{
    public class StateMachine
    {
        private readonly Dictionary<GameStateType, IGameState> _states;
        private IGameState _currentState;
        private IGameState _previousState;


        public StateMachine(Dictionary<GameStateType, IGameState> states)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));
        }

        public void ChangeState(GameStateType newState)
        {
            _currentState?.Exit();

            if (!_states.TryGetValue(newState, out var next))
                throw new Exception($"State {newState} not registered.");
            _previousState = _currentState;
            _currentState = next;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public IGameState GetState(GameStateType type)
        {
            return _states[type];
        }

        public void ReturnToPrevious()
        {
            if (_previousState == null)
                return;

            _currentState?.Exit();

            var temp = _currentState;
            _currentState = _previousState;
            _previousState = temp; 

            _currentState.Enter();
        }
    }
}
