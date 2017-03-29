///
///------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
///------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
///-------------------------------------------------------------------------line:320--在执行点击事件前-将  UIFindDirectory.Instance.ClickedObject = newPressed;----------------------------------------
///------------------------------------------------------------------------------------------------2017-03-29-------------------------------------------------------------------------------------------
///----------------------------------------------------------------------------author -----------lcx---------------------------------------------------------------------------------------------------




using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using CurvedUI;

public class ViveControllerInput : BaseInputModule
{
    //静态实例
    public static ViveControllerInput Instance;

    public Transform keyboard;
    //鼠标图片
    [Header(" [Cursor setup]")]
    public Sprite CursorSprite;
    //鼠标材质
    public Material CursorMaterial;
    //正常的缩放
    public float NormalCursorScale = 0.00025f;

    [Space(10)]

    [Header(" [Runtime variables]")]
    //确定是否有控制器点击了UI
    [Tooltip("Indicates whether or not the gui was hit by any controller this frame")]
    public bool GuiHit;

    //确定这个框架中是否使用了按钮
    [Tooltip("Indicates whether or not a button was used this frame")]
    public bool ButtonUsed;

    //
    [Tooltip("Generated cursors")]
    public RectTransform[] Cursors;

    //当前点击
    private GameObject[] CurrentPoint;
    //当前按下
    private GameObject[] CurrentPressed;
    //当前拖动
    private GameObject[] CurrentDragging;

    //
    private PointerEventData[] PointEvents;

    //是否进行了初始化
    private bool Initialized = false;

    //产生非渲染相机，用来射线触发UI
    [Tooltip("Generated non rendering camera (used for raycasting ui)")]
    public Camera ControllerCamera;

    //VR控制器管理
    private SteamVR_ControllerManager ControllerManager;
    //VR手柄
    private SteamVR_TrackedObject[] Controllers;
    //
    private SteamVR_Controller.Device[] ControllerDevices;

    protected override void Start()
    {
        base.Start();

        if (Initialized == false)
        {
            Instance = this;

            ControllerCamera = new GameObject("Controller UI Camera").AddComponent<Camera>();
            ControllerCamera.clearFlags = CameraClearFlags.Nothing; //CameraClearFlags.Depth;
            ControllerCamera.cullingMask = 0; // 1 << LayerMask.NameToLayer("UI"); 

            ControllerManager = GameObject.FindObjectOfType<SteamVR_ControllerManager>();
            Controllers = new SteamVR_TrackedObject[] { ControllerManager.left.GetComponent<SteamVR_TrackedObject>(), ControllerManager.right.GetComponent<SteamVR_TrackedObject>() };
            ControllerDevices = new SteamVR_Controller.Device[Controllers.Length];
            Cursors = new RectTransform[Controllers.Length];

            for (int index = 0; index < Cursors.Length; index++)
            {
                GameObject cursor = new GameObject("Cursor " + index);
                Canvas canvas = cursor.AddComponent<Canvas>();
                cursor.AddComponent<CanvasRenderer>();
                cursor.AddComponent<CanvasScaler>();
                cursor.AddComponent<UIIgnoreRaycast>();
                cursor.AddComponent<GraphicRaycaster>();

                canvas.renderMode = RenderMode.WorldSpace;
                canvas.sortingOrder = 1000; //set to be on top of everything

                Image image = cursor.AddComponent<Image>();
                image.sprite = CursorSprite;
                image.material = CursorMaterial;


                if (CursorSprite == null)
                    Debug.LogError("Set CursorSprite on " + this.gameObject.name + " to the sprite you want to use as your cursor.", this.gameObject);

                Cursors[index] = cursor.GetComponent<RectTransform>();
            }

            CurrentPoint = new GameObject[Cursors.Length];
            CurrentPressed = new GameObject[Cursors.Length];
            CurrentDragging = new GameObject[Cursors.Length];
            PointEvents = new PointerEventData[Cursors.Length];

            Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
            foreach (Canvas canvas in canvases)
            {
                canvas.worldCamera = ControllerCamera;
            }

            Initialized = true;
        }
    }

