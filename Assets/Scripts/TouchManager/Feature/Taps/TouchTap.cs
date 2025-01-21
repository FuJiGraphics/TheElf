
using fz;
using UnityEngine;

public class TouchTap : IDeviceContext
{
    public bool Active { get; private set; }
    public Vector2 Start {  get; private set; }
    public Vector2 End { get; private set; }
    public float PressedTime { get; private set; }
    public float Distance { get; private set; }
    public float DurationLimit { get; set; }
    public TouchDevice Device { get; private set; }

    private ITouchData data;

    public TouchTap()
    {
        data = new TouchData(0);
        Device = TouchDevice.Touch;
        DurationLimit = 0.3f;
    }

    public void OnUpdate()
    {
        data.OnUpdate();
        if (Input.touchCount < 1)
        {
            this.Clear();
            return;
        }

        if (TouchState.Released == data.State && data.PressedTime < DurationLimit)
        {
            Active = true;
            Start = data.Start;
            End = data.End;
            PressedTime = data.PressedTime;
            Distance = data.Distance;
        }
        else if (TouchState.None == data.State)
        {
            this.Clear();
        }
    }

    public void SetDevice(TouchDevice device)
    {
        switch (device)
        {
            case TouchDevice.Unknown:
            case TouchDevice.Touch:
                data = new TouchData(0);
                break;
            case TouchDevice.Mouse:
            case TouchDevice.Keyboard:
                data = new MouseData(0);
                break;
        }
    }

    private void Clear()
    {
        Active = false;
        Start = Vector2.zero;
        End = Vector2.zero;
        PressedTime = 0.0f;
        Distance = 0.0f;
    }
} // TouchTap