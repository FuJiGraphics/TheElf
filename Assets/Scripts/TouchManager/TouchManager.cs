using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : Singleton<TouchManager>
{
    public TouchTap Tap { get; private set; } = new TouchTap();
    public TouchDoubleTap DoubleTap { get; private set; } = new TouchDoubleTap();
    public TouchHeld TouchHeld { get; private set; } = new TouchHeld();
    public TouchSwipe TouchSwipeUp { get; private set; } = new TouchSwipe(Vector2.up);
    public TouchSwipe TouchSwipeDown { get; private set; } = new TouchSwipe(Vector2.down);
    public TouchSwipe TouchSwipeRight { get; private set; } = new TouchSwipe(Vector2.right);
    public TouchSwipe TouchSwipeLeft { get; private set; } = new TouchSwipe(Vector2.left);
    public TouchZoom TouchZoom { get; private set; } = new TouchZoom();

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
