using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// enum  
/// </summary>
public enum ConfirmType  //enum the type
{
    Pos = 0,   
    Scene,    
    Model,    
    Active,   
    InActive   
}

public class TriggerConfirm : MonoBehaviour
{

    public static TriggerConfirm Instance;

    //to manage the positions of the scene
    public Transform posManage;

    Dictionary<int, Transform> position = new Dictionary<int, Transform>();

    /// <summary>
    /// private avoid that somewhere to new this class
    /// </summary>
    private TriggerConfirm()   // avoid that somewhere to new this class
    {
    }

    /// <summary>
    /// Awake   init the properties 
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

       for (int i = 0; i < posManage.childCount; i++)
       {
           position.Add(i, posManage.GetChild(i).transform);
       }   
    }

    /// <summary>
    /// MenuConfirm
    /// </summary>
    /// <param name="hitTrans"></param>
    /// <param name="type"></param>
    /// <param name="player"></param>
	public void MenuConfirm(Transform hitTrans, ConfirmType type, Transform player = null)   // confirmed by Menu panel
    {
        switch(type)                                         // do something by param's type
        {
            case ConfirmType.Scene:
                LoadScene(hitTrans, player);
                break;
            case ConfirmType.Pos:
                ChangePos(hitTrans, player);
                break;
            case ConfirmType.Model:
                LoadModel(hitTrans, player);
                break;
            case ConfirmType.Active:
                ActiveComponent(hitTrans, player);
                break;
        }
    }

    /// <summary>
    /// LoadConfirm
    /// </summary>
    /// <param name="hitTrans"></param>
    /// <param name="player"></param>
    public void LoadConfirm(Transform hitTrans, Transform player )   
    {
    }

    /// <summary>
    /// ChangePos
    /// </summary>
    /// <param name="targetTrans"></param>
    /// <param name="player"></param>
    private void ChangePos(Transform targetTrans, Transform player)   //change the player's position and rotation when pushdown the trigger button
    {

        if(targetTrans.parent.name == "Menu" && position.ContainsKey(targetTrans.GetSiblingIndex()))      //if the position contain the index ID of the menu hitTrans  change the position etc  并不好没有唯一标识
        {
            player.position = position[targetTrans.GetSiblingIndex()].position;
            player.rotation = position[targetTrans.GetSiblingIndex()].rotation;    
        }
    }

    /// <summary>
    /// LoadScene
    /// </summary>
    /// <param name="targetTrans"></param>
    /// <param name="player"></param>
    private void LoadScene(Transform targetTrans, Transform player)                                  //load scene by param's name
    {
        switch (targetTrans.name)
        {
            case "YongLing":
                SteamVR_LoadLevel.Begin("YongLing", false, 1, 1, 1, 1, 1);
             
                break;
            case "JinSha":
                SteamVR_LoadLevel.Begin("JinSha", false, 1, 1, 1, 1, 1);
                
                break;
            case "ZhuYueLian":
                SteamVR_LoadLevel.Begin("ZhuYueLian", false, 1, 1, 1, 1, 1);
               
                break;
            default:
                break;
        }
       
        //SteamVR_LoadLevel.Begin(targetTrans.name,false,5,1,1,1,1);
    }

    /// <summary>
    /// LoadModel
    /// </summary>
    /// <param name="targetTrans"></param>
    /// <param name="player"></param>
    private void LoadModel(Transform targetTrans, Transform player)  //load model
    {

    }

    /// <summary>
    /// ActiveComponent
    /// </summary>
    /// <param name="targetTrans"></param>
    /// <param name="player"></param>
    private void ActiveComponent(Transform targetTrans, Transform player)  //active the component of player (param)
    {
        //player.GetComponent<BirdEye>().enabled = true;
        targetTrans.parent.gameObject.SetActive(false);
    }

}//End Class TriggerConfirm
