using Godot;
using System;

public partial class BG : ParallaxBackground
{
	double scrolling_speed = 100f;

	public override void _Process(double delta)
	{
		ScrollOffset = new Vector2(ScrollOffset.X - (float)scrolling_speed * (float)delta, Offset.Y);
	}
}

