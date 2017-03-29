using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ControllerEventArgs
{
    public uint controllerIndex;
    public float buttonPressure;
    public Vector2 touchpadAxis;
}

public delegate void ControllerEventHandler(object sender, ControllerEventArgs e);


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ControllerEvent : MonoBehaviour
{

    public int axisFidelity = 1;

    private uint controllerIndex;

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device device;

    private Vector2 touchpadAxis = Vector2.zero;

    private Vector2 triggerAxis = Vector2.zero;

    private bool controllerVisible = true;

    private ushort hapticPulseStrength;

    private int hapticPulseCountdown;

    private ushort maxHapticVibration = 3999;

    public event ControllerEventHandler MenuPressedDown;

    public event ControllerEventHandler MenuPressedUp;

    public event ControllerEventHandler TriggerPressedDown;

    public event ControllerEventHandler TriggerPressedUp;

    public event ControllerEventHandler TriggerPressed;

    public event ControllerEventHandler TriggerAxisChanged;

    public event ControllerEventHandler TouchpadPressedDown;

    public event ControllerEventHandler TouchpadPressedUp;

    public event ControllerEventHandler TouchpadAxisChanged;

    public event ControllerEventHandler GripPressedDown;

    public event ControllerEventHandler GripPressedUp;


    public virtual void OnMenuPressedDown(ControllerEventArgs e)
    {
        if(MenuPressedDown != null)
        {
            MenuPressedDown(this,e);
            Debug.Log("applicationmenuPressedDown");
        }
    }



    public virtual void OnMenuPressedUp(ControllerEventArgs e)
    {
        if(MenuPressedUp != null)
        {
            MenuPressedUp(this,e);
        }
    }


    public virtual void OnTriggerPressedDown(ControllerEventArgs e)
    {
        if(TriggerPressedDown != null)
        {
            TriggerPressedDown(this, e);
        }
    }


    public virtual void OnTriggerPressedUp(ControllerEventArgs e)
    {
        if(TriggerPressedUp != null)
        {
            TriggerPressedUp(this, e);
        }
    }

    public virtual void OnTriggerPressed(ControllerEventArgs e)
    {
        if(TriggerPressed != null)
        {
            TriggerPressed(this, e);
        }
    }

    public virtual void OnTriggerAxisChanged(ControllerEventArgs e)
    {
        if(TriggerAxisChanged != null)
        {
            TriggerAxisChanged(this,e);
        }
    }

    public virtual void OnTouchpadPressedDown(ControllerEventArgs e)
    {
        if(TouchpadPressedDown != null)
        {
            TouchpadPressedDown(this, e);
        }
    }

    public virtual void OnTouchpadPressedUp(ControllerEventArgs e)
    {
        if(TouchpadPressedUp != null)
        {
            TouchpadPressedUp(this, e);
        }
    }

    public virtual void OnTouchpadAxisChanged(ControllerEventArgs e)
    {
        if(TouchpadAxisChanged != null)
        {
            TouchpadAxisChanged(this, e);
        }
    }

    public virtual void OnGripPressedDown(ControllerEventArgs e)
    {
        if(GripPressedDown != null)
        {
            GripPressedDown(this, e);
        }
    }

    public virtual void OnGripPressedUp(ControllerEventArgs e)
    {
        if(GripPressedUp != null)
        {
            GripPressedUp(this, e);
        }
    }

    ControllerEventArgs SetControllerEventArgs(ref bool buttonBool, bool value, float buttonPressure)
    {
        buttonBool = value;
        ControllerEventArgs e;
        e.controllerIndex = controllerIndex;
        e.buttonPressure = buttonPressure;
        e.touchpadAxis = device.GetAxis();
        return e;
    }

    private void GetHandleInput()
    {

        controllerIndex = (uint)trackedObj.index;
         device = SteamVR_Controller.Input((int)trackedObj.index);

        Vector2 currentTriggerAxis = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
        Vector2 currentTouchpadAxis = device.GetAxis();

        if(hapticPulseCountdown > 0)
        {
            device.TriggerHapticPulse(hapticPulseStrength);
            hapticPulseCountdown -= 1;
        }

        if(Vector2ShallowEquals(triggerAxis, currentTriggerAxis))
        {
            triggerAxisChanged = false;
        }
        else
        {
            OnTriggerAxisChanged(SetControllerEventArgs(ref triggerPressed, true, currentTriggerAxis.x));
            triggerAxisChanged = true;
        }


        if(Vector2ShallowEquals(touchpadAxis, currentTouchpadAxis))
        {
            touchpadAxisChanged = false;
        }
        else
        {
            OnTouchpadAxisChanged(SetControllerEventArgs(ref touchpadTouched,true,1f));
            touchpadAxisChanged = true;
        }

        touchpadAxis = new Vector2(currentTouchpadAxis.x, currentTouchpadAxis.y);
        triggerAxis = new Vector2(currentTriggerAxis.x, currentTriggerAxis.y);

        if(device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnTriggerPressedDown(SetControllerEventArgs(ref triggerPressed, true,currentTouchpadAxis.x));
            triggerDown = true;
        }
        else
        {
            triggerDown = false;
        }

        if(device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            OnTriggerPressedUp(SetControllerEventArgs(ref triggerPressed, false, 0f));
            triggerUp = true;
        }
        else
        {
            triggerUp = false;
        }

        if(device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnMenuPressedDown(SetControllerEventArgs(ref applicationMenuPressed, true, 1f));
        }

        if(device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            OnMenuPressedUp(SetControllerEventArgs(ref applicationMenuPressed, false, 0f));
        }

        //Grip
        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            OnGripPressedDown(SetControllerEventArgs(ref gripPressed, true, 1f));
        }
        else if(device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            OnGripPressedUp(SetControllerEventArgs(ref gripPressed, false, 0f));
        }

        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            OnTouchpadPressedDown(SetControllerEventArgs(ref touchpadPressed, true, 1f));
        }
        else if(device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            OnTouchpadPressedUp(SetControllerEventArgs(ref touchpadPressed, false, 0f));
        }

    }


    bool Vector2ShallowEquals(Vector2 vectorA, Vector2 vectorB)
    {
        return (vectorA.x.ToString("F" + axisFidelity) == vectorB.x.ToString("F" + axisFidelity) &&
                vectorA.y.ToString("F" + axisFidelity) == vectorB.y.ToString("F" + axisFidelity));
    }


    public void ToggleControllerVisible(bool on)
    {
        foreach (MeshRenderer renderer in this.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = on;
        }

        foreach (SkinnedMeshRenderer renderer in this.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = on;
        }
        controllerVisible = on;
    }


    public void TriggerHapticPulse(int duration, ushort strength)
    {
        hapticPulseCountdown = duration;
        hapticPulseStrength = (strength <= maxHapticVibration ? strength : maxHapticVibration);
    }

    #region SETTERS AND GETTERS

    public bool IsControllerVisible() { return controllerVisible; }

    public bool IsTriggerDown { get { return triggerDown; } }
    bool triggerDown = false;

    public bool IsTriggerUp { get { return triggerUp; } }
    bool triggerUp = false;

    public bool IsTriggerPressed { get { return triggerPressed; } }
    bool triggerPressed = false;

    public bool IsTriggerAxisChanged { get { return triggerAxisChanged; } }
    bool triggerAxisChanged = false;

    public bool IsTouchpadAxisChanged { get{ return touchpadAxisChanged; } }
    bool touchpadAxisChanged = false;

    public bool IsTouchpadPressed { get { return touchpadPressed; } }
    bool touchpadPressed = false;

    public bool IsTouchpadTouched { get { return touchpadTouched; } }
    bool touchpadTouched = false;

    public bool IsMenuPressed { get { return applicationMenuPressed; } }
    bool applicationMenuPressed = false;

    public bool IsGripPressed { get{ return gripPressed; } }
    bool gripPressed = false;

    public Vector2 TouchpadAxis { get{ return TouchpadAxis; } }
    #endregion


    // Use this for initialization
    void Start ()
    {
        trackedObj = FindObjectOfType<SteamVR_TrackedObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetHandleInput();
    }


}//End Class
