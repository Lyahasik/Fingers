using Fingers.Core.Services.GameStateMachine.States;
using Fingers.UI.Gameplay;
using UnityEngine;

namespace Fingers.Core.Services.GameStateMachine
{
    public class GameplayPrepareState : IState
    {
        private readonly GameplayHandler gameplayHandler;

        private float _startGameTime;

        public GameplayPrepareState(GameplayHandler gameplayHandler)
        {
            this.gameplayHandler = gameplayHandler;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
        }

        public void Exit() {}
    }
}