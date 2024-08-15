using Fingers.Core.Services.GameStateMachine.States;
using Fingers.UI.Gameplay;
using UnityEngine;

namespace Fingers.Core.Services.GameStateMachine
{
    public class GameplayPauseState : IState
    {
        private readonly GameplayHandler gameplayHandler;

        private float _startGameTime;
        private bool _isActive;

        public GameplayPauseState(GameplayHandler gameplayHandler)
        {
            this.gameplayHandler = gameplayHandler;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            gameplayHandler.PauseGame();
        }

        public void Exit() {}
    }
}