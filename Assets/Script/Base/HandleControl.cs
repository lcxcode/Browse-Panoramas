using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using CurvedUI;
using UnityEngine.EventSystems;

public class HandleControl : VIVERay
{

    [Tooltip("ROOT")]
    public GameObject Root;
    [Tooltip("[CameraRig]")]
    public GameObject Player;
    [Tooltip("菜单界面")]
    public GameObject canvas;
    [Tooltip("初始菜单界面")]
    public GameObject startCanvas;
    [Tooltip("头盔Camera (head)")]
    public GameObject HeadCam;
    [Tooltip("用于确定菜单界面的位置")]
    public GameObject orietion;


    [Tooltip("手柄提示")]
    public GameObject helpMes;
    [Tooltip("手柄菜单按钮位置")]
    public Transform button;
    [Tooltip("手柄扳机按钮位置")]
    public Transform trigger;
    [Tooltip("button提示位置")]
    public Transform btnMes;
    [Tooltip("trigger提示位置")]
    public Transform triMes;

    public GameObject Settings;
    //获取的选中的Button
    [HideInInspector]
    public Transform btnTrans;

    public PointerEventData eventData;

    //draw line from button to btnMes
    private LineRenderer btnLine;
    //draw line from trigger to triMes
    private LineRenderer triLine;

    //字典存储需要替换的全景图
    private Dictionary<string, Texture> Images = new Dictionary<string, Texture>();
    //加载图片到数组
    private Texture[] texs;

    /// <summary>
    /// menu used in project
    /// </summary>
    private GameObject menuCanvas;

    /// <summary>
    /// 是否刷新当前界面的位置
    /// </summary>
    bool IsUpdatePos;


    // Use this for initialization
    void Start()
    {
        Init();
        menuCanvas = canvas;
        IsUpdatePos = true;
        texs = Resources.LoadAll<Texture>("sphere");
        for (int i = 0; i < texs.Length; i++)
        {
            Images.Add(texs[i].name, texs[i]);
        }

        btnLine = button.GetComponent<LineRenderer>();
        triLine = trigger.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRay();
        UpdateInput();
        if(helpMes.activeSelf)
        {
            DrawMesLine();
        }
    }

    void FixedUpdate()
    {      
        UpdateMenuPos();
        UpdateSettingPos();
    }


    public override void AppliactionMenuDown()
    {
        ReSetMenuPos();
        menuCanvas.SetActive(menuCanvas.activeSelf ? false : true);
    }

    //选中了的Button
    Transform oldBtn;

    public override void TriggerPressedDown()
    {

    }

    public override void GrispPressedDown()
    {
        //helpMes.SetActive(!helpMes.activeSelf);
        //if (!Settings.activeInHierarchy)
            ReSetSettingPos();
        Settings.SetActive(!Settings.activeInHierarchy);
    }

    bool IsUpdateSettingPos;
    void UpdateSettingPos()
    {
        if (IsUpdateSettingPos)
        {
            Settings.transform.position = orietion.transform.position;
            Settings.transform.rotation = orietion.transform.rotation;
            IsUpdateSettingPos = false;
        }
    }

    /// <summary>
    /// 刷新菜单界面的位置
    /// </summary>
    void UpdateMenuPos()
    {
        if (IsUpdatePos)
        {
            menuCanvas.transform.position = orietion.transform.position;
            menuCanvas.transform.rotation = orietion.transform.rotation;
            IsUpdatePos = false;
        }
    }

    void ReSetSettingPos()
    {
        Vector3 delta = HeadCam.transform.position + HeadCam.transform.forward * 3;
        delta.y = HeadCam.transform.position.y - 0.35f;
        orietion.transform.position = delta;
        orietion.transform.rotation = new Quaternion(0, HeadCam.transform.rotation.y, 0, HeadCam.transform.rotation.w);
        IsUpdateSettingPos = true;
    }

    /// <summary>
    /// 重置菜单界面位置  ,头盔正前方*米，高度为头盔高度减去0.5（看情况调整）
    /// </summary>
    void ReSetMenuPos()
    {
        Vector3 delta = HeadCam.transform.position + HeadCam.transform.forward * 3;
        delta.y = HeadCam.transform.position.y - 0.35f;
        orietion.transform.position = delta;
        orietion.transform.rotation = new Quaternion(0, HeadCam.transform.rotation.y, 0, HeadCam.transform.rotation.w);

        IsUpdatePos = true;
    }

   /// <summary>
   /// draw line to help player understand the message in the image and how to operate by handle
   /// </summary>
    void DrawMesLine()
    {
        btnLine.SetPosition(0, button.position);
        btnLine.SetPosition(1, btnMes.position);
        triLine.SetPosition(0, trigger.position);
        triLine.SetPosition(1, triMes.position);
    }


}
