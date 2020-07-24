using System;
using Godot;
public class IceSpice : BaseBullet
{
    public override void _Ready()
    {
        speed = 10f;

        if (fly)
        {
            flyDirection = new Vector2(forward * speed, 4.4f);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (fly)
        {
            KinematicCollision2D col = this.MoveAndCollide(base.flyDirection, false);

            if (col != null)
            {
                fly = false;
                GD.Print("Hit!");
            }
        }

    }
}


