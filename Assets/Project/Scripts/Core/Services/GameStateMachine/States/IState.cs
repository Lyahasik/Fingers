namespace Fingers.Core.Services.GameStateMachine.States
{
    public interface IState : IOutputState
    {
        public void Enter();
        public void Update() {}
    }
}