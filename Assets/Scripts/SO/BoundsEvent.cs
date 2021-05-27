using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBoundEvent", menuName = "SO/Event/BoundEvent")]
public class BoundsEvent : ScriptableObject
{

    private List<BoundsEventListener> listeners = new List<BoundsEventListener>();
    public void RegisterListerner(BoundsEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(BoundsEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(Bounds bounds)
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].RaiseEvent(bounds);
        }
    }
}