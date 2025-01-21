using UnityEngine;
using fz;

public class TouchData : ITouchData
{
    public int Id { get; private set; } = -1;
    public TouchState State { get; private set; } = TouchState.None;
    public Vector2 Start { get; private set; } = Vector2.zero;
    public Vector2 End { get; private set; } = Vector2.zero;
    public float Distance { get; private set; } = 0.0f;
    public float PressedTime { get; private set; } = 0.0f;

    public TouchData(int id)
    {
        Id = id;
    }

    public void OnUpdate()
    {
        if (Input.touchCount == 0 || TouchState.Released == State)
        {
            this.Clear();
            return;
        }

        var touch = Input.GetTouch(Id);
        this.OnState(ref touch);
    }

    private void Clear()
    {
        State = TouchState.None;
        Start = Vector2.zero;
        Distance = 0.0f;
        PressedTime = 0.0f;
    }

    private void OnState(ref Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                State = TouchState.Pressed;
                Start = touch.position;
                break;
            case TouchPhase.Moved:
            case TouchPhase.Stationary:
                State = TouchState.Held;
                End = touch.position;
                PressedTime += Time.deltaTime;
                break;
            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                State = TouchState.Released;
                End = touch.position;
                Distance = (End - Start).magnitude;
                break;
        }
    }

} // class TouchData