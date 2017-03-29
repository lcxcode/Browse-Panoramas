using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

/// <summary>  
/// 获取文件目录以及文件类  
/// </summary>  
public class FindDirectory
{

    #region 私有变量   

    //当前文件目录  
    private string _currentDirectoryPath;

    //当前文件目录下子目录  
    private ArrayList _directoriesPath;
    //对应子目录的名称  
    private string[] _directoryName;

    //当前目录下的文件的路径  
    private ArrayList _filesPath;
    //对应文件的名称  
    private string[] _fileName;

    private string[] _allFileName;   //add by lcx

    private ArrayList _allFilesList;

    //是否找到指定的文件  
    private bool _findFilesOrNot = true;
    #endregion

    private static FindDirectory _instance;

    /// <summary>
    /// 存贮全部文件名称及路径
    /// </summary>
    //public Dictionary<string, string> DicName_Path = new Dictionary<string, string>();

    public Dictionary<string, string> DicDirectory = new Dictionary<string, string>();

    public Dictionary<string, string> DicFile = new Dictionary<string, string>();

    public List<string> DirectoriesName = new List<string>();

    public static FindDirectory Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new FindDirectory();
            }
            return _instance;
        }
    }


    #region Get Set方法  

    /// <summary>  
    /// 获取当前文件目录以及设置  
    /// </summary>  
    /// <value>The current directory path.</value>  
    public string CurrentDirectoryPath
    {
        get { return _currentDirectoryPath; }
        set { _currentDirectoryPath = value; }
    }

    /// <summary>  
    /// 子目录文件夹路径获取  
    /// </summary>  
    /// <value>The directories path found.</value>  
    public string[] DirectoriesPathFound
    {
        get
        {
            //新建存放文件夹路径  
            string[] directories = new string[_directoriesPath.Count];
            //将存放子目录的动态数组复制一份并返回  
            _directoriesPath.CopyTo(directories);

            return directories;
        }
    }

    /// <summary>  
    /// 获取目录的名称  
    /// </summary>  
    /// <value>The directory name found.</value>  
    public string[] DirectoryNameFound
    {
        get { return _directoryName; }
    }

    /// <summary>  
    /// 获取目录下的文件路径  
    /// </summary>  
    /// <value>The files path found.</value>  
    public string[] FilesPathFound
    {
        get
        {
            string[] files = new string[_filesPath.Count];
            _filesPath.CopyTo(files);

            return files;
        }
    }

    /// <summary>  
    /// 获取文件名称  
    /// </summary>  
    /// <value>The file name found.</value>  
    public string[] FileNameFound
    {
        get { return _fileName; }
    }

    public string[] AllFileName
    {
        get
        {
            return _allFileName;
        }
    }
   

    #endregion

    #region 构造函数  

    /// <summary>  
    /// 默认构造函数  
    /// </summary>  
    public FindDirectory()
    {
        _directoriesPath = new ArrayList();
        _filesPath = new ArrayList();
        _allFilesList = new ArrayList();
    }

    /// <summary>  
    /// 含当前文件目录路径的构造函数  
    /// </summary>  
    /// <param name="_currentDirectoryPath">_current directory path.</param>  
    public FindDirectory(string _currentDirectoryPath)
    {
        _directoriesPath = new ArrayList();
        _filesPath = new ArrayList();
        _allFilesList = new ArrayList();
        this._currentDirectoryPath = _currentDirectoryPath;
    }

    #endregion

    #region 公共方法  


    /// <summary>  
    /// 寻找当前目录下的文件  
    /// </summary>  
    public void Find()
    {
        //当前目录路径不是空且存在  
        if (CurrentDirectoryPath != "" || CurrentDirectoryPath != null)
        {
            FindProcess(CurrentDirectoryPath);
        }
        else
        {
            //Debug.Log("找不到当前文件目录");
        }
    }


    #endregion

    #region 私有方法  

    /// <summary>  
    /// 寻找该目录下的内容  
    /// </summary>  
    private void FindProcess(string target)
    {
        try
        {
            //清空上次搜索的全部内容’  
            _directoriesPath.Clear();
            _filesPath.Clear();
            _allFilesList.Clear();
            //DicName_Path.Clear();
            DicDirectory.Clear();
            DicFile.Clear();
            _directoryName = null;
            _fileName = null;
            _allFileName = null;



            //获取目录中子目录的名称和路径  
            string[] subDirectoryEntities = Directory.GetDirectories(target);

            //将该目录下的所有文件夹名称和路径添加进入文件子目录路径  
            foreach (string sub in subDirectoryEntities)
            {
                _directoriesPath.Add(sub);
            }

            //目录名称数组设置为目录路径动态数组的大小  
            _directoryName = new string[_directoriesPath.Count];

            //将文件目录路径直接全部拷贝到文件目录名称中  
            _directoriesPath.CopyTo(_directoryName);

            //对文件目录名称进行修正  
            for (int i = 0; i < _directoryName.Length; i++)
            {
               
                //从字符串末尾获取目录分隔符的位置  
                int endSeparator = _directoryName[i].Trim().LastIndexOf(Path.DirectorySeparatorChar.ToString());

                //DicName_Path.Add(_directoryName[i].Substring(endSeparator + 1).Trim(), _directoryName[i]);
                DicDirectory.Add(_directoryName[i].Substring(endSeparator + 1).Trim(), _directoryName[i]);


                //重组文件目录名称，从目录分割符的下一个开始获取  
                _directoryName[i] = _directoryName[i].Substring(endSeparator + 1).Trim();
                _allFilesList.Add(_directoryName[i]); //===================================================================lcx===========
                DirectoriesName.Add(_directoryName[i]);
            }


            //若获取到文件目录  
            if (_findFilesOrNot)
            {
                //获取目录下的文件路径和名称  
                string[] fileEntities = Directory.GetFiles(target);

                foreach (string f in fileEntities)
                {
                    //将目录下的文件路径添加进入文件路径动态数组中  
                    _filesPath.Add(f);
                }

                //获取文件名称  
                _fileName = new string[_filesPath.Count];
                //将整个文件路径拷贝给文件名称  
                _filesPath.CopyTo(_fileName);

                //对文件名称进行修正  
                for (int i = 0; i < _fileName.Length; i++)
                {
                    //从字符串末尾获取目录分隔符的位置  
                    int endSeparator = _fileName[i].Trim().LastIndexOf(Path.DirectorySeparatorChar.ToString());

                    //DicName_Path.Add(_fileName[i].Substring(endSeparator + 1).Trim(), _fileName[i]);
                    DicFile.Add(_fileName[i].Substring(endSeparator + 1).Trim(), _fileName[i]);

                    //重组文件名称，从目录分割符的下一个开始获取  
                    _fileName[i] = _fileName[i].Substring(endSeparator + 1).Trim();
                    _allFilesList.Add(_fileName[i]);  //==========================================================================lcx=============================
                }
            }

            _allFileName = new string[_allFilesList.Count];
            _allFilesList.CopyTo(_allFileName);

        }
        catch (Exception e)
        {

        }

    }
    #endregion

}