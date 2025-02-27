using Godot;
using GodotLib;
using GodotLib.Utils;
public partial class Player : CharacterBody3D
{
    [Export] public Camera3D Camera { get; set; }//Control camera distance
    [Export] public Marker3D CameraPivot { get; set; } //Control camera rotation
    [Export] public Marker3D ModelPivot { get; set; } //Control model rotation
    [Export] public CollisionShape3D Collider { get; set; }//Control collisiton rotation
    [Export] public RayCast3D CameraRaycasting { get; set; }

    public CameraControl CameraControl;
    public bool FreePerspective = false;
    public bool ControlPerspective = false;
    private Vector3 _targetVelocity = Vector3.Zero;
    private float _moveSpeed = 20;
    //Camera Args

    private float _defaultCameraDistance = 6;
    private float _defaultRunningCameraDistance = 10;
    private bool _cameraIsBlocked = false;

    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Captured);

        initCameraControl();
        initCameraRaycast();
    }
    private void initCameraControl()
    {
        CameraControl = new CameraControl(Camera, CameraPivot, _defaultCameraDistance)
        {
            Pitch = -30,
            PitchRotationSpeed = 5,
            YawRotationSpeed = 10

        };
    }
    private void initCameraRaycast()
    {
        CameraRaycasting.Enabled = true;
        CameraRaycasting.TargetPosition = Camera.Position;
    }

    public override void _PhysicsProcess(double delta)
    {

        CameraControl.Process(delta);
        cameraRaycastProcess();
        playerMotionProcess(delta);
    }
    private void playerMotionProcess(double delta)
    {
        //1、Sync player and camera direction
        //2、Sync model,collider rotation to move direction
        Vector3 moveDirection = Vector3.Zero;
        Vector3 camDegrees = Camera.GlobalRotationDegrees;

        //Move Player
        if (Input.IsActionPressed("w"))
        {
            moveDirection = CameraControl.GetCameraForward();
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(Collider, camDegrees, ignoreX: true);
        }
        if (Input.IsActionPressed("s"))
        {
            moveDirection = CameraControl.GetCameraBack();
            camDegrees.Y += 180.0f;
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(Collider, camDegrees, ignoreX: true);
        }
        if (Input.IsActionPressed("a"))
        {
            moveDirection = CameraControl.GetCameraLeft();
            camDegrees.Y += 90.0f;
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(Collider, camDegrees, ignoreX: true);

        }
        if (Input.IsActionPressed("d"))
        {
            moveDirection = CameraControl.GetCameraRight();
            camDegrees.Y -= 90.0f;
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(Collider, camDegrees, ignoreX: true);
        }

        // moveDirection.Y = 0;
        _targetVelocity.X = moveDirection.X * _moveSpeed;
        _targetVelocity.Z = moveDirection.Z * _moveSpeed;

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
    private void cameraRaycastProcess()
    {
        CameraRaycasting.TargetPosition = Camera.Position;
        if (CameraRaycasting.IsColliding())
        {
            // GD.Print("CameraRaycasting.IsColliding");
            Vector3 colliderPoint = CameraRaycasting.GetCollisionPoint();
            Vector3 colliderNormal = CameraRaycasting.GetCollisionNormal();
            var colliderObject = CameraRaycasting.GetCollider();
            CameraControl.Distance = CameraRaycasting.ToLocal(colliderPoint).Z + 0.1f;
        }
        else
        {
            CameraControl.Distance = _defaultCameraDistance;
        }
    }
    public void AimObject()
    {
    }
    public void LockObject()
    {
    }
    public void UnLockObject()
    {
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            CameraControl.UpdateMouseOffset(mouseMotion.Relative);
        }
        if (@event is InputEventMouseButton mouseEvent)
        {
            // if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
            // {

            // }
            // else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
            // {
            // }
        }

        // 检查是否是鼠标按键事件
        if (@event is InputEventMouseButton button)
        {
            if (button.ButtonIndex == MouseButton.Left)
            {
                if (button.Pressed)
                {
                    GD.Print("Left Mouse Button Pressed");
                    CameraControl.CameraXOffset = 3;
                }
                else
                {
                    GD.Print("Left Mouse Button Released");
                    CameraControl.CameraXOffset = 0;
                }
            }
            else if (button.ButtonIndex == MouseButton.Right)
            {
                if (button.Pressed)
                {
                    GD.Print("Right Mouse Button Pressed");
                    CameraControl.CameraXOffset = -3;
                }
                else
                {
                    GD.Print("Right Mouse Button Released");
                    CameraControl.CameraXOffset = 0;
                }
            }
        }
    }
    public override void _Input(InputEvent @event)
    {

    }
}