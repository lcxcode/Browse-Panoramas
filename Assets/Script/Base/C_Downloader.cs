//通过IO口加载本地的资源，注意出入的文件路径

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class C_Downloader : MonoBehaviour
{

    public static C_Downloader Instance;


    public static Texture2D LoadByIO(string strFileName)
    {
        FileStream fileStream;

        fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        int width = 4096;
        int height = 2048;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);
        return texture;
    }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    //private void Start()
    //{
    //    strFilePath =/* "file:///" */ Application.dataPath + "/StreamingAssets/";// "E:\\2016素材\\项目资料\\素材_东北抗联\\资源文件\\全景图片\\陈瀚章\\殉国，敦化烈士纪念碑和陵园\\";
    //}


}//End Class C_Downloader
