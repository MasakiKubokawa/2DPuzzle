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

    private TouchInfo _info = new TouchInfo();
    private event System.Action<TouchInfo> _began;
    private event System.Action<TouchInfo> _moved;
    private event System.Action<TouchInfo> _ended;

    //when touch
    public static event System.Action<TouchInfo> Began
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
    public static event System.Action<TouchInfo> Moved
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
    public static event System.Action<TouchInfo> Ended
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
            else if (Input.GetMouseButton(0))
            {
                return TouchState.Moved;
            }
            else if (Input.GetMouseButtonUp(0))
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
    }

    //touch position
    private Vector2 Position
    {
        get
        {
#if IS_EDITOR
            return State == TouchState.None ? Vector2.zero : (Vector2)Input.mousePosition; //(条件式) ? (真の場合):(偽の場合) 
#else
            return Input.GetTouch(0).position;
#endif
        }
    }

    private void Update()
    {
        if (State == TouchState.Began)
        {
            _info.screenPoint = Position;
            _info.deltaScreenPoint = Vector2.zero;
            if(_began != null)
            {
                _began(_info);
            }
        }
        else if (State == TouchState.Moved)
        {
            _info.deltaScreenPoint = Position - _info.screenPoint;
            _info.screenPoint = Position;
            if(_moved != null)
            {
                _moved(_info);
            }
        }
        else if (State == TouchState.Ended)
        {
            _info.deltaScreenPoint = Position - _info.deltaScreenPoint;
            _info.screenPoint = Position;
            if (_ended != null)
            {
                _ended(_info);
            }
        }
        else
        {
            _info.screenPoint = Vector2.zero;
            _info.deltaScreenPoint = Vector2.zero;
        }
    }
}

//touch information
public class TouchInfo
{
    //touched position on screen
    public Vector2 screenPoint;
    //difference btw touched position on screen 1 frame befor and now
    public Vector2 deltaScreenPoint;
    //touched viewport position
    public Vector2 ViewPoint
    {
        get
        {
            _viewPoint.x = screenPoint.x / Screen.width;
            _viewPoint.y = screenPoint.y / Screen.height;
            return _viewPoint;
        }
    }
    public Vector2 DeltaViewPoint
    {
        get
        {
            _deltaViewPoint.x = deltaScreenPoint.x / Screen.width;
            _deltaViewPoint.y = deltaScreenPoint.y / Screen.height;
            return _deltaViewPoint;
        }
    }
    //difference btw touched viewport position 1 frame befor and now

    private Vector2 _viewPoint = Vector2.zero;
    private Vector2 _deltaViewPoint = Vector2.zero;
}

    //touch state
public enum TouchState
{
    None = 0, //no touch
    Began = 1, //began touch
    Moved = 2, //during touch
    Ended = 3, //end touch
}

