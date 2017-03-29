//声明委托事件
/// <summary>
/// Handle event handler.
/// </summary>
/// 在Input变化时响应对应的事件。

using UnityEngine;

//Delegate Statement
public delegate void HandleEventHandler(object sender);

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class HandleEventController : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObj;

    #region Event Statement
    public event HandleEventHandler ApplicationMenuPressedDownEvent;

    public event HandleEventHandler ApplicationMenuPressedUpEvent;

    public event HandleEventHandler TriggerPressedDownEvent;

    public event HandleEventHandler TriggerPressedUpEvent;

    public event HandleEventHandler TriggerPressedEvent;

    public event HandleEventHandler TouchPadPressedDownEvent;

    public event HandleEventHandler TouchPadPressedUpEvent;

    public event HandleEventHandler GrispPressedDownEvent;

    public event HandleEventHandler GrispPressedUpEvent;
    #endregion

    #region Execute Event
    private void OnApplicationPressedDown()
    {
        if(ApplicationMenuPressedDownEvent != null)
        {
            ApplicationMenuPressedDownEvent(this);
        }
    }

    private void OnApplicationPressedUp()
    {
        if(ApplicationMenuPressedUpEvent != null)
        {
            ApplicationMenuPressedUpEvent(this);
        }
    }

    private void OnTriggerPressedDown()
    {
        if(TriggerPressedDownEvent != null)
        {
            TriggerPressedDownEvent(this);
        }
    }

    private void OnTriggerPressedUp()
    {
        if (TriggerPressedUpEvent != null)
        {
            TriggerPressedUpEvent(this);
        }
    }

    private void OnTriggerPressed()
    {
        if(TriggerPressedEvent != null)
        {
            TriggerPressedEvent(this);
        }
    }

    private void OnTouchpadPressedDown()
    {
        if(TouchPadPressedDownEvent != null)
        {
            TouchPadPressedDownEvent(this);
        }
    }

    private void OnTouchpadPressedUp()
    {
        if(TouchPadPressedUpEvent != null)
        {
            TouchPadPressedUpEvent(this);
        }
    }

    private void OnGrispPressedDown()
    {
        if(GrispPressedDownEvent != null)
        {
            GrispPressedDownEvent(this);
        }
    }

    private void OnGrispPressedUp()
    {
        if(GrispPressedUpEvent != null)
        {
            GrispPressedUpEvent(this);
        }
    }
    #endregion

    #region Get Input
    /// <summary>
    /// get the  Input Event of the handle
    /// </summary>
    private void GetInputEvent()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        //pressed down ApplicationMenu
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnApplicationPressedDown();
        }

        //pressed up ApplicationMenu
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnApplicationPressedUp();
        }

        //pressed down Trigger
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerPressedDown();
        }

        //pressed up Trigger
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerPressedUp();
        }

        //pressed  Trigger
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerPressed();
        }

        //pressed down Touchpad
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            OnTouchpadPressedDown();
        }

        //pressed up Touchpad
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            OnTouchpadPressedUp();
        }

        //pressed down Grip
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            OnGrispPressedDown();
        }

        //pressed up Grip
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            OnGrispPressedUp();
        }

    }//End Method  GetInputEvent
    #endregion


    private void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    private void Update()
    {
        GetInputEvent();
    }

}//End Class
