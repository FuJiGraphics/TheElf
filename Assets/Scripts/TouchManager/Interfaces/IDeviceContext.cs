namespace fz
{
    public enum TouchDevice
    {
        Unknown = 0,
        Mouse, Keyboard, Touch, Joystick
    }

    public interface IDeviceContext
    {
        public TouchDevice Device { get; }
        public void SetDevice(TouchDevice device);
    }

} // namespace fz
