using Godot;
using System;

public class MainCamera2D : Camera2D
{
    [Export]
    NodePath test;
    float followSpeed = 3f;
    float heightDifference = 0.5f;
    float xDifference = 0.6f;
    KinematicBody2D player;

    float fpsCounter = 0;
    float secondsCounter = 0;

    public override void _Ready()
    {
        player = GetNode(test) as KinematicBody2D;
        this.Position = player.Position;        
    }

    public override void _Process(float delta)
    {
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Lerp(this.Position.x, player.Position.x,delta*followSpeed);
        newPos.y = Mathf.Lerp(this.Position.y, player.Position.y+heightDifference, delta * followSpeed);

        this.Position = newPos;

        fpsCounter += 1;

        secondsCounter += delta;

        if (secondsCounter >= 1)
        {
            GD.Print("FPS: " + fpsCounter);

            fpsCounter = 0;
            secondsCounter = 0;
        }
    }
}
