using fz;
using UnityEngine;

public interface ITouchHeld
{
    public bool Active { get; }
    public Vector2 Position { get; }
    public float PressedTime { get; }
    public TouchDevice Device { get; }
}