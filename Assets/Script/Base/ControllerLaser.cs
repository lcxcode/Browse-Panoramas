using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class ControllerLaser : MonoBehaviour
{
    /// <summary>
    /// 射线
    /// </summary>
    private Ray ray;

    /// <summary>
    /// 碰到的物体
    /// </summary>
    private RaycastHit hitInfo;

    /// <summary>
    /// 画线
    /// </summary>
    private LineRenderer lineRenderer;

    /// <summary>
    /// 射线的长度
    /// </summary>
    [Range(10,100)]
    public int distance = 30;

    /// <summary>
    /// 射线的材质
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// 提示标志
    /// </summary>
    [HideInInspector]
    public Transform HitTipObject;

    // Use this for initialization
    void Start ()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        //GameObject go = Resources.Load<GameObject>("Prefabs/TipObject");
        // HitTipObject =  Instantiate(go, Vector3.zero, Quaternion.identity).transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateRay();
    }

    /// <summary>
    /// 更新射线
    /// </summary>
    public void UpdateRay()
    {
         ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            SetLine(transform.position, hitInfo.point);
            //HitTipObject.position = hitInfo.point;
            //HitTipObject.rotation = hitInfo.transform.rotation;
            lineMaterial.color = Color.green;
        }
        else
        {
            SetLine(transform.position, ray.GetPoint(distance));
            //HitTipObject.position = Vector3.back * 1000;
            lineMaterial.color = Color.red;
        }
    }


    /// <summary>
    /// 画线
    /// </summary>
    /// <param name="tranpos"></param>
    /// <param name="forwordEndPoint"></param>
    void SetLine(Vector3 tranpos, Vector3 forwordEndPoint)
    {
        lineRenderer.SetPosition(0, tranpos);
        lineRenderer.SetPosition(1, forwordEndPoint);
    }

}//End Class
