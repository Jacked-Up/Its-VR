using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsVRGrabber : MonoBehaviour
{
    public HandSide side;
    public VRInput grabButton;

    public PhysicsVRGrabbable current;

    public ConfigurableJoint configJ;
    public FixedJoint fixedJ;

    public bool debugMode;
    public bool grab;

    private void Update()
    {
        if (!debugMode)
            grab = InputHandler.GetInputBool(side, grabButton);
    }

    public void GrabFixed()
    {
        if (current.grabType == GrabType.Fixed)
        {
            if (side == HandSide.Left)
            {
                transform.position = current.pointLeft.position;
                transform.rotation = current.pointLeft.rotation;

                fixedJ = gameObject.AddComponent<FixedJoint>();
                fixedJ.connectedBody = current.rb;
            }

            if (side == HandSide.Right)
            {
                transform.position = current.pointRight.position;
                transform.rotation = current.pointRight.rotation;

                fixedJ = gameObject.AddComponent<FixedJoint>();
                fixedJ.connectedBody = current.rb;
            }
        }

        if (current.grabType == GrabType.Free)
        {
            if (side == HandSide.Left)
            {
                fixedJ = gameObject.AddComponent<FixedJoint>();
                fixedJ.connectedBody = current.rb;
            }

            if (side == HandSide.Right)
            {
                fixedJ = gameObject.AddComponent<FixedJoint>();
                fixedJ.connectedBody = current.rb;
            }
        }
    }

    public void GrabConfig()
    {
        if (current.grabType == GrabType.Fixed)
        {
            if (side == HandSide.Left)
            {
                transform.position = current.pointLeft.position;
                transform.rotation = current.pointLeft.rotation;

                configJ = gameObject.AddComponent<ConfigurableJoint>();

                configJ.xMotion = configJ.yMotion = configJ.zMotion = configJ.angularXMotion = configJ.angularYMotion = configJ.angularZMotion = ConfigurableJointMotion.Locked;

                configJ.connectedBody = current.rb;
            }

            if (side == HandSide.Right)
            {
                transform.position = current.pointRight.position;
                transform.rotation = current.pointRight.rotation;

                configJ = gameObject.AddComponent<ConfigurableJoint>();

                configJ.xMotion = configJ.yMotion = configJ.zMotion = configJ.angularXMotion = configJ.angularYMotion = configJ.angularZMotion = ConfigurableJointMotion.Locked;

                configJ.connectedBody = current.rb;
            }
        }

        if (current.grabType == GrabType.Free)
        {
            if (side == HandSide.Left)
            {
                configJ = gameObject.AddComponent<ConfigurableJoint>();

                configJ.xMotion = configJ.yMotion = configJ.zMotion = configJ.angularXMotion = configJ.angularYMotion = configJ.angularZMotion = ConfigurableJointMotion.Locked;

                configJ.connectedBody = current.rb;
            }

            if (side == HandSide.Right)
            {
                configJ = gameObject.AddComponent<ConfigurableJoint>();

                configJ.xMotion = configJ.yMotion = configJ.zMotion = configJ.angularXMotion = configJ.angularYMotion = configJ.angularZMotion = ConfigurableJointMotion.Locked;

                configJ.connectedBody = current.rb;
            }
        }
    }

    public void Release()
    {
        current.grabbers.Remove(this);
        current = null;

        if (configJ)
        {
            Destroy(configJ);
        }

        if (fixedJ)
        {
            Destroy(fixedJ);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PhysicsVRGrabbable>() || other.GetComponent<PhysicsVRGrabbableChild>() && grab)
        {
            var child = other.GetComponent<PhysicsVRGrabbableChild>();
            if (child)
            {
                current = child.parent;
            }
            else
            {
                current = other.GetComponent<PhysicsVRGrabbable>();
            }

            if (!current)
                return;

            if (!current.grabbers.Contains(this))
            {
                current.grabbers.Add(this);
            }

            if (current.jointToUse == JointToUse.ConfigurableJoint && !configJ)
            {
                GrabConfig();
            }

            if (current.jointToUse == JointToUse.FixedJoint && !fixedJ)
            {
                GrabFixed();
            }
        }

        if (!grab && current)
        {
            Release();
        }
    }
}
