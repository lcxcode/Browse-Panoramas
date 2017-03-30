using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
//using System.Collections.Generic;

public class ClientData
{
    public enum GameState
    {
        idel,
        start,
        end
    }

    /// <summary>
    /// http://101.201.148.166/
    /// </summary>
    public string mainIP = "http://101.201.148.166/";

    /// <summary>路径
    /// Application.persistentDataPath + "/"
    /// </summary>
    public string loacPathStr 
    {
        get {
            return GetDataPath() + "/";
        }
    }

    /// <summary>本地 账号写入路径
    /// 本地路径+手机号 + 斜杠(/)
    /// </summary>
    public string loacUserPathStr;

    //获取路径/
    private string GetDataPath()
    {
        return Application.persistentDataPath;
    }

    /// <summary>
    /// 打开场景文件
    /// </summary>
    /// <param name="leveName"></param>
    public void OpenScene(string leveName)
    {
        //Application.LoadLevelAsync(leveName);
        SceneManager.LoadSceneAsync(leveName);
    }

	private Transform root;
	private Transform find;

    private GameObject uiRoot;
    private Transform findUI;

//    public GameObject GetUIRoot() 
//    {
//        if (uiRoot == null)
//        {
//            uiRoot = GameObject.Find("UI Root");
//        }
//        return uiRoot;
//    }
//
//    public GameObject UIRoot
//    {
//        get
//        {
//            if (!uiRoot)
//            {
//                uiRoot = GameObject.Find("UI Root");
//            }
//            return uiRoot;
//        }
//    }

	public Transform RootFind(string path) 
	{
		if (root)
		{
			find = root.Find(path);
		}
		else 
		{
			root = GameObject.Find("ROOT").transform;
			find = root.Find(path);
		}  
		return find;
	}

    public Transform UIFind(string path) 
    {
        if (uiRoot)
        {
            findUI = uiRoot.transform.Find(path);
        }
        else 
        {
            uiRoot = GameObject.Find("UI Root");
            findUI = uiRoot.transform.Find(path);
        }  
        return findUI;
    }

    public void TweenBackScaleAndPosition(Transform view)
    {
//        view.GetComponent<TweenScale>().PlayReverse();
//        view.GetComponent<TweenPosition>().PlayReverse();
    }

    public GameState gameStata = GameState.idel;

    public bool onTriggerEnterBool;

}
