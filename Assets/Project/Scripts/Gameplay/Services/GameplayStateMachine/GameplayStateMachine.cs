using System;
using System.Collections.Generic;
using Fingers.Core.Services.GameStateMachine.States;
using Fingers.Core.Services.StaticData;
using Fingers.Core.Update;
using Fingers.UI.Gameplay;

namespace Fingers.Core.Services.GameStateMachine
{
    public class GameplayStateMachine : IGameStateMachine, IUpdating
    {
        private Dictionary<Type, IState> _states;
        
        private IState _activeState;

        public IState ActiveState => _activeState;

        public void Initialize(IStaticDataService staticDataService, UpdateHandler updateHandler, GameplayHandler gameplayHandler)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(GameplayInactiveState)] = new GameplayInactiveState(gameplayHandler),
                [typeof(GameplayPrepareState)] = new GameplayPrepareState(gameplayHandler),
                [typeof(GameplayPauseState)] = new GameplayPauseState(gameplayHandler),
                [typeof(GameplayActiveState)] = new GameplayActiveState(gameplayHandler)
            };
            
            updateHandler.AddUpdatedObject(this);
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state?.Enter();
        }

        public void Enter<TState, TData>(TData data) where TState : class, IDataState<TData> {}

        public void Update()
        {
            _activeState?.Update();
        }

        public TState GetState<TState>() where TState : class, IState => 
            _states[typeof(TState)] as TState;

        private TState ChangeState<TState>() where TState : class, IState
        {
            if (_activeState is TState)
                return null;
            
            _activeState?.Exit();
      
            TState state = GetState<TState>();
            _activeState = state;
      
            return state;
        }
    }
}