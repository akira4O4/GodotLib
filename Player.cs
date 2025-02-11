using Godot;
using System;

public class MyCharacter : CharacterBody3D
{
    // 角色的移动速度和跳跃力
    public float MoveSpeed = 5f;
    public float JumpSpeed = 10f;
    public float Gravity = -9.8f;

    // 移动方向向量
    private Vector3 _velocity = Vector3.Zero;

    // 输入处理
    private Vector2 _inputDirection;

    public override void _Ready()
    {
        // 在这里你可以初始化角色，或者进行其他设置
        GD.Print("Character is ready!");
    }

    public override void _PhysicsProcess(float delta)
    {
        // 获取用户输入
        _inputDirection = new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
        );

        // 移动控制
        MoveCharacter(delta);

        // 处理重力
        ApplyGravity(delta);

        // 进行跳跃
        HandleJump(delta);

        // 最终移动角色
        MoveAndSlide(_velocity, Vector3.Up);
    }

    private void MoveCharacter(float delta)
    {
        // 计算角色的移动方向
        Vector3 direction = new Vector3(_inputDirection.x, 0, _inputDirection.y).Normalized();

        // 更新速度（X和Z轴方向）
        _velocity.x = direction.x * MoveSpeed;
        _velocity.z = direction.z * MoveSpeed;
    }

    private void ApplyGravity(float delta)
    {
        // 角色的垂直速度受重力影响
        if (!IsOnFloor())
        {
            _velocity.y += Gravity * delta;
        }
        else
        {
            _velocity.y = 0;
        }
    }

    private void HandleJump(float delta)
    {
        // 检测是否按下跳跃键
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            // 角色跳跃
            _velocity.y = JumpSpeed;
        }
    }
}
