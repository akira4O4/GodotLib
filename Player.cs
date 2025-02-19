using Godot;
using GodotLib.Core.Camera;
using GodotLib.Core.Motion;
using GodtoLib.Core.Camera;
using GodotLib.Utils;
public partial class Player : CharacterBody3D
{
    [Export] public Camera3D Camera { get; set; }//Control camera distance
    [Export] public Marker3D CameraPivot { get; set; } //Control camera rotation
    [Export] public RayCast3D RayCast { get; set; }
    [Export] public Marker3D ModelPivot { get; set; } //Control model rotation
    [Export] public CollisionShape3D Collider { get; set; }//Control collisiton rotation

    private CameraAngleControl CameraAngle;
    private CameraDistanceControl CameraDistance;
    private PlayerMotionControl Motion;
    private CameraUtils _cameraUtils;

    public bool FreePerspective = false;
    public bool ControlPerspective = false;

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

        Input.SetMouseMode(Input.MouseModeEnum.Captured);
        _cameraUtils = new CameraUtils(Camera);

    }

    private Vector3 _targetVelocity = Vector3.Zero;
    private float _moveSpeed = 20;

    public override void _PhysicsProcess(double delta)
    {
        //Control Camera
        CameraAngle.ProcessPitch(delta);
        CameraAngle.ProcessYaw(delta);
        CameraDistance.ProcessDistance(delta);

        //1、Sync player and camera direction
        //2、Sync model,collider rotation to move direction

        Vector3 moveDirection = Vector3.Zero;
        Vector3 camDegrees = Camera.GlobalRotationDegrees;

        if (Input.IsActionPressed("w"))
        {
            moveDirection = _cameraUtils.GetForward();
            Utils.ChangeNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.ChangeNodeRotationDegrees(Collider, camDegrees, ignoreX: true);
        }
        if (Input.IsActionPressed("s"))
        {
            moveDirection = _cameraUtils.GetBack();
            camDegrees.Y += 180.0f;
            Utils.ChangeNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.ChangeNodeRotationDegrees(Collider, camDegrees, ignoreX: true);
        }
        if (Input.IsActionPressed("a"))
        {
            moveDirection = _cameraUtils.GetLeft();
            camDegrees.Y += 90.0f;
            Utils.ChangeNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.ChangeNodeRotationDegrees(Collider, camDegrees, ignoreX: true);

        }
        if (Input.IsActionPressed("d"))
        {
            moveDirection = _cameraUtils.GetRight();
            camDegrees.Y -= 90.0f;
            Utils.ChangeNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.ChangeNodeRotationDegrees(Collider, camDegrees, ignoreX: true);
        }

        moveDirection.Y = 0;
        _targetVelocity = moveDirection * _moveSpeed;

        //Juamp and fall
        if (!IsOnFloor())
        {
            _targetVelocity.Y -= 15.0f * (float)delta;
        }
        if (IsOnFloor() && Input.IsActionJustPressed("space"))
        {
            _targetVelocity.Y = 15.0f;
        }

        Velocity = _targetVelocity;
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