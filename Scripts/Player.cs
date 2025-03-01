using Godot;
using GodotLib;
using GodotLib.Utils;
using System;
using System.Linq;

public partial class Player : CharacterBody3D
{
    [Export] public Camera3D Camera { get; set; }//Control camera distance
    [Export] public Marker3D CameraPivot { get; set; } //Control camera rotation
    [Export] public Marker3D ModelPivot { get; set; } //Control model rotation
    [Export] public CollisionShape3D BodyCollider { get; set; }//Control collisiton rotation
    [Export] public RayCast3D Player2CameraRay { get; set; } // Player-->Camera

    // [Export] public RayCast3D CameraRay1 { get; set; } // Camera-->
    // [Export] public RayCast3D CameraRay2 { get; set; } // Camera-->
    // [Export] public RayCast3D CameraRay3 { get; set; } // Camera-->
    // [Export] public RayCast3D CameraRay4 { get; set; } // Camera-->
    // [Export] public RayCast3D CameraRay5 { get; set; } // Camera-->
    // [Export] public RayCast3D CameraRay6 { get; set; } // Camera-->
    // [Export] public RayCast3D CameraRay7 { get; set; } // Camera-->

    [Export] public CollisionShape3D CameraCollider { get; set; }//Control collisiton rotation

    public CameraControl CameraControl;
    private Vector3 _targetVelocity = Vector3.Zero;
    private float _moveSpeed = 20;
    private float _defaultCameraDistance = 6;
    private float _defaultRunningCameraDistance = 10;
    private bool _cameraIsSafe = true;
    private float _safeDistance = 0;

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
        Player2CameraRay.Enabled = true;
        Player2CameraRay.TargetPosition = Camera.Position;
        // _safeDistance = CameraRay1.TargetPosition.Length();
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
            Utils.SetNodeRotationDegrees(BodyCollider, camDegrees, ignoreX: true);
        }
        if (Input.IsActionPressed("s"))
        {
            moveDirection = CameraControl.GetCameraBack();
            camDegrees.Y += 180.0f;
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(BodyCollider, camDegrees, ignoreX: true);
        }
        if (Input.IsActionPressed("a"))
        {
            moveDirection = CameraControl.GetCameraLeft();
            camDegrees.Y += 90.0f;
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(BodyCollider, camDegrees, ignoreX: true);

        }
        if (Input.IsActionPressed("d"))
        {
            moveDirection = CameraControl.GetCameraRight();
            camDegrees.Y -= 90.0f;
            Utils.SetNodeRotationDegrees(ModelPivot, camDegrees, ignoreX: true);
            Utils.SetNodeRotationDegrees(BodyCollider, camDegrees, ignoreX: true);
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

        // foreach (RayCast3D ray in Camera.GetChildren().OfType<RayCast3D>())
        // {
        //     if (ray.IsColliding())
        //     {
        //         _cameraIsSafe = false;
        //     }
        // }

        if (Player2CameraRay.IsColliding())
        {
            Vector3 colliderPoint = Player2CameraRay.GetCollisionPoint();
            Vector3 colliderNormal = Player2CameraRay.GetCollisionNormal();
            var colliderObject = Player2CameraRay.GetCollider();

            if (colliderObject is Area3D area)
            {
                if (area.Name != "CameraArea3D")
                {
                    _cameraIsSafe = false;
                    CameraControl.DistanceZoomSpeed = 8;
                    CameraControl.Distance = Player2CameraRay.ToLocal(colliderPoint).Length() - 0.5f;
                }
            }
            else
            {
                _cameraIsSafe = false;
                CameraControl.DistanceZoomSpeed = 8;
                CameraControl.Distance = Player2CameraRay.ToLocal(colliderPoint).Length() - 0.5f;

            }
        }
        if (_cameraIsSafe && CameraControl.Distance != _defaultCameraDistance)
        {
            CameraControl.DistanceZoomSpeed = 2;
            CameraControl.Distance = _defaultCameraDistance;
        }
    }

    private void onCameraCollisonBodyEntered(Node body)
    {
        GD.Print("Camera is not safe");
        if (body.Name != "CharacterBody3D")
        {
            _cameraIsSafe = false;
        }
    }
    private void onCameraCollisionBodyExited(Node body)
    {
        GD.Print("Camera is safe");
        _cameraIsSafe = true;
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