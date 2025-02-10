using Godot;
using System;
using CameraControl;
public partial class CameraPivot : Marker3D
{
	// Camera args
	[Export] public Camera3D Camera { get; set; }
	private CameraDistanceControal CamDistance;
	private CameraAngleControl CamAngle;
	public override void _Ready()
	{
		CamDistance = new CameraDistanceControal(Camera, 10)
		{
			Speed = 1
		};
		CamAngle = new CameraAngleControl(this, 0, 0, 0)
		{
			PitchRotationSpeed = 10,
			YawRotationSpeed = 10
		};
		 Input.SetMouseMode(Input.MouseModeEnum.Captured);
	}
	public override void _Process(double delta)
	{
		CamAngle.UpdatePitch((float)delta);
		CamAngle.UpdateYaw((float)delta);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			//Wheel Control
			if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
				CamDistance.AddDistance(-mouseEvent.Factor);
			else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
				CamDistance.AddDistance(mouseEvent.Factor);
		}
		if (@event is InputEventMouseMotion mouseMotion)
		{
			CamAngle.MouseOffsetX = mouseMotion.Relative.X;
			CamAngle.MouseOffsetY = mouseMotion.Relative.Y;
		}
	}
}
