using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public sealed class LocalUnityEvent : UnityEvent<Vector3> { }

public class LocalEventListener : MonoBehaviour
{
    // The game event instance to register to.
    public LocalEvent GameEvent;
    // The unity event responce created for the event.
    public LocalUnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListerner(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void RaiseEvent(Vector3 position)
    {
        Response.Invoke(position);
    }
}