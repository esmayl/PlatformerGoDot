using System;
using Godot;

public class Rocket : BaseBullet
{
    public override void _Ready()
    {
        speed = 1f;

        if (fly)
        {
            flyDirection = new Vector2(forward * speed, 0);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (fly)
        {
            this.MoveAndCollide(base.flyDirection, false);
        }
    }
    
}
