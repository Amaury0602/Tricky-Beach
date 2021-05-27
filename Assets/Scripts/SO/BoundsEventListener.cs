using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public sealed class BoundsUnityEvent : UnityEvent<Bounds> { }

public class BoundsEventListener : MonoBehaviour
{
    // The game event instance to register to.
    public BoundsEvent BoundsEvent;
    // The unity event responce created for the event.
    public BoundsUnityEvent Response;

    private void OnEnable()
    {
        BoundsEvent.RegisterListerner(this);
    }

    private void OnDisable()
    {
        BoundsEvent.UnregisterListener(this);
    }

    public void RaiseEvent(Bounds bounds)
    {
        Response.Invoke(bounds);
    }
}