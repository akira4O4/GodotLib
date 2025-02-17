// using Godot;
// using GodotLib.Core.Camera;
// using GodotLib.Core.Motion;
// using GodtoLib.Core.Camera;

// public partial class Player : CharacterBody3D
// {
//     [Export] public Camera3D Camera { get; set; }//Control camera distance
//     [Export] public Marker3D CameraPivot { get; set; } //Control camera rotation
//     [Export] public RayCast3D RayCast { get; set; }
//     [Export] public Marker3D ModelPivot { get; set; } //Control model rotation
//     [Export] public CollisionShape3D Collider { get; set; }//Control collisiton rotation

//     private CameraAngleControl CameraAngle;
//     private CameraDistanceControl CameraDistance;
//     private PlayerMotionControl Motion;

//     public bool FreePerspective = false;
//     public bool ControlPerspective = false;

//     public override void _Ready()
//     {
//         CameraDistance = new CameraDistanceControl(Camera, 5, RayCast)
//         {
//             Speed = 5,
//             MinSpeed = 1,
//             MaxSpeed = 10,
//             MinDistance = 2,
//             MaxDistance = 10
//         };
//         CameraAngle = new CameraAngleControl(CameraPivot, 0, 0, 0)
//         {
//             PitchRotationSpeed = 20,
//             YawRotationSpeed = 10
//         };
//         Motion = new PlayerMotionControl();
//         Input.SetMouseMode(Input.MouseModeEnum.Captured);

//     }

//     private Vector3 _targetVelocity = Vector3.Zero;
//     private float _moveSpeed = 20;

//     public override void _PhysicsProcess(double delta)
//     {
//         //Control Camera
//         CameraAngle.ProcessPitch(delta);
//         CameraAngle.ProcessYaw(delta);
//         CameraDistance.ProcessDistance(delta);

//         Vector3 cameraForward = -CameraPivot.Basis.Z.Normalized();
//         Vector3 direction = Vector3.Zero;
//         if (Input.IsActionPressed("w"))
//         {
//             SyncNodeRotation(CameraPivot, ModelPivot, false, true, true);
//             SyncNodeRotation(CameraPivot, Collider, false, true, true);
//             direction=cameraForward;
//         }
//         if (Input.IsActionPressed("s"))
//         {
//             direction.Z += 1.0f;  // 向后
//         }
//         if (Input.IsActionPressed("a"))
//         {
//             direction.X -= 1.0f;  // 向左
//             TurnLeft(ModelPivot);
//         }
//         if (Input.IsActionPressed("d"))
//         {
//             TurnRight(ModelPivot);
//             direction.X += 1.0f;  // 向右
//         }

//         direction.Y = 0;
//         _targetVelocity = direction  * _moveSpeed;

//         //Juamp and fall
//         if (!IsOnFloor())
//         {
//             _targetVelocity.Y -= Motion.UniformFall() * (float)delta;
//         }
//         if (IsOnFloor() && Input.IsActionJustPressed("space"))
//         {
//             _targetVelocity.Y = Motion.UniformJump();
//         }

//         Velocity = _targetVelocity;
//         MoveAndSlide();
//         // GD.Print($"Camera Front: {CameraPivot.Basis.Z.Normalized()}");
//         // GD.Print($"Player Front: {ModelPivot.Basis.Z.Normalized()}\n");

//     }

//     //node -> target node
//     public void SyncNodeRotation(Node3D node, Node3D target, bool syncX = true, bool syncY = true, bool syncZ = true)
//     {
//         var nodeRotationDegrees = target.RotationDegrees;

//         if (syncX)
//             nodeRotationDegrees.X = node.RotationDegrees.X;
//         if (syncY)
//             nodeRotationDegrees.Y = node.RotationDegrees.Y;
//         if (syncZ)
//             nodeRotationDegrees.Z = node.RotationDegrees.Z;

//         target.RotationDegrees = nodeRotationDegrees;
//     }
//     public void TurnLeft(Node3D node)
//     {
//         var degrees = node.RotationDegrees;
//         degrees.Y = 90.0f;
//         node.RotationDegrees = degrees;

//     }
//     public void TurnRight(Node3D node)
//     {
//         var degrees = node.RotationDegrees;
//         degrees.Y = -90.0f;
//         node.RotationDegrees = degrees;
//     }
//     public void TurnBack(Node3D node)
//     {
//         var degrees = node.RotationDegrees;
//         degrees.Y = 180.0f;
//         node.RotationDegrees = degrees;
//     }

//     public override void _Input(InputEvent @event)
//     {
//         if (@event is InputEventMouseMotion mouseMotion)
//         {
//             CameraAngle.SetMouseOffset(mouseMotion.Relative);
//         }
//         if (@event is InputEventMouseButton mouseEvent)
//         {
//             if (mouseEvent.ButtonIndex == MouseButton.WheelUp)
//             {
//                 CameraDistance.TargetDistance -= mouseEvent.Factor;

//             }
//             else if (mouseEvent.ButtonIndex == MouseButton.WheelDown)
//             {
//                 CameraDistance.TargetDistance += mouseEvent.Factor;
//             }
//         }

//         // 检查是否是鼠标按键事件
//         if (@event is InputEventMouseButton button && button.ButtonIndex == MouseButton.Left)
//         {
//             if (button.Pressed) // 判断左键按下
//             {
//                 GD.Print("Left Mouse Button Pressed");
//             }
//             else // 判断左键松开
//             {
//                 // isLeftButtonPressed = false;
//                 GD.Print("Left Mouse Button Released");
//             }
//         }

//         // // 检查鼠标移动事件，只有在左键按下时才处理
//         // if (@event is InputEventMouseMotion mouseMotion && isLeftButtonPressed)
//         // {
//         //     GD.Print("Move Mouse with Left Button Pressed");
//         //     CameraAngle.SetMouseOffset(mouseMotion.Relative);
//         // }
//     }

// }