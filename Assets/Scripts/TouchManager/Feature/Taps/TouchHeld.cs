using fz;
using UnityEngine;

public class TouchHeld : IDeviceContext
{
    public bool Active { get; private set; } = false;
    public Vector2 Position { get; private set; } = Vector2.zero;
    public float PressedTime { get; set; } = 0.0f;
    public TouchDevice Device { get; private set; } = TouchDevice.Touch;

    private ITouchData data;

    public TouchHeld()
    {
        // data = new TouchData(0);
        data = UtilManager.FindWithName("Joystick").GetComponent<VirtualJoystck>();
    }

    public void OnUpdate()
    {
        data.OnUpdate();
        if (Input.touchCount < 1)
        {
            this.Clear();
            return;
        }

        if (TouchState.Held == data.State)
        {
            Active = true;
            Position = data.End;
            PressedTime = data.PressedTime;
        }
        else
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
        Position = Vector2.zero;
        PressedTime = 0.0f;
    }

} // class TouchHeld