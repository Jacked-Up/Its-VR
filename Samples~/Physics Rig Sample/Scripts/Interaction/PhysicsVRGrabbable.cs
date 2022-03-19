using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum JointToUse
{
    ConfigurableJoint,
    FixedJoint
}

public enum GrabType
{
    Fixed,
    Free
}

public class PhysicsVRGrabbable : MonoBehaviour
{
    public JointToUse jointToUse;
    public GrabType grabType;

    public Rigidbody rb;

    public Transform pointLeft;
    public Transform pointRight;

    public List<PhysicsVRGrabber> grabbers = new List<PhysicsVRGrabber>();
}
