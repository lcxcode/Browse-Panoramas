//获取手柄的Input并将对应的事件绑定到相应的Input上，

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEventExecuter : MonoBehaviour
{

    protected HandleEventController hec;
    public Transform orientionTrans;

    public GameObject curMenuCanvas;

    protected GameObject HeadSet;

    protected bool IsUpdateCanvasPos;

    private void OnEnable()
    {
        hec = FindObjectOfType<HandleEventController>();
        HeadSet = DeviceFinder.HeadsetCamera().gameObject;
        hec.ApplicationMenuPressedDownEvent += new HandleEventHandler(ApplicationPressedDown);
        hec.ApplicationMenuPressedUpEvent += new HandleEventHandler(ApplicationPressedUp);
        hec.TriggerPressedDownEvent += new global::HandleEventHandler(TriggerPressedDown);
        hec.TriggerPressedUpEvent += new HandleEventHandler(TriggerPressedUp);
        hec.TriggerPressedEvent += new HandleEventHandler(TriggerPressed);
        hec.TouchPadPressedDownEvent += new HandleEventHandler(TouchpadPressedDown);
        hec.TouchPadPressedUpEvent += new HandleEventHandler(TouchpadPressedUp);
        hec.GrispPressedDownEvent += new HandleEventHandler(GripPressedDown);
    }

    private void OnDisable()
    {
        hec.ApplicationMenuPressedDownEvent -= new HandleEventHandler(ApplicationPressedDown);
        hec.ApplicationMenuPressedUpEvent -= new HandleEventHandler(ApplicationPressedUp);
        hec.TriggerPressedDownEvent -= new global::HandleEventHandler(TriggerPressedDown);
        hec.TriggerPressedUpEvent -= new HandleEventHandler(TriggerPressedUp);
        hec.TriggerPressedEvent -= new HandleEventHandler(TriggerPressed);
        hec.TouchPadPressedDownEvent -= new HandleEventHandler(TouchpadPressedDown);
        hec.TouchPadPressedUpEvent -= new HandleEventHandler(TouchpadPressedUp);
        hec.GrispPressedDownEvent -= new HandleEventHandler(GripPressedDown);
        hec = null;
        HeadSet = null;
    }


    protected virtual void ApplicationPressedDown(object sender)
    {
        Debug.Log("ApplicationMenu Pressed Down");
        ReSetMenuPos();
    }

    protected virtual void ApplicationPressedUp(object sender)
    {
        Debug.Log("ApplicationMenu Pressed Up");
    }

    protected virtual void TriggerPressedDown(object sender)
    {
        Debug.Log("Trigger pressed Down");
    }
    protected virtual void TriggerPressedUp(object sender)
    {
        Debug.Log("Trigger pressed Up");
    }

    protected virtual void TriggerPressed(object sender)
    {
        Debug.Log("Trigger pressed");
    }

    protected virtual void TouchpadPressedDown(object sender)
    {
        Debug.Log("Touchpad pressed Down");
    }

    protected virtual void TouchpadPressedUp(object sender)
    {
        Debug.Log("Touchpad Pressed Up");
    }

    protected virtual void GripPressedDown(object sender)
    {
        Debug.Log("Grip Pressed Down");
    }

    private void ReSetMenuPos()
    {
        Vector3 delta = HeadSet.transform.position + HeadSet.transform.forward * 2f;
        delta.y = HeadSet.transform.position.y - 0.25f;
        orientionTrans.transform.position = delta;
        orientionTrans.transform.rotation = new Quaternion(0, HeadSet.transform.rotation.y, 0, HeadSet.transform.rotation.w);
        IsUpdateCanvasPos = true;
    }


    protected void UpdateCanvasPos()
    {
        if (IsUpdateCanvasPos)
        {
            curMenuCanvas.transform.position = orientionTrans.position;
            curMenuCanvas.transform.rotation = orientionTrans.rotation;
            IsUpdateCanvasPos = false;
            curMenuCanvas.SetActive(!curMenuCanvas.activeInHierarchy);
        }
    }

}//End Class
