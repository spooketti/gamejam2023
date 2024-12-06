using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonGenerator : Node
{

	public static int dungeonWidth = 5;
	public static int dungeonHeight = 5;

	public static int tileX = 30;
	public static int tileZ = 30; //this is 1/2 the size and is to be used as a scale factor
	//eg 3 from the right in the matrix is 3*15=45 in global coordinates
	private PackedScene fourWay = GD.Load<PackedScene>("res://Assets/Rooms/4_way.tscn");
	private PackedScene spawnRoom = GD.Load<PackedScene>("res://Assets/Rooms/spawn.tscn");

	private Random random = new Random();

	private int startX = 0;
	private int startZ = 0;
	private int[,] dungeon = new int[dungeonWidth,dungeonHeight];

		//0xxxxx
		//z
		//z
		//z

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnStartRoom();
		// spawnTile(startingRoomX,startingRoomZ);
		// // for (int i = 0; i < rooms.GetLength(0); i++)
		// // {
		// // 	for (int j = 0; j < rooms.GetLength(1); j++)
		// // 	{
		// // 		GD.Print(rooms[i,j]);
		// // 	}
		// // }

	}

	private void spawnStartRoom()
	{
		startX = random.Next(dungeonWidth);
		startZ = random.Next(dungeonHeight);
		int randomRotation = random.Next(4)*90;
		int oldX = startX;
		int oldZ = startZ;
		//startRoom points south by default
		switch(randomRotation)
		{
			case 0:
			startZ++;
			break;

			case 90:
			startX++;
			break;

			case 180:
			startZ--;
			break;

			case 270:
			startX--;
			break;
		}

		if(startX < 0 || startX == dungeonWidth || startZ < 0 || startZ == dungeonWidth)
		{
			spawnStartRoom();
			return;
		}

		startX = oldX;
		startZ = oldZ;
		

		dungeon[startZ,startX] = 2;
		startX = (startX*tileX)+(tileX/2);
		startZ = (startZ*tileZ)+(tileZ/2);

		spawnTileLiteral(startX,startZ,randomRotation,spawnRoom);
	}

	private void spawnTileLiteral(int x, int z, int rotation,PackedScene room)
	{
		Node3D roomModel = room.Instantiate<Node3D>();
		roomModel.GlobalPosition = new Vector3(x,0,z);
		roomModel.RotateY(Mathf.DegToRad(rotation));
		AddChild(roomModel);
		for (int i = 0; i < dungeon.GetLength(0); i++)
		{
			for (int j = 0; j < dungeon.GetLength(1); j++)
			{
				GD.Print(dungeon[i,j]);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
