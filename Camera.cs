using Godot;
using System;
using CameraControl;
public partial class Camera : Camera3D
{
    [Export] public RayCast3D RayCast { get; set; }

    private CameraDistanceControal Distance;

    public override void _Ready()
    {
        Distance = new CameraDistanceControal(this, 10, RayCast)
        {
            Speed = 1
        };
    }
    public override void _Process(double delta)
    {
        Distance.Process(delta);
    }
    public override void _Input(InputEvent @event)
    {
        if (inputEvent is InputEventKey keyEvent)
        {
            if (keyEvent.Scancode == (int)KeyList.Up && keyEvent.Pressed)
            {
                Distance.TargetDistance -= 5;
            }
            else if (keyEvent.Scancode == (int)KeyList.Down && keyEvent.Pressed)
            {
                Distance.TargetDistance += 5;
            }
        }
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
                Distance.TargetDistance -= mouseEvent.Factor;
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
                Distance.TargetDistance += mouseEvent.Factor;
        }

    }
}
