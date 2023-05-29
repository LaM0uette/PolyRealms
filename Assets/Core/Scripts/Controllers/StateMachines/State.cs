namespace Core.Scripts.Controllers.StateMachines
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Tick(float deltaTime);
        public abstract void TickLate(float deltaTime);
        public abstract void Exit();
    }
}
