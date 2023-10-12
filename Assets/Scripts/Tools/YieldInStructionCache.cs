using System.Collections.Generic;
using UnityEngine;

internal static class YieldInStructionCache
{
    private class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return x == y;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    private static readonly Dictionary<float, WaitForSeconds> _WaitForSeconds =
        new Dictionary<float, WaitForSeconds>(new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (_WaitForSeconds.TryGetValue(seconds, out var waitForSeconds))
        {
            _WaitForSeconds.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
        }

        return waitForSeconds;
    }
}