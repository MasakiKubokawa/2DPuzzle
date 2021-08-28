using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TouchManager.Began += (info) =>
        {
            Debug.Log("ボタンが押されました！" + info.screenPoint);
        };
        TouchManager.Ended += (info) => 
        {
            Debug.Log("ボタンが離れました！" + info.screenPoint);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
