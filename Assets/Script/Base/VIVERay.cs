using UnityEngine;
using System.Collections;


[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VIVERay : MonoBehaviour
{
    RaycastHit hitInfo;

    private SteamVR_TrackedObject trackedObj;
    private FixedJoint joint;

    //线段渲染器  
    private LineRenderer lineRenderer;

    public int distance = 1;

    private Vector3 pointPos;

    private Transform mtransform;

    private Vector3 forwordEndPoint;

    public Material lineMaterial;

    [HideInInspector]
    public Transform rayTransform;

    public Transform pointTranform;

    private bool pulseBool;

    /// <summary>
    /// true 为震动
    /// </summary>
    public bool triggerHapticPulse;

    public void Init()
    {
        mtransform = transform;
        lineRenderer = mtransform.GetComponent<LineRenderer>();
        trackedObj = mtransform.GetComponent<SteamVR_TrackedObject>();
        lineRenderer.material = lineMaterial;
    }

    // Use this for initialization
    public void Init(Transform tra, Material linemat, Transform point)
    {
        mtransform = tra;
        lineMaterial = linemat;
        pointTranform = point;
        lineRenderer = mtransform.GetComponent<LineRenderer>();
        trackedObj = mtransform.GetComponent<SteamVR_TrackedObject>();
    }

    public void UpdateRay()
    {

        //forwordEndPoint = mtransform.position + mtransform.forward * distance;

        //SetLine(mtransform.position, forwordEndPoint);

        Ray ray = new Ray(mtransform.position, mtransform.forward);

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            if (triggerHapticPulse && pulseBool)
            {
                //between 100 to 2000ms and settled for 500ms. 
                SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(500);
                pulseBool = false;
            }

            rayTransform = hitInfo.collider.transform;
            if (rayTransform.tag != "Ground")
            {
                lineMaterial.color = Color.green;
                pointPos = hitInfo.point;
                pointTranform.position = hitInfo.point;
            }
            SetLine(mtransform.position, hitInfo.point);

        }
        else
        {
            if (rayTransform)
            {
                if (!pulseBool)
                    pulseBool = true;

                rayTransform = null;
                pointTranform.position = new Vector3(0, 1200, 0);
                lineMaterial.color = Color.red;
            }
            SetLine(mtransform.position, ray.GetPoint(distance));
        }
    }

    void SetLine(Vector3 tranpos, Vector3 forwordEndPoint)
    {
        lineRenderer.SetPosition(0, tranpos);
        lineRenderer.SetPosition(1, forwordEndPoint);
    }

    public void UpdateInput()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);

        //if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        //{
        //    Debug.Log("按了 “trigger” “扳机键”");

        //    //右手震动  
        //    //拉弓类似操作应该就是按住trigger（扳机）gettouch时持续调用震动方法模拟弓弦绷紧的感觉。  
        //    var deviceIndex2 = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        //    SteamVR_Controller.Input(deviceIndex2).TriggerHapticPulse(500);
        //}

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            AppliactionMenuDown();
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            TriggerPressedDown();
        }

        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            TriggerPressedUp();
        }

        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            GrispPressedDown();        
        }

        ////Axis1键  目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis1))
        //{
        //    Debug.Log("按下了 “Axis1” “ ”");
        //}
        ////按动触发   
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis1))
        //{
        //    Debug.Log("用press按下了 “Axis1” “ ”");
        //}

        ////Axis2键 目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis2))
        //{
        //    Debug.Log("按下了 “Axis2” “ ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis2))
        //{
        //    Debug.Log("用press按下了 “Axis2” “ ”");
        //}

        ////Axis3键  未目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis3))
        //{
        //    Debug.Log("按下了 “Axis3” “ ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis3))
        //{
        //    Debug.Log("用press按下了 “Axis3” “ ”");
        //}

        ////Axis4键  目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis4))
        //{
        //    Debug.Log("按下了 “Axis4” “ ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis4))
        //{
        //    Debug.Log("用press按下了 “Axis4” “ ”");
        //}

        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis0))
        //{
        //    Debug.Log("按下了 “Axis0” “方向 ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis0))
        //{
        //    Debug.Log("用press按下了 “Axis0” “方向 ”");
        //}

        ////Axis1键  目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis1))
        //{
        //    Debug.Log("按下了 “Axis1” “ ”");
        //}
        ////按动触发   
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis1))
        //{
        //    Debug.Log("用press按下了 “Axis1” “ ”");
        //}

        ////Axis2键 目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis2))
        //{
        //    Debug.Log("按下了 “Axis2” “ ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis2))
        //{
        //    Debug.Log("用press按下了 “Axis2” “ ”");
        //}

        ////Axis3键  未目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis3))
        //{
        //    Debug.Log("按下了 “Axis3” “ ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis3))
        //{
        //    Debug.Log("用press按下了 “Axis3” “ ”");
        //}

        ////Axis4键  目前未发现按键位置  
        ////触摸触发  
        //if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Axis4))
        //{
        //    Debug.Log("按下了 “Axis4” “ ”");
        //}
        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Axis4))
        //{
        //    Debug.Log("用press按下了 “Axis4” “ ”");
        //}

        ////按动触发  
        //if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        //{
        //    Debug.Log("用press按下了 “Touchpad” “ ”");
        //}


        //ATouchpad键 圆盘交互  
        //触摸触发  
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            //Debug.Log("按下了 “Touchpad” “ ”");

            //方法返回一个坐标 接触圆盘位置  
            Vector2 pos = device.GetAxis();
            //Debug.Log(pos);
            // 例子：圆盘分成上下左右

            float angles = VectorAngle(new Vector2(1, 0), pos);
            //Debug.Log(jiaodu);
            //下  
            if (angles > 45 && angles < 135)
            {
                Debug.Log("下");
            }
            //上  
            if (angles < -45 && angles > -135)
            {
                Debug.Log("上");
            }
            //左  
            if ((angles < 180 && angles > 135) || (angles < -135 && angles > -180))
            {
                Debug.Log("左");
            }
            //右  
            if ((angles > 0 && angles < 45) || (angles > -45 && angles < 0))
            {
                Debug.Log("右");
            }
        }

    }

    public virtual void AppliactionMenuDown()
    {
        Debug.Log("菜单按钮");
    }

    public virtual void TriggerPressedDown()
    {
        Debug.Log("扳机 down");
    }

    public virtual void TriggerPressedUp()
    {
        Debug.Log("扳机 up");
    }

    public virtual void GrispPressedDown()
    {
        Debug.Log("手柄按钮");
    }

    //方向圆盘最好配合这个使用 圆盘的.GetAxis()会检测返回一个二位向量，可用角度划分圆盘按键数量  
    //这个函数输入两个二维向量会返回一个夹角 180 到 -180  
    float VectorAngle(Vector2 from, Vector2 to)
    {
        float angle;
        Vector3 cross = Vector3.Cross(from, to);
        angle = Vector2.Angle(from, to);
        return cross.z > 0 ? -angle : angle;
    }

}
