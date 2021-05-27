using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewIntEvent", menuName = "SO/Event/IntEvent")]
public class IntEvent : ScriptableObject
{

    private List<IntEventListener> listeners = new List<IntEventListener>();
    public void RegisterListerner(IntEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(IntEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise(int num)
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].RaiseEvent(num);
        }
    }
}