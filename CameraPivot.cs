using Godot;
using System;
using Cam;
public partial class CameraPivot : Marker3D
{
	// Camera args
	[Export] public Camera3D Camera { get; set; }
	private CameraDistanceControal CamDistance;
	private CameraAngleControl CamAngle;
	public override void _Ready()
	{
		CamDistance = new CameraDistanceControal(Camera, 20, 1);
		CamAngle=new CameraAngleControl(Camera,0,0,0,1);
	}
	public override void _Process(double delta)
	{

	}
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			// if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			// {
			// 	IsFree = IsMouseControl = true;
			// }
			// else if (mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
			// {
			// 	IsFree = false;
			// 	IsMouseControl = !IsFree;
			// }
			// else
			// {
			// 	IsFree = IsMouseControl = false;
			// }
			if (mouseEvent.Factor != 0) // 只处理有效滚动事件，过滤无效事件
			{
				if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
				{
					GD.Print($"Wheel Up: {mouseEvent.Factor}");
					CamDistance.AddDistance(-mouseEvent.Factor);
				}

				if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
				{
					GD.Print($"Wheel Down: {mouseEvent.Factor}");
					CamDistance.AddDistance(mouseEvent.Factor);
				}
			}
		}
		// if (@event is InputEventMouseMotion mouseMotion)
		// {
		// 	MouseOffset = NormalAxis(mouseMotion.Relative);
		// }
		// else
		// {
		// 	MouseOffset = Vector2.Zero;
		// }
	}
}
