using Godot;
using Utils.Camera;
public partial class Camera : Camera3D
{
    [Export] public RayCast3D RayCast { get; set; }
    private CameraDistanceControl Distance;
    public override void _Ready()
    {
        Distance = new CameraDistanceControl(this, 3, RayCast)
        {
            Speed = 10,
            MinSpeed = 1,
            MaxSpeed = 10,
            MinDistance = 3,
            MaxDistance = 20
        };

    }
    public override void _Process(double delta)
    {
        Distance.Process(delta);

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                Distance.TargetDistance -= mouseEvent.Factor;

            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                Distance.TargetDistance += mouseEvent.Factor;
            }
        }

    }
}
