using UnityEngine;

public class PatrolState : State
{
    private int direction;

    public PatrolState(EnemyAI controller) : base(controller)
    {
        if (Random.Range(0, 2) == 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    public override void Handle()
    {
        controller.rb.linearVelocity = new Vector2(direction * 3, controller.rb.linearVelocity.y);
    }
}
