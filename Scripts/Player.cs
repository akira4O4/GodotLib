using Godot;

public partial class Player : CharacterBody3D
{
    public float Mass { get; set; } = 50.0f;
    public float MoveSpeed { get; set; } = 14.0f;
    public float JumpSpeed { get; set; } = 14.0f;
    public float Gravity { get; set; } = 9.8f;
    public float FallAcceleration { get; set; } = 75.0f;

    private Vector3 _targetVelocity = Vector3.Zero;
    public override void _Ready()
    {
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }
    private void Move(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("a"))
        {
            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("d"))
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("s"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("w"))
        {
            direction.Z -= 1.0f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        // Ground velocity
        _targetVelocity.X = direction.X * MoveSpeed;
        _targetVelocity.Z = direction.Z * MoveSpeed;

        // Vertical velocity
        if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        // Moving the character
        Velocity = _targetVelocity;

    }
    private void Jump(double delta)
    {

    }
    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
        Jump(delta);
        MoveAndSlide();
    }

    public override void _Process(double delta)
    {

    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            CamAngle.SetMouseOffset(mouseMotion.Relative);
        }
    }
}