using Godot;

namespace Utils.Motion
{
    public class MotionControl
    {
        private float _maxJumpHeight = 3.0f;
        private float _jumpAcceleration = 9.8f;
        private float _decelerationFactor = 1.0f;  // 上升减速的强度
        private float _fallingAcceleration = 20.0f;

        private float _moveAcceleration = 10.0f;
        private float _maxMoveAcceleration = 20.0f;

        private float _gravity = 9.8f;
        private float _inititalJumpVelocity = 0.0f;
        private float _inititalMoveVelocity = 0.0f;
        private float _currentVelocity = 0.0f;
        private float _currentHeight = 0.0f;
        private float _timeElapsed = 0.0f;
        private Vector3 _targetVelocity = Vector3.Zero;

        public MotionControl()
        {
            //h=2gv0
            _inititalJumpVelocity = Mathf.Sqrt(2 * _gravity * _maxJumpHeight);
        }
        public void Move(double delta)
        {
            var moveDirection = Vector3.Zero;

            if (Input.IsActionPressed("w"))
            {
                moveDirection.Z -= 1.0f;
            }
            if (Input.IsActionPressed("a"))//forward
            {
                moveDirection.X -= 1.0f;
            }
            if (Input.IsActionPressed("d"))
            {
                moveDirection.X += 1.0f;
            }
            if (Input.IsActionPressed("s"))
            {
                moveDirection.Z += 1.0f;
            }

            if (moveDirection != Vector3.Zero)
            {
                moveDirection = moveDirection.Normalized();
                // Model.Basis.LookingAt(moveDirection);
                // CollisionShape.Basis.LookingAt(moveDirection);
            }

            _targetVelocity.X = moveDirection.X * MoveSpeed;
            _targetVelocity.Z = moveDirection.Z * MoveSpeed;
        }
        public Vector3 Direction()
        {

        }
        public void Jump(double delta)
        {
            if (IsOnFloor() && Input.IsActionJustPressed("jump"))
            {
                _timeElapsed += (float)delta;
                _currentVelocity = _initialJumpVelocity * Mathf.Exp(-_decelerationFactor * _timeElapsed);
                _currentHeight = _initialJumpVelocity * _timeElapsed - 0.5f * _jumpAcceleration * Math.Pow(_timeElapsed, 2) * Mathf.Exp(-decelerationFactor * timeElapsed);
                if (_currentHeight <= 0 && _currentHeight >= _maxJumpHeight)
                {
                    _timeElapsed = 0.0f;
                }
                _targetVelocity.Y = _currentHeight;
            }
        }
        public void Fall(float delta)
        {
            timeElapsed += (float)delta;

            // 加速下落：重力作用
            _currentVelocity += _gravity * (float)delta;  // 加速落地，受重力影响

            // 计算下落高度
            _currentHeight -= 0.5f * _fallAcceleration * Math.Pow(_timeElapsed, 2);

            // 如果物体落地，停止下落
            if (_currentHeight <= 0)
            {
                _currentHeight = 0;
                _timeElapsed = 0.0f;
                isFalling = false;
            }

            // 更新物体位置
            Vector3 velocity = Velocity;
            velocity.y = currentHeight;
        }


    }

}


