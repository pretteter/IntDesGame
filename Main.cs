using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Main : Node
{

	public override void _Ready()
	{
		GD.Print("Hello World!");
	}

	public void _on_start_pressed()
	{
		GD.Print("Starting Game");
		GetTree().ChangeSceneToFile("res://world.tscn");
	}

	public void _on_quit_pressed()
	{
		GD.Print("Quitting Game");
		GetTree().Quit();
	}
	public void _on_spawn_rate_value_changed(double value)
	{
		GD.Print("SpawnRate Changed");
		//GetNode<Timer>("././world/SpawnTimer").WaitTime = GetNode<HSlider>("SpawnRate").Value * 0.1f;
		//GD.Print(GetNode<HSlider>("SpawnRate").GetParent().GetParent().GetNode<Timer>("SpawnTimer"));
		//GetNode<PlayerData>("/root/PlayerData");
		GetNode<Label>("SpawnRate/RateDisplay").Text = value.ToString();
		GetNode<PlayerData>("/root/PlayerData").SpawnRate = value;
	}
}



