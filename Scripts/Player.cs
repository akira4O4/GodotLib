using System;
using System.Security.Cryptography.X509Certificates;
using Godot;
using Utils.Camera;
public partial class Player : CharacterBody3D
{
    [Export] public Marker3D CameraPivot { get; set; }
    [Export] public Marker3D ModelPivot { get; set; }
    [Export] public Node3D CollisionShape { get; set; }
    private float _moveSpeed = 14.0f;
    private float _jumpSpeed = 14.0f;
    public float Mass { get; set; } = 50.0f;
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => Math.Max(0, value);
    }
    public float JumpSpeed
    {
        get => _moveSpeed;
        set => Math.Max(0, value);
    }
    public float Gravity { get; set; } = 9.8f;
    public float FallAcceleration { get; set; } = 75.0f;
    private CameraAngleControl Angle;
    private Vector3 _targetVelocity = Vector3.Zero;
    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
    }
    public override void _Process(double delta)
    {
        move(delta);
        jump(delta);
        MoveAndSlide();

    }
    private void move(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("w"))
        {
            direction.Z -= 1.0f;
            var cameraDirection = CameraPivot.Basis.Z.Normalized();
            var playerDirection = Basis.Z.Normalized();
            float angle = Mathf.RadToDeg(playerDirection.AngleTo(cameraDirection.Normalized()));
            GD.Print(angle);
            if (angle > 0f)
            {
            }
        }
        if (Input.IsActionPressed("a"))//forward
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("d"))
        {

            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("s"))
        {
            direction.Z += 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            ModelPivot.Basis = Basis.LookingAt(direction);
            CollisionShape.Basis = Basis.LookingAt(direction);
        }

        _targetVelocity.X = direction.X * MoveSpeed;
        _targetVelocity.Z = direction.Z * MoveSpeed;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        Velocity = _targetVelocity;

    }
    private void jump(double delta)
    {
    }
    public override void _Input(InputEvent @event)
    {
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
        //     Angle.SetMouseOffset(mouseMotion.Relative);
        // }
    }

}