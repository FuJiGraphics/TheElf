using fz;
using UnityEngine;

public class TouchDoubleTap : IDeviceContext
{
    public bool Active { get; private set; } = false;
    public Vector2 Position { get; private set; } = Vector2.zero;
    public float DurationLimit { get; set; } = 0.3f;
    public TouchDevice Device { get; private set; } = TouchDevice.Touch;

    private TouchTap data;
    private bool isFirstTouch;
    private float tapThreshold;

    public TouchDoubleTap()
    {
        data = new TouchTap();
    }

    public void OnUpdate()
    {
        data.OnUpdate();
        if (this.Active)
        {
            this.Clear();
            return;
        }

        if (isFirstTouch)
        {
            tapThreshold += Time.deltaTime;
            if (data.Active && tapThreshold <= DurationLimit)
            {
                Active = true;
                Position = data.End;
            }
            else if (tapThreshold > DurationLimit)
            {
                this.Clear();
            }
        }
        else if (data.Active)
        {
            isFirstTouch = true;
            tapThreshold = 0.0f;
        }
        else
        {
            this.Clear();
        }
    }

    public void SetDevice(TouchDevice device)
        => data.SetDevice(device);

    private void Clear()
    {
        Active = false;
        Position = Vector2.zero;
        isFirstTouch = false;
        tapThreshold = 0.0f;
    }

} // class TouchDoubleTap