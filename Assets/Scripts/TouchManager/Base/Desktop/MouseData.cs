using UnityEngine;
using fz;

public class MouseData : ITouchData
{
    public int Id { get; private set; } = -1;
    public TouchState State { get; private set; } = TouchState.None;
    public Vector2 Start { get; private set; } = Vector2.zero;
    public Vector2 End { get; private set; } = Vector2.zero;
    public float Distance { get; private set; } = 0.0f;
    public float PressedTime { get; private set; } = 0.0f;

    public MouseData(int id)
    {
        Id = id;
    }

    public void OnUpdate()
    {
        if (Input.touchCount < 1 || TouchState.Released == State)
        {
            this.Clear();
            return;
        }

        this.OnState();
    }

    private void Clear()
    {
        State = TouchState.None;
        Start = Vector2.zero;
        End = Vector2.zero;
        Distance = 0.0f;
        PressedTime = 0.0f;
    }

    private void OnState()
    {
        if (Input.GetMouseButtonDown(0))
        {
            State = TouchState.Pressed;
            Start = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            State = TouchState.Held;
            End = Input.mousePosition;
            PressedTime += Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            State = TouchState.Released;
            End = Input.mousePosition;
            Distance = (End - Start).magnitude;
        }
    }

} // class TouchData