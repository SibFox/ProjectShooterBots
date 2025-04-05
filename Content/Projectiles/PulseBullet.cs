using Godot;
using System;

public partial class PulseBullet : Projectile
{
	protected override void OnRichochet()
	{
		Stats.RicochetChance -= 30;
	}

}