    // use screen midpoint as locked pointer location, enabling look location to be the "mouse"
    private bool GetLookPointerEventData(int index)
    {
        if (PointEvents[index] == null)
            PointEvents[index] = new PointerEventData(base.eventSystem);
        else
            PointEvents[index].Reset();

        PointEvents[index].delta = Vector2.zero;
        PointEvents[index].position = new Vector2(Screen.width / 2, Screen.height / 2);
        PointEvents[index].scrollDelta = Vector2.zero;

        base.eventSystem.RaycastAll(PointEvents[index], m_RaycastResultCache);
        PointEvents[index].pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        if (PointEvents[index].pointerCurrentRaycast.gameObject != null)
        {
            GuiHit = true; //gets set to false at the beginning of the process event
        }

        m_RaycastResultCache.Clear();

        return true;
    }

    //当处于激活状态时，刷新鼠标的位置
    // update the cursor location and whether it is enabled
    // this code is based on Unity's DragMe.cs code provided in the UI drag and drop example
    private void UpdateCursor(int index, PointerEventData pointData)
    {
        if (PointEvents[index].pointerCurrentRaycast.gameObject != null)
        {
            //Cursors[index].gameObject.SetActive(true);

            if (pointData.pointerEnter != null)
            {
                RectTransform draggingPlane = pointData.pointerEnter.GetComponent<RectTransform>();
                Vector3 globalLookPos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(draggingPlane, pointData.position, pointData.enterEventCamera, out globalLookPos))
                {
                    Cursors[index].position = globalLookPos;
                    Cursors[index].rotation = draggingPlane.rotation;

                    // scale cursor based on distance to camera
                    float lookPointDistance = (Cursors[index].position - Camera.main.transform.position).magnitude;
                    float cursorScale = lookPointDistance * NormalCursorScale;
                    if (cursorScale < NormalCursorScale)
                    {
                        cursorScale = NormalCursorScale;
                    }

                    Cursors[index].localScale = Vector3.one * cursorScale;
                }
            }
        }
        else
        {
            Cursors[index].gameObject.SetActive(false);
        }
    }

    //清空当前选中
    // clear the current selection
    public void ClearSelection()
    {
        if (base.eventSystem.currentSelectedGameObject)
        {
            base.eventSystem.SetSelectedGameObject(null);
        }
    }

    //选择物体
    // select a game object
    private void Select(GameObject go)
    {
        ClearSelection();

        if (ExecuteEvents.GetEventHandler<ISelectHandler>(go))
        {
            //Debug.Log(go.name);
            base.eventSystem.SetSelectedGameObject(go);
        }
    }

    //刷新选中物体的事件，输入框据此接收键盘输入
    // send update event to selected object
    // needed for InputField to receive keyboard input
    private bool SendUpdateEventToSelectedObject()
    {
        if (base.eventSystem.currentSelectedGameObject == null)
            return false;

        BaseEventData data = GetBaseEventData();

        ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);

        return data.used;
    }

    //更新相机的位置
    private void UpdateCameraPosition(int index)
    {
        ControllerCamera.transform.position = Controllers[index].transform.position;
        ControllerCamera.transform.forward = Controllers[index].transform.forward;
    }

    //初始化控制器
    private void InitializeControllers()
    {
        for (int index = 0; index < Controllers.Length; index++)
        {
            if (Controllers[index] != null && Controllers[index].index != SteamVR_TrackedObject.EIndex.None)
            {
                ControllerDevices[index] = SteamVR_Controller.Input((int)Controllers[index].index);
            }
            else
            {
                ControllerDevices[index] = null;
            }
        }
    }

    //Process被UI系统调用，来实现事件
    // Process is called by UI system to process events
    public override void Process()
    {
        InitializeControllers();

        GuiHit = false;
        ButtonUsed = false;

        //如果有选中的物体，执行事件刷新
        // send update events if there is a selected object - this is important for InputField to receive keyboard events
        SendUpdateEventToSelectedObject();

        //查找是否有整备注视的UI元素
        // see if there is a UI element that is currently being looked at
        for (int index = 0; index < Cursors.Length; index++)
        {
            if (Controllers[index].gameObject.activeInHierarchy == false)
            {
                if (Cursors[index].gameObject.activeInHierarchy == true)
                {
                    Cursors[index].gameObject.SetActive(false);
                }
                continue;
            }

            UpdateCameraPosition(index);

            bool hit = GetLookPointerEventData(index);
            if (hit == false)
                continue;

            CurrentPoint[index] = PointEvents[index].pointerCurrentRaycast.gameObject;

            //处理高亮显示与退出高亮显示
            // handle enter and exit events (highlight)
            base.HandlePointerExitAndEnter(PointEvents[index], CurrentPoint[index]);

            //刷新鼠标位置
            // update cursor
            UpdateCursor(index, PointEvents[index]);

            //控制器不为空
            if (Controllers[index] != null)
            {
                //如果扳机键按下
                if (ButtonDown(index))
                {
                    //清空选中
                    ClearSelection();

                    //
                    PointEvents[index].pressPosition = PointEvents[index].position;
                    PointEvents[index].pointerPressRaycast = PointEvents[index].pointerCurrentRaycast;
                    PointEvents[index].pointerPress = null;

                    if (CurrentPoint[index] != null)
                    {
                        CurrentPressed[index] = CurrentPoint[index];

                        GameObject newPressed = ExecuteEvents.ExecuteHierarchy(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerDownHandler);

                        if (newPressed == null)
                        {
                            // some UI elements might only have click handler and not pointer down handler
                            newPressed = ExecuteEvents.ExecuteHierarchy(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerClickHandler);
                            if (newPressed != null)
                            {
                                CurrentPressed[index] = newPressed;
                            }
                        }
                        else
                        {

                            UIFindDirectory.Instance.ClickedObject = newPressed;

                            CurrentPressed[index] = newPressed;
                            // we want to do click on button down at same time, unlike regular mouse processing
                            // which does click when mouse goes up over same object it went down on
                            // reason to do this is head tracking might be jittery and this makes it easier to click buttons
                            ExecuteEvents.Execute(newPressed, PointEvents[index], ExecuteEvents.pointerClickHandler);
                        }

                        if (newPressed != null)
                        {
                            PointEvents[index].pointerPress = newPressed;
                            CurrentPressed[index] = newPressed;
                            Select(CurrentPressed[index]);
                            ButtonUsed = true;
                        }
                        if(newPressed)
                        {
                            if(newPressed.name == "ShuRu")
                            {
                                keyboard.GetComponent<Canvas>().enabled = true;
                            }
                            //Debug.Log(newPressed.name + "-----------");
                        }
                        
                        ExecuteEvents.Execute(CurrentPressed[index], PointEvents[index], ExecuteEvents.beginDragHandler);
                        PointEvents[index].pointerDrag = CurrentPressed[index];
                        CurrentDragging[index] = CurrentPressed[index];
                    }
                }
                
                //手柄控制器扳机键抬起
                if (ButtonUp(index))
                {
                    if (CurrentDragging[index])
                    {
                        ExecuteEvents.Execute(CurrentDragging[index], PointEvents[index], ExecuteEvents.endDragHandler);
                        if (CurrentPoint[index] != null)
                        {
                            ExecuteEvents.ExecuteHierarchy(CurrentPoint[index], PointEvents[index], ExecuteEvents.dropHandler);
                        }
                        PointEvents[index].pointerDrag = null;
                        CurrentDragging[index] = null;
                    }
                    if (CurrentPressed[index])
                    {
                        ExecuteEvents.Execute(CurrentPressed[index], PointEvents[index], ExecuteEvents.pointerUpHandler);
                        PointEvents[index].rawPointerPress = null;
                        PointEvents[index].pointerPress = null;
                        CurrentPressed[index] = null;
                    }
                }

                // drag handling
                if (CurrentDragging[index] != null)
                {
                    ExecuteEvents.Execute(CurrentDragging[index], PointEvents[index], ExecuteEvents.dragHandler);
                }
            }
        }
    }

    //判断扳机键是否按下
    private bool ButtonDown(int index)
    {
        return (ControllerDevices[index] != null && ControllerDevices[index].GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) == true);
    }

    //判断扳机键是否抬起
    private bool ButtonUp(int index)
    {
        return (ControllerDevices[index] != null && ControllerDevices[index].GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) == true);
    }
}
