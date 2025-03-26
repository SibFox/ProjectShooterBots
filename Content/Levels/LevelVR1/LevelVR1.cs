using Godot;
using System;

public partial class LevelVR1 : Level
{
	public override void OnLoad()
	{
		Player player = GD.Load<PackedScene>("res://Content/Entities/Characters/Players/BasePlayer.tscn").Instantiate<Player>();
		player.Position = PlayerSpawnPosition;
		Players.AddChild(player);
		
		NPC npc = GD.Load<PackedScene>("res://Content/Entities/Characters/NPCs/BaseNPC.tscn").Instantiate<NPC>();
		npc.Position = Main.Rand.RandomPointInRectangle(new(GetNode<Marker2D>("SpawnPoints/Enemy1SpawnPoint").Position, new(100, 100)));
		npc.Rotate((float)Main.Rand.RandomAngle());
		NPCs.AddChild(npc);
	}
}
