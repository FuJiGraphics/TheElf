using fz;
using UnityEngine;

public class TouchZoom : IDeviceContext
{
    public bool Active { get; private set; } = false;
    public float Scale { get; private set; } = 1.0f;
    public float PressedTime { get; set; } = 0.0f;
    public float MinScale { get; set; } = 0.5f;
    public float MaxScale { get; set; } = 2.0f;
    public float ScaleFactor { get; set; } = 1.0f;
    public TouchDevice Device { get; private set; } = TouchDevice.Touch;

    private ITouchData data0;
    private ITouchData data1;

    public TouchZoom()
    {
        data0 = new TouchData(0);
        data1 = new TouchData(1);
    }

    public void OnUpdate()
    {
        if (Input.touchCount != 2)
        {
            this.Clear();
            return;
        }

        data0.OnUpdate();
        data1.OnUpdate();

        float prevDistance = (data0.Start - data1.Start).magnitude;
        float currDistance = (data0.End - data1.End).magnitude;

        float delta = (currDistance - prevDistance) / Screen.dpi;
        delta *= ScaleFactor;

        Scale = Mathf.Clamp(Scale + delta, MinScale, MaxScale);
        Active = true;
    }

    public void SetDevice(TouchDevice device)
    {
        switch (device)
        {
            case TouchDevice.Unknown:
            case TouchDevice.Touch:
            case TouchDevice.Mouse:
            case TouchDevice.Keyboard:
                break;
        }
    }

    private void Clear()
    {
        Active = false;
        Scale = 1.0f;
        PressedTime = 0.0f;
    }

} // class TouchHeld