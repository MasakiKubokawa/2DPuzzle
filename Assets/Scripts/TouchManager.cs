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

    private static TouchManager Instance
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

    private event System.Action<TouchManager> _began;
    private event System.Action<TouchManager> _moved;
    private event System.Action<TouchManager> _ended;

    //when touch
    public static event System.Action<TouchManager> Began
    {
        add
        {
            Instance._began += value;
        }
        remove
        {
            Instance._began -= value;
        }
    }

    //during touch
    public static event System.Action<TouchManager> Moved
    {
        add
        {
            Instance._moved += value;
        }
        remove
        {
            Instance._moved -= value;
        }
    }

    //remove touch
    public static event System.Action<TouchManager> Ended
    {
        add
        {
            Instance._ended += value;
        }
        remove
        {
            Instance._ended -= value;
        }
    }

    //current touch state
    private TouchState State
    {
        get
        {
#if IS_EDITOR
            //EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                return TouchState.Began;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                return TouchState.Moved;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                return TouchState.Ended;
            }
#else
            //exclude EDITOR
            if (Input.touchCount > 0)
            {
                switch (Input.GetTouch(0).phase)
                {
                    case TouchPhase.Began:
                        return TouchState.Began;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        return TouchState.Moved;
                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        return TouchState.Ended;
                    default:
                        break;
                }
            }
#endif
            return TouchState.None;
        }


    //touch state
    private enum TouchState
    {
        None = 0, //no touch
        Began = 1, //began touch
        Moved = 2, //during touch
        Ended = 3, //end touch
    }

}
