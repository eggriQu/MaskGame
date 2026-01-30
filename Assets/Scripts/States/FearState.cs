using UnityEngine;

public class FearState : State
{
    public FearState(EnemyAI controller) : base(controller)
    {

    }

    public override void Handle()
    {
        controller.rb.linearVelocity = new Vector2(-controller.followDirection.x, controller.rb.linearVelocity.y).normalized * 3.5f;
    }
}
