using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HMI.UI{

public class ClusterSpeed : MonoBehaviour
{
    /// <summary>
    /// Textual speed of the Ego Vehicle
    /// </summary>
    public TMPro.TextMeshPro Text;

    public float speed;


    private void Start()
    {
            
        Text = GetComponent<TextMeshPro>(); 
            
    }


    /// <summary>
    /// Update the textual speed
    /// </summary>
    void Update()
    {
        presentClusterSpeed();
    }
    
    /// <summary>
    /// show the textual speed
    /// </summary>
    private void presentClusterSpeed()
    {
        Text.text = speed.ToString("F1") + " km/h";
    }
}

}
