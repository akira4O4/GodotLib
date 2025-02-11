using Godot;
using System;
namespace Move
{
    public class MoveControl
    {
        private float _mass = 50f;//50kg
        private float _moveSpeed = 5f;//m/s
        private float _jumpSpeed = 10f;//m/s
        private float _gravity = 9.8f;//m/s^2
        public MoveControl()
        {
        }
        public float JumpHeight()
        {
            return (Math.Pow(_jumpSpeed, 2) / (2 * _mass * _gravity));
        }
        public float Jump(double delta)
        {
            float height = JumpHeight(); 

            // 根据时间和初始速度，使用公式 s = vt - (1/2)gt^2 计算当前位置
            return _jumpSpeed * (float)delta - 0.5f * _gravity * Math.Pow((float)delta,2);
        }
    }
}
