using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
                if(_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    go.hideFlags = HideFlags.HideAndDontSave;
                    _instance = go.AddComponent<T>();
                }          
            }
            return _instance;
        }
    }


    //void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}



}//End Class
