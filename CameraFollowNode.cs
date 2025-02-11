using Godot;
using System;
using CameraControl;
public partial class CameraFollowNode : Marker3D
{
	private CameraAngleControl CamAngle;

	public override void _Ready()
	{
		CamAngle = new CameraAngleControl(this, 0, 0, 0)
		{
			PitchRotationSpeed = 10,
			YawRotationSpeed = 10
		};
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}
	public override void _Process(double delta)
	{
		CamAngle.ProcessPitch(delta);
		CamAngle.ProcessYaw(delta);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			CamAngle.SetMouseOffset(mouseMotion.Relative);
		}
	}
}
