using UnityEngine;

public interface IState
{
    void Handle();
}

public abstract class State : IState
{
    protected EnemyAI controller;

    public State(EnemyAI controller)
    {
        this.controller = controller;
    }

    public abstract void Handle();
}

public class IdleState : State
{
    public IdleState(EnemyAI controller) : base(controller)
    {

    }

    public override void Handle()
    {
        controller.rb.linearVelocity = new Vector2(0, controller.rb.linearVelocity.y);
    }
}
