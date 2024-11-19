using Godot;
using System;

public partial class player : CharacterBody3D
{
	private Node3D Head;
	private Node3D CamRot;
	private Camera3D camera;
	private float acceleration = 0.5f;
	private float gravity = 30f;
	private float speed  = 5f;
	private bool isInVR = false;
	private Node3D testCamera;
	private Camera3D securityCamera;
	public CharacterBody3D playerCB3D;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		CamRot = GetNode<Node3D>("Head").GetNode<Node3D>("CamRot");
		camera = CamRot.GetNode<Camera3D>("Camera3D");
		testCamera = GetNode<Node3D>("../testCamera");
		securityCamera = GetNode<Camera3D>("../Camera3D");
		// playerCB3D = this.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseMotion eventMouseMotion)
		{
			RotateY(-eventMouseMotion.Relative.X * (float)(Math.PI / 180));
			camera.RotateX(-eventMouseMotion.Relative.Y * (float)(Math.PI / 180));
			Vector3 camRot = camera.Rotation;
			camRot.X = Mathf.Clamp(camRot.X, Mathf.DegToRad(-80), Mathf.DegToRad(80));
			camera.Rotation = camRot;
		}	
		if(@event.IsActionPressed("ToggleHeadset"))
		{
			isInVR = !isInVR;
			if(isInVR){
				securityCamera.MakeCurrent();
				securityCamera.GlobalPosition = testCamera.GlobalPosition;
			}			
			else
			{
				camera.MakeCurrent();
			}

		}
	}

	private Vector3 velocity = Vector3.Zero;
    public override void _PhysicsProcess(double delta)
    {
        Vector2 moveInput = Input.GetVector("left","right","forward","backward");
		Vector3 moveVector = (Transform.Basis * new Vector3(moveInput.X, 0, moveInput.Y)).Normalized();
		if(moveVector != Vector3.Zero)
		{
			velocity.X  = -moveVector.X * speed;
			float backSpeed = speed/2;
			velocity.Z = -moveVector.Z * speed;
			if(-moveVector.Z < 0)
			{
				velocity.Z = -moveVector.Z * backSpeed;
			}
			// camera.TranslateObjectLocal(new Vector3(0.05f,0,0));
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X,0,acceleration);
			velocity.Z = Mathf.MoveToward(velocity.Z,0,acceleration);
		}
		if(!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;
		}
		Velocity = velocity;
		MoveAndSlide();
    }
}
