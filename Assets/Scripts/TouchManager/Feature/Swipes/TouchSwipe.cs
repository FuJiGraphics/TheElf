using fz;
using UnityEngine;

public class TouchSwipe : IDeviceContext
{
    public bool Active { get; private set; } = false;
    public Vector2 Start { get; private set; } = Vector2.zero;
    public Vector2 End { get; private set; } = Vector2.zero;
    public float PressedTime { get; private set; } = 0.0f;
    public float Distance { get; private set; } = 0.0f;
    public float MinSwipeDistance { get; set; } = 0.25f;
    public float DurationLimit { get; set; } = 0.3f;
    public float AngleLimit { get; set; } = 15.0f;
    public Vector2 Direction { get; private set; } = Vector2.up;
    public TouchDevice Device { get; private set; } = TouchDevice.Touch;

    private ITouchData data;
    private float minSwipeDistancePixels = 0.0f;

    public TouchSwipe(Vector2 direction)
    {
        data = new TouchData(0);
        Device = TouchDevice.Touch;
        Direction = direction;

        minSwipeDistancePixels = MinSwipeDistance * Screen.dpi;
    }

    public void OnUpdate()
    {
        data.OnUpdate();

        if (TouchState.Released == data.State && PressedTime < DurationLimit)
        {
            Vector2 dir = End - Start;
            Distance = dir.magnitude;
            if (Mathf.Abs(Distance) > minSwipeDistancePixels)
            {
                float angle = Vector2.SignedAngle(Direction, dir.normalized);
                if (Mathf.Abs(angle) <= AngleLimit)
                {
                    Active = true;
                }
            }
        }
        else if (TouchState.Held == data.State)
        {
            PressedTime += Time.deltaTime;
            End = data.End;
        }
        else if (TouchState.Pressed == data.State)
        {
            Start = data.Start;
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
        Start = Vector2.zero;
        End = Vector2.zero;
        PressedTime = 0.0f;
        Distance = 0.0f;
    }
}
