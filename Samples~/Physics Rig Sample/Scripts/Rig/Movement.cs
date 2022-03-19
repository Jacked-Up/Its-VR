using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public HandSide movementSide;
    public HandSide rotationSide;

    public Vector2 inputsM;
    public Vector2 inputsR;

    public Rigidbody rb;

    public BodyManager m;

    public PhysicMaterial grip;
    public Collider[] body;

    public float speed;
    public float jumpForce;

    public bool debugMode;

    private void Update()
    {
        //Vector3 vel = new Vector3(rb.velocity.x + inputsM.x, rb.velocity.y, rb.velocity.z + inputsM.y);

        if (debugMode)
        {
            inputsM.x = Input.GetAxis("Horizontal");
            inputsM.y = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space) && m.grounded)
            {
                foreach (var item in body)
                {
                    item.material = null;
                }
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
        else
        {
            inputsM = InputHandler.GetInputVector2(movementSide, VRInput.Joystick);

            if (InputHandler.GetInputBool(movementSide, VRInput.Joystick) && m.grounded)
            {
                foreach (var item in body)
                {
                    item.material = null;
                }
                rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (inputsM.magnitude > 0.5f)
        {
            foreach (var item in body)
            {
                item.material = null;
            }
        }

        if (inputsM.magnitude < 0.5f)
        {
            foreach (var item in body)
            {
                item.material = grip;
            }
        }

        rb.AddForce(m.transform.TransformDirection(inputsM.x*speed, 0, inputsM.y*speed),ForceMode.VelocityChange);
    }
}
