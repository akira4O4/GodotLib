using System;
using Godot;
using Godotlib.Camera;
using Godotlib.Motion;

public partial class Player : CharacterBody3D
{
    [Export] public Camera3D Camera { get; set; }//Control camera distance
    [Export] public Marker3D CameraPivot { get; set; } //Control camera rotation
    [Export] public RayCast3D RayCast { get; set; }
    [Export] public Marker3D ModelPivot { get; set; } //Control model rotation
    [Export] public CollisionShape3D Collider { get; set; }//Control collisiton rotation

    private CameraAngleControl CameraAngle;
    private CameraDistanceControl CameraDistance;
    private MotionControl Motion;
    public override void _Ready()
    {
        CameraDistance = new CameraDistanceControl(Camera, 5, RayCast)
        {
            Speed = 5,
            MinSpeed = 1,
            MaxSpeed = 10,
            MinDistance = 2,
            MaxDistance = 10
        };
        CameraAngle = new CameraAngleControl(CameraPivot, 0, 0, 0)
        {
            PitchRotationSpeed = 20,
            YawRotationSpeed = 10
        };
        Motion = new MotionControl();
        Input.SetMouseMode(Input.MouseModeEnum.Captured);

    }
    private Vector3 _targetVelocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        //Control Camera
        CameraAngle.ProcessPitch(delta);
        CameraAngle.ProcessYaw(delta);
        CameraDistance.ProcessDistance(delta);

        //Motion Control 
        var newVelocity = Motion.UniformMove();//only have  X and Z
        _targetVelocity.X=newVelocity.X; 
        _targetVelocity.Z=newVelocity.Z; 

        if (newVelocity != Vector3.Zero)
        {
            ModelPivot.Basis = Basis.LookingAt(newVelocity.Normalized());
            Collider.Basis = Basis.LookingAt(newVelocity.Normalized());
        }

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= Motion.UniformFall() * (float)delta;
        }

        // Jumping.
        if (IsOnFloor() && Input.IsActionJustPressed("space"))
        {
            _targetVelocity.Y = Motion.UniformJump();
        }
        Velocity = _targetVelocity;
        MoveAndSlide();
    }
    private void changeCameraDistance()
    {
        CameraDistance.TargetDistance=5;
        CameraDistance.Speed=10;
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