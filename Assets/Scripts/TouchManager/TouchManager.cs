using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : Singleton<TouchManager>
{
    public virtual TouchTap Tap { get; private set; } = new TouchTap();
    public virtual TouchDoubleTap DoubleTap { get; private set; } = new TouchDoubleTap();
    public virtual TouchHeld TouchHeld { get; private set; } = new TouchHeld();
    public virtual TouchSwipe TouchSwipeUp { get; private set; } = new TouchSwipe(Vector2.up);
    public virtual TouchSwipe TouchSwipeDown { get; private set; } = new TouchSwipe(Vector2.down);
    public virtual TouchSwipe TouchSwipeRight { get; private set; } = new TouchSwipe(Vector2.right);
    public virtual TouchSwipe TouchSwipeLeft { get; private set; } = new TouchSwipe(Vector2.left);
    public virtual TouchZoom TouchZoom { get; private set; } = new TouchZoom();

    private void Update()
    {
        UpdateFeatures();
    }

    private void UpdateFeatures()
    {
        Tap.OnUpdate();
        DoubleTap.OnUpdate();
        TouchHeld.OnUpdate();
        TouchSwipeUp.OnUpdate();
        TouchSwipeDown.OnUpdate();
        TouchSwipeRight.OnUpdate();
        TouchSwipeLeft.OnUpdate();
        TouchZoom.OnUpdate();
    }

} // class TouchManager
