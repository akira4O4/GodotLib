using System;
using System.Security.Cryptography.X509Certificates;
using Godot;
using Utils.Camera;
using Utils.Motion;

public partial class Player : CharacterBody3D
{
    [Export] public Camera3D Camera { get; set; }//Control camera distance
    [Export] public Marker3D CameraPivot { get; set; } //Control camera rotation
    [Export] public RayCast3D RayCast { get; set; }
    [Export] public Node3D Model { get; set; } //Control model rotation
    [Export] public CollisionShape3D CollisionShape { get; set; }//Control collisiton rotation

    private CameraAngleControl CameraAngle;
    private CameraDistanceControl CameraDistance;
    private MotionControl Motion;
    public override void _Ready()
    {
        CameraDistance = new CameraDistanceControl(Camera, 3, RayCast)
        {
            Speed = 10,
            MinSpeed = 1,
            MaxSpeed = 10,
            MinDistance = 3,
            MaxDistance = 20
        };
        CameraAngle = new CameraAngleControl(CameraPivot, 0, 0, 0)
        {
            PitchRotationSpeed = 20,
            YawRotationSpeed = 10
        };
        Motion = new MotionControl();
    }
    public override void _PhysicsProcess(double delta)
    {
    }
    public override void _Process(double delta)
    {
        //Control Camera
        CameraDistance.Process(delta);
        CameraAngle.ProcessPitch(delta);
        CameraAngle.ProcessYaw(delta);
        //Motion Control 
        Motion.Move(delta);
        Motion.Jump(delta);
        Motion.Fall(delta);
        Velocity = _targetVelocity
        MoveAndSlide();

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            CameraAngle.SetMouseOffset(mouseMotion.Relative);
        }
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            {
                CameraDistance.TargetDistance -= mouseEvent.Factor;

            }
            else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            {
                CameraDistance.TargetDistance += mouseEvent.Factor;
            }
        }

        // 检查是否是鼠标按键事件
        if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left)
        {
            if (button.Pressed) // 判断左键按下
            {
                GD.Print("Left Mouse Button Pressed");
            }
            else // 判断左键松开
            {
                // isLeftButtonPressed = false;
                GD.Print("Left Mouse Button Released");
            }
        }

        // // 检查鼠标移动事件，只有在左键按下时才处理
        // if (@event is InputEventMouseMotion mouseMotion && isLeftButtonPressed)
        // {
        //     GD.Print("Move Mouse with Left Button Pressed");
        //     CameraAngle.SetMouseOffset(mouseMotion.Relative);
        // }
    }

}