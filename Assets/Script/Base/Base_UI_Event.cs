using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base_UI_Event : Singleton<Base_UI_Event>
{
    /// <summary>
    /// 用于标示当前方向
    /// </summary>
    protected Transform orientionTrans;

    /// <summary>
    /// 当前的菜单
    /// </summary>
    [HideInInspector]
    protected GameObject curMenuCanvas;

    /// <summary>
    /// 菜单键是否按下
    /// </summary>
    [HideInInspector]
    public bool isMenuBtnClick;

    /// <summary>
    /// 视频列表菜单
    /// </summary>
    public GameObject videoMenuCanvas;

    /// <summary>
    /// 视频播放菜单
    /// </summary>
    public GameObject videoControlCanvas;

    /// <summary>
    /// 头盔
    /// </summary>
    private GameObject HeadSet;

    /// <summary>
    /// 是否更新菜单位置
    /// </summary>
    private bool IsUpdateCanvasPos;

    
    /// <summary>
    /// 重置菜单位置
    /// </summary>
    void ReSetMenuPos()
    {
        Vector3 delta = HeadSet.transform.position + HeadSet.transform.forward * 2f;
        delta.y = HeadSet.transform.position.y - 0.25f;
        orientionTrans.transform.position = delta;
        orientionTrans.transform.rotation = new Quaternion(0, HeadSet.transform.rotation.y, 0, HeadSet.transform.rotation.w);
        IsUpdateCanvasPos = true;
    }


    /// <summary>
    /// 跟新菜单位置到临时变量
    /// </summary>
    void UpdateCanvasPos()
    {
        if (IsUpdateCanvasPos)
        {
            curMenuCanvas.transform.position = orientionTrans.position;
            curMenuCanvas.transform.rotation = orientionTrans.rotation;
            IsUpdateCanvasPos = false;
            curMenuCanvas.SetActive(!curMenuCanvas.activeInHierarchy);
        }
    }

    /// <summary>
    /// 更新函数
    /// </summary>
    protected void UpdateFunction()
    {
        UpdateCanvasPos();
        if (isMenuBtnClick)
        {
            ReSetMenuPos();
            isMenuBtnClick = false;
        }
    }

    /// <summary>
    /// 唤醒函数
    /// </summary>
    private void Awake()
    {
        HeadSet = DeviceFinder.HeadsetCamera().gameObject;
        GameObject go = new GameObject("Temp");

        orientionTrans = go.transform;
        curMenuCanvas = videoMenuCanvas;
    }


}//End Class
