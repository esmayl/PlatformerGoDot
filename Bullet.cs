using Godot;
using System;

public class Bullet : KinematicBody2D
{

    public bool fly = false;
    public int forward = 1;
    Vector2 flyDirection;
    Vector2 gravity = new Vector2(0, 0);

    float gravityTimer = 10;
    float gravityCounter = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (fly)
        {
            flyDirection = new Vector2(forward, 0);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (fly)
        {
            this.MoveAndCollide(flyDirection, false);
        }

        if (gravityCounter < gravityTimer)
        {
            gravityCounter += delta;
        }
        else
        {
            gravity.y += 9.8f*delta;
            this.MoveAndCollide(gravity, false);
        }
    }
}
