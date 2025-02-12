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

}
