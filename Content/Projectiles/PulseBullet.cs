using Godot;
using System;

public partial class PulseBullet : Projectile
{
	protected override void OnRichochet(Node2D body)
	{
		Stats.RicochetChance -= 30;
	}

}
