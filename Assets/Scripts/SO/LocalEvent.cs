using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLocalEvent", menuName = "SO/Event/LocalEvent")]
public class LocalEvent : ScriptableObject
{

    private List<LocalEventListener> listeners = new List<LocalEventListener>();
    public void RegisterListerner(LocalEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(LocalEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(Vector3 position)
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].RaiseEvent(position);
        }
    }
}