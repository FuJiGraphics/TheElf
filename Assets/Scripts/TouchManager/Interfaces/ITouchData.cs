using UnityEngine;

namespace fz
{
    public enum TouchState
    {
        None = 0,
        Pressed,
        Held,
        Released,
    }

    public interface ITouchData
    {
        public int Id { get; }
        public TouchState State { get; }
        public Vector2 Start { get; }
        public Vector2 End { get; }
        public float Distance { get; }
        public float PressedTime { get; }
        public void OnUpdate();
    }

} // namespace fz