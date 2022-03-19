using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandHandler : MonoBehaviour
{
    [Header("Requirements")]
    public Rigidbody handRigidbody;
    public Rigidbody playerRigidbody;
    public Transform trackedController;

    [Header("Settings")]
    public bool computeForce;

    [Tooltip("This one is super buggy")]
    public bool computeTorque;

    [Header("States")]
    public bool isClimbing;

    [Header("Backward PD Adjustment")]
    public float posFrequency;
    public float posDamping;

    public float rotFrequency;
    public float rotDamping;

    public float climbSpringForce;
    public float climbSpringDamper;

    [Header("Debug")]
    public Vector3 outputForce;
    public Vector3 outputTorque;
    public Vector3 climbForce;
    public Vector3 climbDamp;

    private void Start()
    {
        Physics.defaultMaxAngularSpeed = 9000000;
    }

    void Update()
    {
        if (computeForce)
        {
            outputForce = ComputeForce(trackedController.position, handRigidbody, posFrequency, posDamping);
            handRigidbody.AddForce(outputForce);
        }

        if (computeTorque)
        {
            outputTorque = ComputeTorque(trackedController.rotation, handRigidbody, rotFrequency, rotDamping);
            handRigidbody.AddTorque(outputTorque);
        }
    }

    public Vector3 ComputeForce(Vector3 targetPosition, Rigidbody rigidbody, float frequency, float damping)
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float dt = Time.fixedDeltaTime;
        float g = 1 / (1 + kd * dt + kp * dt * dt);
        float ksg = kp * g;
        float kdg = (kd + kp * dt) * g;
        Vector3 F = (targetPosition - transform.position) * ksg + (playerRigidbody.velocity - handRigidbody.velocity) * kdg;
        return F;
    }

    public Vector3 ComputeTorque(Quaternion desiredRotation, Rigidbody rigidbody, float frequency, float damping)
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float dt = Time.fixedDeltaTime;
        float g = 1 / (1 + kd * dt + kp * dt * dt);
        float ksg = kp * g;
        float kdg = (kd + kp * dt) * g;
        Vector3 x;
        float xMag;
        Quaternion q = desiredRotation * Quaternion.Inverse(transform.rotation);
        // Q can be the-long-rotation-around-the-sphere eg. 350 degrees
        // We want the equivalant short rotation eg. -10 degrees
        // Check if rotation is greater than 190 degees == q.w is negative
        if (q.w < 0)
        {
            // Convert the quaterion to eqivalent "short way around" quaterion
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAngleAxis(out xMag, out x);
        x.Normalize();
        x *= Mathf.Deg2Rad;
        Vector3 pidv = kp * x * xMag - kd * rigidbody.angularVelocity;
        Quaternion rotInertia2World = rigidbody.inertiaTensorRotation * transform.rotation;
        pidv = Quaternion.Inverse(rotInertia2World) * pidv;
        pidv.Scale(rigidbody.inertiaTensor);
        pidv = rotInertia2World * pidv;
        return pidv;
    }

    private void OnCollisionStay(Collision collision)
    {
        isClimbing = true;

        if (playerRigidbody)
        {
            Vector3 displacement = transform.position - trackedController.position;

            climbForce = displacement * climbSpringForce;
            climbDamp = -playerRigidbody.velocity * climbSpringDamper;

            playerRigidbody.AddForce(climbForce, ForceMode.Acceleration);
            playerRigidbody.AddForce(climbDamp, ForceMode.Acceleration);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isClimbing = false;
    }
}