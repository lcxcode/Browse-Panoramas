using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SceneManage : MonoBehaviour
{

    public static SceneManage Instance;

   // [HideInInspector]
    public int tipIndex = 0;

    public bool isPressed;

    public bool isRealease;

    public GameObject selectedBtn;

    public PointerEventData eventData;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance == null)
        {
            Instance = this;
        }
    }

}
