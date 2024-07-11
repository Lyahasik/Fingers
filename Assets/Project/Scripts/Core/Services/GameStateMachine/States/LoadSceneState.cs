using Fingers.Core.Coroutines;
using Fingers.UI.Loading;
using UnityEngine;

namespace Fingers.Core.Services.GameStateMachine.States
{
    public class LoadSceneState : IState
    {
        private readonly CoroutinesContainer coroutinesContainer;
        private readonly LoadingCurtain curtain;

        public LoadSceneState(CoroutinesContainer coroutinesContainer,
            LoadingCurtain curtain)
        {
            this.coroutinesContainer = coroutinesContainer;
            this.curtain = curtain;
        }

        public void Enter()
        {
            Debug.Log($"Start state { GetType().Name }");
            
            curtain.Show();
        }

        public void Exit()
        {
            curtain.Hide(coroutinesContainer);
        }
    }
}