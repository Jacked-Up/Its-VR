using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
    public UnityEvent Pressed;
    public UnityEvent Depressed;

    private void OnTriggerEnter(Collider other)
    {
        Pressed.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        Depressed.Invoke();
    }
}
