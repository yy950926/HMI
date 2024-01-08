using System;
using System.Collections;
using System.Collections.Generic;
using HMI.UI;
using UnityEngine;


public class UIManager :  MonoBehaviour
{
    public ClusterEventSO clusterEvent;

    public ClusterSpeed EgoCarSpeed;

    
    
    private void OnEnable() {
        clusterEvent.OnEventRaise += OnClusterEvent;
    }

    private void OnDisable() {
        clusterEvent.OnEventRaise -= OnClusterEvent;
    }

    private void OnClusterEvent(SharedMemoryReader SharedMemoryReader)
    {
        EgoCarSpeed.speed = SharedMemoryReader.currentSpeed;
        
    }
}
