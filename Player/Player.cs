using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class Player : CharacterBody2D
{
	public int health = 3;
	public int score = 0;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("idle");
		GD.Print("Ready");
	}
	//private AnimatedSprite2D _animatedSprite;
	/* public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animatedSprite.Play("idle");
		GD.Print("Ready");
	} */



	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
		}
		/*
				// Handle Jump.
				if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
				{
					velocity.Y = JumpVelocity;
					_animationPlayer.Play("jump");
				}
		 */
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		if (direction != Vector2.Zero)
		{
			if (direction[0] < 0)
			{
				//GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
				if (GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.X < 0)
				{
					//GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").ApplyScale(new Vector2(-1, 1));
				}

			}
			else
			{
				if (GetNode<AnimatedSprite2D>("AnimatedSprite2D").Scale.X > 0)
				{
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").ApplyScale(new Vector2(-1, 1));
				}
				
			}
			/* 
						velocity.X = direction.X * Speed;
						if (velocity.Y == 0)
						{
							_animationPlayer.Play("run");
						} */
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			if (velocity.Y == 0)
			{
				if (_animationPlayer.CurrentAnimation != "attack")
				{
					_animationPlayer.Play("idle");
				}
			}
		}
		/* if (velocity.Y > 0)
		{
			_animationPlayer.Play("fall");
		} */

		Velocity = velocity;
		MoveAndSlide();

		if (health <= 0)
		{
			QueueFree();
			GetTree().ChangeSceneToFile("res://Main.tscn");
		}
	}
	public void _on_gun_hit_body_entered(Node2D body)
	{
		GD.Print("gun hit area entered");
		GD.Print("Body in Hitbox: " + body.Name);
        string bodyName = body.Name.ToString();


        if (bodyName.Contains("Enemy") )
		{
			GD.Print("enemy detected");
			Enemy enemy = GetNode<Enemy>("../../" + bodyName);
			score += 1000;
			enemy.death();
		}
	}



	//add Action to InputMap
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("LeftHit"))
		{
			GD.Print("AttackAnimLeft");
			_animationPlayer.Play("attack");

		}
		if (@event.IsActionPressed("RightHit"))
		{
			GD.Print("AttackAnimRight");
			_animationPlayer.Play("attack");
		}
	}
}
