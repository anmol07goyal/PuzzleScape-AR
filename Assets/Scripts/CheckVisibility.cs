using System;
using UnityEngine;

public class CheckVisibility : MonoBehaviour
{
    public static Action<bool, Transform> ObjectVisibility;

    private void OnBecameVisible()
    {
        Debug.Log("visible: " + gameObject.name);
        ObjectVisibility?.Invoke(true, null);
    }

    private void OnBecameInvisible()
    {
        Debug.Log("invisible: " + gameObject.name);
        ObjectVisibility?.Invoke(false, transform);
    }
}