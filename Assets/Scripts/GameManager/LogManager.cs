
using UnityEngine;

public static class LogManager
{
    public static bool IsVaild(UnityEngine.Object obj)
        => !LogManager.IsNull(obj);
    
    public static bool IsNull(UnityEngine.Object obj)
    {
        bool result = false;
        if (obj == null)
        {
            Debug.LogWarning($"Did not found Object!");
            result = true;
        }
        return result;
    }

} // static class 