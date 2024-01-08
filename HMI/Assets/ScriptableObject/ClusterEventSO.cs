using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/ClusterEventSO")]
public class ClusterEventSO : ScriptableObject
{
    public UnityAction<SharedMemoryReader> OnEventRaise;


    public void RaiseEvent(SharedMemoryReader reader)
    {
        OnEventRaise?.Invoke(reader);
    }    
    
}
