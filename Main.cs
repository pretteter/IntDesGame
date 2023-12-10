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
}



