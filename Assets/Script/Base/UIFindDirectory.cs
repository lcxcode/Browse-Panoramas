using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Drawing;



public class UIFindDirectory : MonoBehaviour
{
    //存储按钮
    private List<GameObject> btnListPool = new List<GameObject>();

    private List<string> listHistoricalPath = new List<string>();

    //下载预制体存储
    private GameObject btnItem;

    //找寻文件的类
    protected FindDirectory _findDirectory;

    /// <summary>
    /// 全景图
    /// </summary>
    public GameObject cubemap;
    
    /// <summary>
    /// 按钮的父节点
    /// </summary>
    public Transform transP;

    /// <summary>
    /// 当前类的实例
    /// </summary>
    public static UIFindDirectory Instance;

    [HideInInspector]
    public GameObject ClickedObject;

    /// <summary>
    /// 显示路径信息
    /// </summary>
    public Text PathText;



    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }


    // Use this for initialization  
    void Start()
    {
        btnItem = Resources.Load<GameObject>("Prefabs/Button");

        FindDirectory.Instance.CurrentDirectoryPath = "E:/";

        Find();
    }



    /// <summary>
    /// 全局临时变量
    /// </summary>
    string strTempName;


    /// <summary>
    /// 按钮点击事件
    /// </summary>
    /// <param name="obj"></param>
    public void Clicked(GameObject obj)
    {
        switch (obj.name)
        {
            case "desktop":
                {
                    strTempName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    BtnClicked(strTempName);
                }

                break;
            case "picture":
                {
                    strTempName = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    BtnClicked(strTempName);
                }

                break;
            case "D":
                {
                    strTempName = "D:/";
                    BtnClicked(strTempName);
                }

                break;
            case "E":
                {
                    strTempName = "E:/";
                    BtnClicked(strTempName);
                }
     
                break;
            case "F":
                {
                    strTempName = "F:/";
                    BtnClicked(strTempName);
                }
          
                break;
            case "return":
                {
                    if(listHistoricalPath.Count > 0)
                    {
                        strTempName = listHistoricalPath[listHistoricalPath.Count - 1];
                        listHistoricalPath.RemoveAt(listHistoricalPath.Count - 1);
                    }
                    else
                    {
                        strTempName = null;
                    }
                    ReturnBtnClick(strTempName);
                }      
                break;
        }

    }


    /// <summary>
    /// 根据路径执行查找
    /// </summary>
    /// <param name="filePath"></param>
    void BtnClicked(string filePath)
    {

        strTempName = null;
        _directoryName = null;

        if(!listHistoricalPath.Contains(FindDirectory.Instance.CurrentDirectoryPath))
        {
            listHistoricalPath.Add(FindDirectory.Instance.CurrentDirectoryPath);
        }
  
        FindDirectory.Instance.CurrentDirectoryPath = filePath;

      Find();
    }

    /// <summary>
    /// 返回按键点击事件
    /// </summary>
    /// <param name="filePath"></param>
    void ReturnBtnClick(string filePath)
    {
        if(filePath == null)
        {
            return;
        }
        strTempName = null;
        _directoryName = null;
        FindDirectory.Instance.CurrentDirectoryPath = filePath;
        Find();
    }

    /// <summary>
    /// 存储文件名称
    /// </summary>
    string[] _directoryName;

    /// <summary>
    /// 按路径查找文件
    /// </summary>
    void Find()
    {
        PathText.text = FindDirectory.Instance.CurrentDirectoryPath;

        //清空当前界面内容
        for (int i = 0; i < transP.childCount; i++)
        {
            btnListPool.Add(transP.GetChild(i).gameObject);
            transP.GetChild(i).gameObject.SetActive(false);
        }

        //执行查找
        FindDirectory.Instance.Find();

      StartCoroutine(WaitForFound());

        #region 
        ////获取查找到的文件的名称
        // _directoryName = _findDirectory.AllFileName;

        //if (_directoryName == null)
        //{
        //    Debug.Log("没找到===");
        //    return;
        //}

        ////生成当前查找到的界面
        //foreach (string s in _directoryName)
        //{

        //    GameObject btn;
        //    //Debug.Log(s);

        //    //生成Button,根据对象池
        //    if (btnListPool.Count > 0)
        //    {
        //        btn = btnListPool[0];
        //        btn.gameObject.SetActive(true);
        //        btnListPool.Remove(btnListPool[0]);
        //    }
        //    else
        //    {
        //        btn = Instantiate(btnItem, transP.position, transP.rotation) as GameObject;

        //    }
        //    //设置Button的属性
        //    btn.name = s;
        //    //设置按钮的父节点
        //    btn.transform.SetParent(transP);
        //    //设置按钮的位置
        //    Vector3 temp = btn.gameObject.GetComponent<RectTransform>().localPosition;
        //    temp.z = 0;
        //    btn.gameObject.GetComponent<RectTransform>().localPosition = temp;
        //    //设置按钮的缩放
        //    btn.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        //    //设置文件的名称
        //    btn.transform.FindChild("Text").GetComponent<Text>().text = s;
        //    //绑定点击事件到按钮上
        //    btn.GetComponent<Button>().onClick.AddListener(delegate { Load(btn.gameObject); });
        //    //设置显示文件的ICON图标=======未解决
        //    //btn.GetComponent<UnityEngine.UI.Image>().material.mainTexture = LoadIcon(_findDirectory.DicName_Path[s]);
        //    Debug.Log(_findDirectory.DicName_Path[s]);
        //}
        #endregion
    }

    /// <summary>
    /// 协同，等待查找完成
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForFound()
    {
        yield return new WaitForEndOfFrame();

        //获取查找到的文件的名称
        _directoryName = FindDirectory.Instance.AllFileName;

        while (_directoryName == null)
        {
            _directoryName = FindDirectory.Instance.AllFileName;
            yield return null;
        }

        //生成当前查找到的界面
        foreach (string s in _directoryName)
        {

            //生成Button,根据对象池
            if (btnListPool.Count  >  0)
            {
                GameObject btnTemp;
                btnTemp = btnListPool[0];
                btnListPool.Remove(btnListPool[0]);
                btnTemp.SetActive(true);
                btnTemp.name = s;

                btnTemp.transform.FindChild("Text").GetComponent<Text>().text = s;
                //绑定点击事件到按钮上
                btnTemp.GetComponent<Button>().onClick.RemoveAllListeners();
                btnTemp.GetComponent<Button>().onClick.AddListener(delegate { Load(); });
            }
            else
            {
                GameObject btn;
                btn = Instantiate(btnItem, transP.position, transP.rotation) as GameObject;

                //设置Button的属性
                btn.name = s;
                //设置按钮的父节点
                btn.transform.SetParent(transP);
                //设置按钮的位置
                Vector3 temp = btn.gameObject.GetComponent<RectTransform>().localPosition;
                temp.z = 0;
                btn.gameObject.GetComponent<RectTransform>().localPosition = temp;
                //设置按钮的缩放
                btn.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
                //设置文件的名称
                btn.transform.FindChild("Text").GetComponent<Text>().text = s;
                //绑定点击事件到按钮上
                btn.GetComponent<Button>().onClick.RemoveAllListeners();
                btn.GetComponent<Button>().onClick.AddListener(delegate { Load(); });
            }

            #region
            ////设置Button的属性
            //btn.name = s;
            ////设置按钮的父节点
            //btn.transform.SetParent(transP);
            ////设置按钮的位置
            //Vector3 temp = btn.gameObject.GetComponent<RectTransform>().localPosition;
            //temp.z = 0;
            //btn.gameObject.GetComponent<RectTransform>().localPosition = temp;
            ////设置按钮的缩放
            //btn.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
            ////设置文件的名称
            //btn.transform.FindChild("Text").GetComponent<Text>().text = s;
            ////绑定点击事件到按钮上
            //btn.GetComponent<Button>().onClick.RemoveAllListeners();
            //btn.GetComponent<Button>().onClick.AddListener(delegate { Load(btn.gameObject); });
            //设置显示文件的ICON图标=======未解决
            //btn.GetComponent<UnityEngine.UI.Image>().material.mainTexture = LoadIcon(_findDirectory.DicName_Path[s]);
            //Debug.Log(_findDirectory.DicName_Path[s]);
            #endregion
        }
    }

    //子目录的点击事件
    public void Load()
    {
        if(ClickedObject == null)
        {
            return;
        }

        //判断是否是文件夹
        if (FindDirectory.Instance.DicDirectory.ContainsKey (ClickedObject.name))     //_findDirectory.DirectoriesName.Contains(go1.name))
        {
            //打开文件夹
            BtnClicked(FindDirectory.Instance.DicDirectory[ClickedObject.name]);     //_findDirectory.DicName_Path[go1.name]);
        }

        if(FindDirectory.Instance.DicFile.ContainsKey(ClickedObject.name))
        {
            //加载图片
            StartCoroutine(WaitLoadDone(FindDirectory.Instance.DicFile[ClickedObject.name] ));
        }
    }


    //加载图片的协程
    IEnumerator WaitLoadDone(string strImagePath)
    {

        yield return new WaitForEndOfFrame();

        Texture tempTex = C_Downloader.LoadByIO(strImagePath);

        while(tempTex == null)
        {
            yield return null;
        }

        cubemap.GetComponent<Renderer>().material.mainTexture = tempTex;

    }


    #region Load ICON
    Texture2D LoadIcon(string strPath)
    {
        Texture2D image = null;
        StartCoroutine(LoadIconDone(strPath, image));
        return image;
    }


    IEnumerator LoadIconDone(string str, Texture2D image)
    {
        yield return new WaitForEndOfFrame();
        Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(str);
        Bitmap bit = icon.ToBitmap();
        MemoryStream ms = new MemoryStream();
        icon.Save(ms);
        byte[] bytes = ms.ToArray();
        Texture2D texture = new Texture2D(bit.Width, bit.Height);
        texture.LoadImage(bytes);
        image = texture;
    }
    #endregion


}//End Class
