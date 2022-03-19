using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItsVR;
using ItsVR.Input;
using ItsVR.Scriptables;

public enum HandSide
{
    Left,
    Right
}

public enum VRInput
{
    Trigger,
    Grip,
    Primary,
    Secondary,
    Joystick
}

public class InputHandler : MonoBehaviour
{
    [Header("Required")]
    public VRInputContainer LeftController;
    public VRInputContainer RightController;
    public static VRInputContainer LeftControllerS;
    public static VRInputContainer RightControllerS;

    [Header("Left Debug")]
    public Vector2 leftJoystick;
    public Color leftTrigger;
    public Color leftGrip;
    public Color leftPrimary;
    public Color leftSecondary;

    [Header("Right Debug")]
    public Vector2 rightJoystick;
    public Color rightTrigger;
    public Color rightGrip;
    public Color rightPrimary;
    public Color rightSecondary;

    private void Awake()
    {
        LeftControllerS = LeftController;
        RightControllerS = RightController;
    }

    private void Update()
    {
        leftJoystick = GetInputVector2(HandSide.Left, VRInput.Joystick);
        leftTrigger = DebugYes(HandSide.Left, VRInput.Trigger);
        leftGrip = DebugYes(HandSide.Left, VRInput.Grip);
        leftPrimary = DebugYes(HandSide.Left, VRInput.Primary);
        leftSecondary = DebugYes(HandSide.Left, VRInput.Secondary);  
        
        leftJoystick = GetInputVector2(HandSide.Right, VRInput.Joystick);
        rightTrigger = DebugYes(HandSide.Right, VRInput.Trigger);
        rightGrip = DebugYes(HandSide.Right, VRInput.Grip);
        rightPrimary = DebugYes(HandSide.Right, VRInput.Primary);
        rightSecondary = DebugYes(HandSide.Right, VRInput.Secondary); 
    }

    public Color DebugYes(HandSide side, VRInput input)
    {
        if (GetInputBool(side, input))
        {
            return Color.green;
        }
        return Color.red;
    }

    public static bool GetInputBool(HandSide side, VRInput input)
    {
        bool state = false;

        if (side == HandSide.Left)
        {
            if (input == VRInput.Trigger)
            {
                state = LeftControllerS.universalInputs.TriggerPressed;
            }

            if (input == VRInput.Grip)
            {
                state = LeftControllerS.universalInputs.GripPressed;
            }

            if (input == VRInput.Primary)
            {
                state = LeftControllerS.universalInputs.PrimaryButtonPressed;
            }

            if (input == VRInput.Secondary)
            {
                state = LeftControllerS.universalInputs.SecondaryButtonPressed;
            }

            if (input == VRInput.Joystick)
            {
                state = LeftControllerS.universalInputs.JoystickPressed;
            }
        }

        if (side == HandSide.Right)
        {
            if (input == VRInput.Trigger)
            {
                state = RightControllerS.universalInputs.TriggerPressed;
            }

            if (input == VRInput.Grip)
            {
                state = RightControllerS.universalInputs.GripPressed;
            }

            if (input == VRInput.Primary)
            {
                state = RightControllerS.universalInputs.PrimaryButtonPressed;
            }

            if (input == VRInput.Secondary)
            {
                state = RightControllerS.universalInputs.SecondaryButtonPressed;
            }

            if (input == VRInput.Joystick)
            {
                state = RightControllerS.universalInputs.JoystickPressed;
            }
        }

        return state;
    }

    public static Vector2 GetInputVector2(HandSide side, VRInput input)
    {
        Vector2 state = Vector2.zero;

        if (side == HandSide.Left)
        {
            if (input == VRInput.Joystick)
            {
                state = LeftControllerS.universalInputs.JoystickPosition;
            }
        }

        if (side == HandSide.Right)
        {
            if (input == VRInput.Joystick)
            {
                state = RightControllerS.universalInputs.JoystickPosition;
            }
        }

        return state;
    }
}
