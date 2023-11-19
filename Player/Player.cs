using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public int health = 3;
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
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			}
			else
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
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
				_animationPlayer.Play("idle");
			}
		}
		/* if (velocity.Y > 0)
		{
			_animationPlayer.Play("fall");
		} */

		Velocity = velocity;
		MoveAndSlide();

		if(health<=0)
		{
			QueueFree();
			GetTree().ChangeSceneToFile("res://Main.tscn");
		}
	}
}
