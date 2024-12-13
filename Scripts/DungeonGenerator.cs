using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class DungeonGenerator : Node
{

	public static int dungeonWidth = 10;
	public static int dungeonHeight = 10;

	public static int tileX = 30;
	public static int tileZ = 30; //this is 1/2 the size and is to be used as a scale factor
								  //eg 3 from the right in the matrix is 3*15=45 in global coordinates

	private static PackedScene fourWay = GD.Load<PackedScene>("res://Assets/Rooms/4_way.tscn");
	private static PackedScene spawnRoom = GD.Load<PackedScene>("res://Assets/Rooms/spawn.tscn");

	class Room
	{
		public int x;
		public int z;
		public int rotation;
		public bool north = false; //true == this has an available entrance
		public bool east = false;
		public bool west = false;
		public bool south = false;
		public PackedScene roomModel;
		public Room(int x, int z, int rot, bool north, bool east, bool south, bool west, PackedScene scene)
		{
			this.x = x;
			this.z = z;
			rotation = rot;
			this.north = north;
			this.east = east;
			this.south = south;
			this.west = west;
			roomModel = scene;
		}

	}

	private Random random = new Random();

	private int startX = 0;
	private int startZ = 0;
	private Room[,] dungeon = new Room[dungeonWidth, dungeonHeight];

	//0xxxxx
	//z
	//z
	//z

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawnStartRoom();
	}

	private void spawnMathTile(Room roomToSpawn)
	{

		//TODO: comeback to this as this is where you left off
		//to save the moment: 12/12/2024 12:41PM -> listening to buzz buzz propehcy
		//feeling p emotional given something i wont say for the sake of self preservation when i read this ater
		int x = roomToSpawn.x;
		int z = roomToSpawn.z;
		if (x < 0 || x > dungeonWidth || z < 0 || z > dungeonHeight)
		{
			return;
		}
		if (dungeon[z, x] != null)
		{
			return;
		}
		dungeon[z, x] = roomToSpawn;
		spawnTileLiteral((x * tileX) + (tileX / 2), (z * tileZ) + (tileZ / 2), 0, roomToSpawn.roomModel);
		if (roomToSpawn.north && z - 1 > 0 && dungeon[z-1, x] == null)
		{
			spawnTileLiteral((x * tileX) + (tileX / 2), ((z - 1) * tileZ) + (tileZ / 2), 0, fourWay);
		}
		if (roomToSpawn.east && x + 1 < dungeonWidth && dungeon[z, x+1] == null)
		{
			spawnTileLiteral(((x+1) * tileX) + (tileX / 2), (z * tileZ) + (tileZ / 2), 0, fourWay);
		}
		if (roomToSpawn.south && z + 1 < dungeonHeight && dungeon[z+1, x] == null)
		{
			spawnTileLiteral((x * tileX) + (tileX / 2), ((z + 1) * tileZ) + (tileZ / 2), 0, fourWay);
		}
		if (roomToSpawn.west && x - 1 > 0 && dungeon[z, x-1] == null)
		{
			spawnTileLiteral(((x-1) * tileX) + (tileX / 2), (z * tileZ) + (tileZ / 2), 0, fourWay);
		}
	}
	private void spawnStartRoom()
	{
		startX = 5;//random.Next(5);
		startZ = 2;//random.Next(5);
		int randomRotation = random.Next(4) * 90;
		int oldX = startX;
		int oldZ = startZ;
		//startRoom points south by default
		switch (randomRotation)
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

		if (startX < 0 || startX == dungeonWidth || startZ < 0 || startZ == dungeonWidth)
		{
			spawnStartRoom();
			return;
		}

		startX = oldX;
		startZ = oldZ;

		Room roomToSpawn = new Room(startX, startZ, randomRotation, true, true, true, true, spawnRoom);

		spawnMathTile(roomToSpawn);
	}

	private void spawnTileLiteral(int x, int z, int rotation, PackedScene room)
	{
		Node3D roomModel = room.Instantiate<Node3D>();
		roomModel.GlobalPosition = new Vector3(x, 0, z);
		// roomModel.GlobalRotate(Vector3.Up, Mathf.DegToRad(rotation));
		AddChild(roomModel);
		for (int i = 0; i < dungeon.GetLength(0); i++)
		{
			for (int j = 0; j < dungeon.GetLength(1); j++)
			{
				GD.Print(dungeon[i, j]);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}