using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    public GameObject standing;
    public GameObject jumpPrep;
    public GameObject nonGrounded;

    public bool grounded;

    private void Update()
    {
        if (grounded)
        {
            standing.SetActive(true);
            jumpPrep.SetActive(false);
            nonGrounded.SetActive(false);
        }

        if (grounded && InputHandler.GetInputBool(HandSide.Right, VRInput.Joystick))
        {
            standing.SetActive(false);
            jumpPrep.SetActive(true);
            nonGrounded.SetActive(false);
        }

        if (!grounded)
        {
            standing.SetActive(false);
            jumpPrep.SetActive(false);
            nonGrounded.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        grounded = false;
    }
}
