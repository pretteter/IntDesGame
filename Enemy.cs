using Godot;
using System;
using System.Collections;

public partial class Enemy : CharacterBody2D
{
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public Player player;
	public bool chase;
	public const float SPEED = 50.0f;


	public override void _Ready()
	{
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walking");
	}
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		//Gravity for frog
		velocity.Y += gravity * (float)delta;
		if (chase == true)
		{
			if (GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation != "death")
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walking");
			}
			player = GetNode<Player>("../../Player/Player");
			Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
			if (direction.X > 0)
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
			}
			else
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			}
			//Move frog
			velocity.X = direction.X * SPEED;
		}
		else
		{
			if (GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation != "death")
			{
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
			}
			velocity.X = 0;
		}



		Velocity = velocity;
		MoveAndSlide();
	}

	public void _on_player_detection_body_entered(Node2D body)
	{
		if (body.GetType().Name == "Player")
		{
			chase = true;
		}
	}
	public void _on_player_detection_body_exited(Node2D body)
	{
		if (body.GetType().Name == "Player")
		{
			chase = false;
		}
	}
	public void _on_player_death_body_entered(Node2D body)
	{
		if (body.GetType().Name == "Player")
		{
			death();
		}
	}
	public void _on_player_collision_body_entered(Node2D body)
	{
		if (body.GetType().Name == "Player")
		{
			player.health -= 1;
			death();
		}
	}
	public async void death()
	{
		chase = false;
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("death");
		await ToSignal(GetNode<AnimatedSprite2D>("AnimatedSprite2D"), "animation_finished");
		QueueFree();
	}
}
