using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    // The game event instance to register to.
    public GameEvent GameEvent;
    // The unity event responce created for the event.
    public UnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListerner(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void RaiseEvent()
    {
        Response.Invoke();
    }
}