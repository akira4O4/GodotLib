using Godot;
using System;
using Utils.Camera;
public partial class ModelPivot : Marker3D
{
    private CameraAngleControl Angle;

    public override void _Ready()
    {
        Angle = new CameraAngleControl(this, 0, 0, 0)
        {
            PitchRotationSpeed = 20,
            YawRotationSpeed = 10
        };
        Input.SetMouseMode(Input.MouseModeEnum.Captured);
    }
    public override void _Process(double delta)
    {
        Angle.ProcessPitch(delta);
        Angle.ProcessYaw(delta);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventButton button)
        {
            if (button.ButtonIndex == (int)ButtonList.Left) // 判断是否是左键
            {
                if (button.Pressed) // 判断左键是否按下
                    isLeftMousePressed = true;
                else
                    isLeftMousePressed = false;
            }
        }
        if (@event is InputEventMouseMotion mouseMotion && isLeftMousePressed)
        {
                Angle.SetMouseOffset(mouseMotion.Relative);
        }
    }
}
