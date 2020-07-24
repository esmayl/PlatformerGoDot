using Godot;
using System;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
    [Export]
    PackedScene[] bullets = new PackedScene[3];

    [Export]
    PackedScene[] buildBullets = new PackedScene[3];

    Dictionary<int, Vector2> bulletSpriteSizes = new Dictionary<int, Vector2>();
    Dictionary<int, Vector2> buildBulletSpriteSizes = new Dictionary<int, Vector2>();

    Vector2 motion = new Vector2();
    float forward = 1;
    float speed = 100f;
    float jumpHeight = 300f;
    float gravity = 980f;
    float drag = 0.04f;
    bool jumping = false;
    bool moving = false;
    float shotCounter = 0;
    float shotTimer = 1f;

    int activeShot = 0;
    bool canShoot = true;

    int[] lr = new int []{ 0,0 };

    //Collision
    KinematicCollision2D lastHit;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (int i = 0; i < buildBullets.Length; i++)
        {
            Node temp = buildBullets[i].Instance() as BaseBullet;
            Sprite s = temp.FindNode("Sprite") as Sprite;
            Vector2 spriteSize = s.Texture.GetSize();

            /* Massively dirty fix for instantiating blocks below the player*/
            spriteSize.y = 0;

            bulletSpriteSizes.Add(i, spriteSize);
        }

        for (int i = 0; i < bullets.Length; i++)
        {
            Node temp = bullets[i].Instance() as BaseBullet;
            Sprite s = temp.FindNode("Sprite") as Sprite;
            Vector2 spriteSize = s.Texture.GetSize();

            /* Massively dirty fix for instantiating blocks below the player*/
            spriteSize.x = 0;

            buildBulletSpriteSizes.Add(i, spriteSize);
        }
    }


    // Only runs when input is detected
    public override void _Input(InputEvent @event)
    {
        // Spawing skills

        if (@event.IsActionPressed("weapon1"))
        {
            activeShot = 0;
        }
        if (@event.IsActionPressed("weapon2"))
        {
            activeShot = 1;
        }
        if (@event.IsActionPressed("weapon3"))
        {
            activeShot = 2;
        }


        if (@event.IsActionPressed("ui_left"))
        {
            motion.x = -speed;
            forward = -1;
            moving = true;
            lr[0] = 1;
        }

        if (@event.IsActionPressed("ui_right"))
        {
            motion.x = speed;
            forward = 1;
            moving = true;
            lr[1] = 1;
        }

        if (@event.IsActionPressed("ui_jump") && !jumping)
        {
            motion.y = -jumpHeight;
            jumping = true;
        }

        if (@event.IsActionPressed("ui_shootl") && !this.IsOnFloor())
        {
            BaseBullet temp = buildBullets[activeShot].Instance() as BaseBullet;
            temp.Position = this.Position+buildBulletSpriteSizes[activeShot];
            GD.Print(bulletSpriteSizes[activeShot]);
            GetParent().AddChild(temp);

            // TO-DO: Test location before instantiating
        }

        if (@event.IsActionPressed("ui_shootr") && canShoot)
        {
            if(activeShot == 2)
            {
                if (!jumping) { GD.Print("not jumping");  return; }
                else { GD.Print("PIEW"); }
            }

            BaseBullet temp = bullets[activeShot].Instance() as BaseBullet;
            temp.Position = this.Position + (bulletSpriteSizes[activeShot]*this.forward);
            temp.forward = (int)this.forward;

            GD.Print(bulletSpriteSizes[activeShot]);

            temp.fly = true;
            GetParent().AddChild(temp);

            canShoot = false;
            shotCounter = 0;
        }


        if (@event.IsActionReleased("ui_left"))
        {
            lr[0] = 0;
        }

        if (@event.IsActionReleased("ui_right"))
        {
            lr[1] = 0;
        }
    }

    // Runs every frame
    public override void _Process(float delta)
    {

        int hits = this.GetSlideCount();
        if (hits > 0)
        {
            if (lastHit != this.GetSlideCollision(hits - 1))
            {
                lastHit = this.GetSlideCollision(hits - 1);
            }
        }

        if (lr[0] == 0 && lr[1] == 0)
        {
            motion.x = 0;
            moving = false;
        }

        if (!canShoot)
        {
            ShotTimer(delta);
        }

        this.MoveAndSlide(motion, upDirection: new Vector2(0, -1), infiniteInertia: false);
    }

    public void ShotTimer(float deltaTime)
    {
        shotCounter += deltaTime;

        if(shotCounter >= shotTimer)
        {
            canShoot = true;
        }
    }

    // Runs every physics step
    public override void _PhysicsProcess(float delta)
    {

        if (this.IsOnFloor())
        {
            motion.y = 0;
            jumping = false;

        }
        else
        {
            motion.y += gravity * delta;
        }


        if (!moving)
        {
            motion.x -= motion.x * drag;
        }

    }

}
