using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
		GetNode<CollisionShape2D>("AtkLeft/CollisionShape2D").Disabled = false;
		GetNode<CollisionShape2D>("AtkRight/CollisionShape2D").Disabled = false;

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

	if (health <= 0)
	{
		QueueFree();
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}
}
//add Action to InputMap
public override async void _Input(InputEvent @event)
{
    if (@event.IsActionPressed("LeftHit"))
    {
		GetNode<Timer>("Timer"); //------------------------------????????
		//GetNode<Area2D>("AtkLeft").SetProcess(true);
		GetNode<CollisionShape2D>("AtkLeft/CollisionShape2D").Disabled = true;
		GD.Print("collision left spawn");

		await ToSignal(GetTree().CreateTimer(1.0), "timeout"); //-----------------FIX THIS (durch timer ersetzen)
		//GetNode<Area2D>("AtkLeft").SetProcess(false);
		GetNode<CollisionShape2D>("AtkLeft/CollisionShape2D").Disabled = false;
		GD.Print("collision left despawn");
    }
}

public void _on_atk_right_body_entered(Node2D body)
{

	if (body.GetType().Name == "Enemy" && GetNode<CollisionShape2D>("AtkRight/CollisionShape2D").Disabled == true)
	{
		Enemy enemy = GetNode<Enemy>("../../Enemy");
		enemy.death();
	}
}
 public void _on_atk_left_body_entered(Node2D body)
{
	GD.Print("left body entered");
	if (body.GetType().Name == "Enemy" && GetNode<CollisionShape2D>("AtkLeft/CollisionShape2D").Disabled == true)
	{
		GD.Print("HitboxEnabled");
		Enemy enemy = GetNode<Enemy>("../../Enemy");
		enemy.death();
	}
} 
/* public void _on_atk_left_input_event(Node2D body)
{

	if (Input.IsActionPressed("LeftHit"))
	{
		//GetNode<Area2D>("AtkLeft").SetProcess(true);
		GetNode<Area2D>("AtkLeft").Show();
		GD.Print("collision left spawn");
		System.Threading.Thread.Sleep(500);
		//GetNode<Area2D>("AtkLeft").SetProcess(false);
		GetNode<Area2D>("AtkLeft").Hide();
		GD.Print("collision left despawn");
	}
} */
}
