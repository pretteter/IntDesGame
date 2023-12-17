using Godot;
using System;
using System.Diagnostics.Metrics;

public partial class world : Node2D
{
	PackedScene Enemy = ResourceLoader.Load("res://Enemy.tscn") as PackedScene;
	int enemyCounter = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("SpawnTimer").WaitTime = 60/GetNode<PlayerData>("/root/PlayerData").SpawnRate;
		GD.Print(GetNode<Timer>("SpawnTimer").WaitTime);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void _on_spawn_timer_timeout()
	{
		enemyCounter++;
		var enemy = Enemy.Instantiate() as Enemy;
		enemy.Name = "Enemy" + enemyCounter.ToString();
		
		Random PosOrNeg = new(enemyCounter);
		int randNum = PosOrNeg.Next(1, 3);
		GD.Print(randNum);
		if(randNum == 1){
			Random XCoord = new(enemyCounter);
			enemy.Position = new Vector2(XCoord.Next(90, 130), 370);
			enemy.Position = new Vector2(120, 370); 
		}
		else{
			Random XCoord = new(enemyCounter);
			enemy.Position = new Vector2(XCoord.Next(1100, 1170), 370);
			enemy.Position = new Vector2(1140, 370); 
		}
		AddChild(enemy);
	}
}
