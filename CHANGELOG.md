## Info

    Version: 0.0.4
    Released: 11/14/2021

## Releases

## 0.0.4

- Moved joystick touched input from universal to its respective input classes (Meta and Index)
- Removed the device position and rotation static references from VRInputHandler.cs as it was unnecessary
- Renamed class ItsVR to ItsSystems
- Added class ItsMath
- Add velocity and angular velocity calculations
- Added middle point calculation to ItsMath.cs
- Changed OculusInputs class to MetaInputs class
- Changed IndexInputs class to ValveInputs class
- Added summaries for all classes
- Renamed VRInputReferences.cs class to VRInputContainer.cs
- Renamed VRTrackerReferences.cs class to VRTrackerContainer.cs
- Renamed VRInputHandler.cs to VRInputSuite.cs
- Added input events for all inputs on the input container
- Added input events for all inputs on the tracker container
- Changed input reference modifiers from public to private
- Updated the license
- Updated all documentation

## 0.0.3

- Added teleport move sample height check at teleport target
- Added local space and world space velocity variable (Vector3) to VRTracker.cs
- Added local space and world space speed variable (Float) to VRTracker.cs
- HMD specific inputs seperated by classes on VRInputReferences.cs (Universal, System, Oculus, Valve, Vive)
- You can now set the main interactor and main interactable attachment point properties in VRInteractable.cs
- Added front to back and right to left holding calculations in VRGrabbable.cs
- Added Button sample
- Added interactor selection panel in the interaction sample scene
- Fixed teleport move sample positioning
- Added Right hand controller and Left hand controller properties on ItsVR.cs
- Finalized the Interaction Sample scene
- Finalized the Locomotion Sample scene

## 0.0.2

- Added snap turn locomotion sample
- Added continuous turn locomotion sample
- Added continuous move locomotion sample
- Added teleport move locomotion sample
- Added Grabbable object sample
- Added Direct interactor
- Added Ray interactor
- Added tons more input bindings for Oculus (Meta), Index, Vive, and Mixed Reality
- Haptics support
- Added tracker velocity, angular velocity, and speed properties in VRTracker.cs
- Fixed hip and feet position variables in VRRig.cs

## 0.0.1

- Gave every debug context
- Fixed all gizmos
- Add hip position and hip local position variable to the VR Rig
- Documentation for all components (Except for VRInputReferences.cs and VRTrackerReferences.cs)
- Many other bug fixes

## 0.0.0

- Initial version
