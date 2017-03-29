using UnityEngine;


public class DeviceFinder : MonoBehaviour
{

    public enum Devices
    {
        Headset,
        Left_Controller,
        Right_Controller,
    }

    public enum ControllerHand
    {
        None,
        Left,
        Right
    }

    public static bool TrackedIndexIsController(uint index)
    {
        return SDK_Bridge.TrackedIndexIsController(index);
    }

    public static uint GetControllerIndex(GameObject controller)
    {
        return SDK_Bridge.GetIndexOfTrackedObject(controller);
    }

    public static GameObject TrackedObjectByIndex(uint index)
    {
        return SDK_Bridge.GetTrackedObjectByIndex(index);
    }

    public static Transform TrackedObjectOrigin(GameObject obj)
    {
        return SDK_Bridge.GetTrackedObjectOrigin(obj);
    }

    public static GameObject TrackedObjectOfGameObject(GameObject obj, out uint index)
    {
        return SDK_Bridge.GetTrackedObject(obj,out index);
    }

    public static Transform DeviceTransform(Devices device)
    {
        switch (device)
        {
            case Devices.Headset:
                return HeadsetTransform();
            case Devices.Left_Controller:
                return GetControllerLeftHand().transform;
            case Devices.Right_Controller:
                return GetControllerRightHand().transform;
        }
        return null;
    }

    public static ControllerHand GetControllerHandType(string hand)
    {
        switch(hand.ToLower())
        {
            case "left":
                return ControllerHand.Left;
            case "right":
                return ControllerHand.Right;
            default:
                return ControllerHand.None;
        }
    }

    public static ControllerHand GetControllerHand(GameObject controller)
    {
        if(SDK_Bridge.IsControllerLeftHand(controller))
        {
            return ControllerHand.Left;
        }
        else if(SDK_Bridge.IsControllerRightHand(controller))
        {
            return ControllerHand.Right;
        }
        else
        {
            return ControllerHand.None;
        }
    }

    public static GameObject GetControllerLeftHand()
    {
        return SDK_Bridge.GetControllerLeftHand();
    }

    public static GameObject GetControllerRightHand()
    {
        return SDK_Bridge.GetControllerRightHand();
    }

    public static bool IsControllerOfHand(GameObject checkController, ControllerHand hand)
    {
        if(hand == ControllerHand.Left && SDK_Bridge.IsControllerLeftHand(checkController))
        {
            return true;
        }

        if(hand == ControllerHand.Right && SDK_Bridge.IsControllerRightHand(checkController))
        {
            return true;
        }
        return false;
    }

    public static Transform HeadsetTransform()
    {
        return SDK_Bridge.GetHeadset();
    }

    public static Transform HeadsetCamera()
    {
        return SDK_Bridge.GetHeadsetCamera();
    }

    public static Transform PlayAreaTransform()
    {
        return SDK_Bridge.GetPlayArea();
    }


}//End Class
