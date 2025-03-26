using Godot;
using System;

[GlobalClass]
public partial class DamageType : Resource
{
	public bool OnHit() => false;
	public virtual void OnHitAnything() {}
}
