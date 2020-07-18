using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export]
    PackedScene bullet = GD.Load("res://Bullet.tscn") as PackedScene;

    Vector2 motion = new Vector2();
    float forward = 1;
    float speed = 100f;
    float jumpHeight = 300f;
    float gravity = 980f;
    float drag = 0.38f;
    bool jumping = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }



    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_left"))
        {
            motion.x = -speed;
            forward = -1;
        }
        if (@event.IsActionReleased("ui_left"))
        {
            motion.x = 0;
        }

        if (@event.IsActionPressed("ui_right"))
        {
            motion.x = speed;
            forward = 1;
        }
        if (@event.IsActionReleased("ui_right"))
        {
            motion.x = 0;
        }

        if (@event.IsActionPressed("ui_jump") && !jumping)
        {
            motion.y = -jumpHeight;
            jumping = true;
        }

        if (@event.IsActionPressed("ui_shootl"))
        {
            KinematicBody2D temp = bullet.Instance() as KinematicBody2D;
            temp.Position = this.Position-Vector2.Up;
            GetParent().AddChild(temp);
        }

        if (@event.IsActionPressed("ui_shootr"))
        {
            Bullet temp = bullet.Instance() as Bullet;
            temp.Position = this.Position + new Vector2(this.forward*65,0);
            temp.forward = (int)this.forward*9;

            temp.fly = true;
            GetParent().AddChild(temp);
        }
    }

    public override void _Process(float delta)
    {
        if (this.IsOnFloor() && !jumping)
        {
            motion.y = 0;
        }
        else if (this.IsOnFloor() && jumping)
        {
            jumping = false;
        }
        else if (!this.IsOnFloor())
        {
            motion.y += gravity * delta;
        }

        this.MoveAndSlide(motion, upDirection: new Vector2(0, -1), infiniteInertia: false);
    }

}
