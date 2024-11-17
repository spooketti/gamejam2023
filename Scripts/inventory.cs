using Godot;
using System;
using System.Collections;

public partial class inventory : Node
{
	public Node viewModelNode;

	public override void _Ready()
	{
		viewModelNode = GetParent().GetNode<Node3D>("Head/CamRot/Camera3D/Node3D");
	}
	public override void _Input(InputEvent @event)
	{
		Node clonedModel;
		GD.Print(@event.AsText());
		
		switch (@event.AsText())
		{
			case "1":
				GD.Print("i dont wanna fall in love alone");
				clonedModel = GetChild(0).Duplicate();
				// viewModelNode.RemoveChild(viewModelNode.GetChild(0));
				viewModelNode.AddChild(clonedModel);
				break;

			case "2":
				clonedModel = GetChild(1).Duplicate();
				viewModelNode.RemoveChild(viewModelNode.GetChild(0));
				viewModelNode.AddChild(clonedModel);
				break;

			case "3":
				clonedModel = GetChild(2).Duplicate();
				viewModelNode.RemoveChild(viewModelNode.GetChild(0));
				viewModelNode.AddChild(clonedModel);
				break;
		}
	}
}
