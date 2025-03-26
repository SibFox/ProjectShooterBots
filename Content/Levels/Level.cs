using Godot;

[GlobalClass]
public partial class Level : Node
{
	public Node2D Projectiles => GetNode<Node2D>("Entities/Projectiles");
	public Node2D Players => GetNode<Node2D>("Entities/Players");
	public Node2D NPCs => GetNode<Node2D>("Entities/NPCs");
	public Node2D MapObjects => GetNode<Node2D>("MapObjects");
	public Vector2 PlayerSpawnPosition => GetNode<Marker2D>("SpawnPoints/PlayerSpawnPoint").Position;
	



	public override void _Ready()
	{
		CallDeferred(MethodName.OnLoad);
	}

	public virtual void OnLoad() {}
	
}
