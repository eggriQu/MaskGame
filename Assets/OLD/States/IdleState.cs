using UnityEngine;

public interface IState
{
    void Handle();
}

public abstract class State : IState
{
    protected Entity controller;

    public State(Entity controller)
    {
        this.controller = controller;
    }

    public abstract void Handle();
}

public class IdleState : State
{
    public IdleState(Entity controller) : base(controller)
    {

    }

    public override void Handle()
    {
        controller.rb.linearVelocity = new Vector2(0, controller.rb.linearVelocity.y);
    }
}
