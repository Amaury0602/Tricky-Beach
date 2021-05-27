using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public sealed class IntUnityEvent : UnityEvent<int> { }

public class IntEventListener : MonoBehaviour
{
    // The game event instance to register to.
    public IntEvent GameEvent;
    // The unity event responce created for the event.
    public IntUnityEvent Response;

    private void OnEnable()
    {
        GameEvent.RegisterListerner(this);
    }

    private void OnDisable()
    {
        GameEvent.UnregisterListener(this);
    }

    public void RaiseEvent(int num)
    {
        Response.Invoke(num);
    }
}