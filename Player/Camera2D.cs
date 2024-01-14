using Godot;
using System;
using System.Numerics;

public partial class Camera2D : Godot.Camera2D
{
	float decay = 0.1f; //shaking duration
	Godot.Vector2 maxOffset = new Godot.Vector2(100, 75); //max shake in pixels
	float maxRoll = 0.1f; //max rotation in radians

	float trauma = 0.0f; //current shake strength
	float traumaPower = 2.0f; //trauma exponent (use 2 or 3)

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		/* while (GetNode<PlayerData>("/root/PlayerData").shakeTimer.TimeLeft > 0)
		{
			if (GetNode<PlayerData>("/root/PlayerData").playerIsHit)
			{
				GD.Print("if");
				float offsetX = Offset.X;
				float offsetY = Offset.Y;
				offsetX = (float)GD.RandRange(-maxOffset.X, maxOffset.X);
				offsetY = (float)GD.RandRange(-maxOffset.Y, maxOffset.Y);
				GD.Print("offsets genommen");
				Offset = new Godot.Vector2(offsetX, offsetY);
				GD.Print("offsets gesetzt");
			}
		}
		GetNode<PlayerData>("/root/PlayerData").playerIsHit = false;
 */

	}

}
