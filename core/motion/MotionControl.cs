using Godot;
namespace GodotLib.Core.Motion
{
    public class PlayerMotionControl
    {

        private float _moveSpeed = 10.0f;
        private float _jumpSpeed = 10.0f;

        private float _maxMoveSpeed = 20.0f;
        private float _maxJumpSpeed = 20.0f;

        private float _moveAcceleration = 10.0f;
        private float _jumpAcceleration = 9.8f;

        private float _inititalJumpSpeed = 0.0f;
        private float _inititalMoveSpeed = 0.0f;

        private float _decelerationFactor = 1.0f;  // 上升减速的强度
        private float _fallingAcceleration = 20.0f;

        private float _maxJumpHeight = 3.0f;

        private float _gravity = 9.8f;

        private float _currentSpeed = 0.0f;
        private float _currentHeight = 0.0f;

        private float _timeElapsed = 0.0f;
        private Vector3 _targetVelocity = Vector3.Zero;

        public PlayerMotionControl()
        {
            //h=2gv0
            _inititalJumpSpeed = Mathf.Sqrt(2 * _gravity * _maxJumpHeight);
        }

        public Vector3 UniformMove()
        {
            var targetVelocity = Vector3.Zero;

            if (Input.IsActionPressed("w"))
            {
                targetVelocity.Z -= 1.0f;  // 向前
            }
            if (Input.IsActionPressed("s"))
            {
                targetVelocity.Z += 1.0f;  // 向后
            }

            if (Input.IsActionPressed("a"))
            {
                targetVelocity.X -= 1.0f;  // 向左
            }
            if (Input.IsActionPressed("d"))
            {
                targetVelocity.X += 1.0f;  // 向右
            }

            targetVelocity = targetVelocity.Normalized();

            targetVelocity.X *= _moveSpeed;
            targetVelocity.Z *= _moveSpeed;

            return targetVelocity;
        }

        public float UniformJump()
        {
            return _jumpSpeed;
        }
        public float UniformFall()
        {
            return _fallingAcceleration;
        }

        public float NonUniformMove(double delta)
        {
            return 0.0f;
        }
        public float NonUniformJump(double delta)
        {
            _timeElapsed += (float)delta;
            _currentSpeed = _inititalJumpSpeed * Mathf.Exp(-_decelerationFactor * _timeElapsed);
            _currentHeight = _inititalJumpSpeed * _timeElapsed - 0.5f * _jumpAcceleration * Mathf.Pow(_timeElapsed, 2) * Mathf.Exp(-_decelerationFactor * _timeElapsed);
            if (_currentHeight <= 0 && _currentHeight >= _maxJumpHeight)
            {
                _timeElapsed = 0.0f;
            }
            _targetVelocity.Y = _currentHeight;

            return 0.0f;
        }
        public void NonUniformFall(double delta)
        {
            _timeElapsed += (float)delta;

            // 加速下落：重力作用
            _currentSpeed += _gravity * (float)delta;  // 加速落地，受重力影响

            // 计算下落高度
            _currentHeight -= 0.5f * _fallingAcceleration * Mathf.Pow(_timeElapsed, 2);

            // 如果物体落地，停止下落
            if (_currentHeight <= 0)
            {
                _currentHeight = 0;
                _timeElapsed = 0.0f;
            }

            // 更新物体位置
            // Vector3 velocity = Velocity;
            // velocity.y = currentHeight;
        }


    }

}


