using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stroke : MonoBehaviour
{

    public static Stroke Instance;                             

    public Transform MenuPanel;

    [HideInInspector]
    public bool IsFlying;

    private Stroke()
    {
    }

    private Material lastMaterial;

    private Material currentMaterial;

    private Dictionary<string, Transform> MenuItem = new Dictionary<string, Transform>();  //dictionary save info of menupanel

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        //for(int i = 0; i < MenuPanel.childCount; i++)                  //when application  awake  , set the value of MenuItem
        //{
        //    MenuItem.Add(MenuPanel.GetChild(i).name, MenuPanel.GetChild(i).transform);
        //}
        IsFlying = false;   //如果要设置飞行，将该值设置为true
    }

    public void AddItemToDic()
    {
        MenuItem.Clear();
        for (int i = 0; i < MenuPanel.childCount; i++)                  //when application  awake  , set the value of MenuItem
        {
            MenuItem.Add(MenuPanel.GetChild(i).name, MenuPanel.GetChild(i).transform);
        }
    }


    public void ChangeOutline(Transform hitTrans)                  //we can change the material of the borderMesh
    {
        if (MenuItem.ContainsKey(hitTrans.name))
        {
            lastMaterial = currentMaterial;
            currentMaterial = hitTrans.GetComponent<MeshRenderer>().materials[1];
        }
        if(lastMaterial)                                                              //set material of the last menu's borderMesh  color = white
        {
            lastMaterial.color = Color.white;
        }
        if(currentMaterial)                                                        //set  material of  the choosed menu's borderMesh  color = red 
        {
            currentMaterial.color = Color.red;
        }

    }

    public void HideOrShowMenu(GameObject go)
    {
        if(!IsFlying)                                                                 // if camera is not flying in the air , we can show or hide the menu panel
        {
           go.SetActive(!go.activeSelf );
        }
    }

    public void HideMenu(GameObject go)                         //if camera is not flying in the air , we can hide the menu panel
    {
        if (!IsFlying)
        {
            go.SetActive( false);
        }
    }

}//
