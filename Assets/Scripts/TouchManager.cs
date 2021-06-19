#if UNITY_EDITOR
#define IS_EDITOR
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TouchManager : MonoBehaviour
{
    //singleton
    private static TouchManager _instance;

    public static TouchManager Instance
    {
        get
        {
            if (null == _instance)
            {
                var obj = new GameObject(typeof(TouchManager).Name);
                _instance = obj.AddComponent<TouchManager>();
                if (null == _instance)
                {
                    Debug.Log(" TouchManager Instance Error ");
                }
            }
            return _instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
