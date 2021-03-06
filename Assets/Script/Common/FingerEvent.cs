using UnityEngine;
using System.Collections;
using System;

public class FingerEvent :  MonoBehaviour {

    public static FingerEvent Instance;

    public enum FingerDir
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// 滑动委托
    /// </summary>
    public Action<FingerDir> OnFingerDrag;

    public enum ZoomType
    {
        In,
        Out
    }
    /// <summary>
    /// 缩放的委托
    /// </summary>
    public Action<ZoomType> OnZoom;
    private Vector2 m_TempFinger1Pos;
    private Vector2 m_TempFinger2Pos;

    private Vector2 m_OldFinger1Pos;
    private Vector2 m_OldFinger2Pos;

    /// <summary>
    /// 玩家点击地面的委托
    /// </summary>
    public Action OnPlayerClickGround;
    /// <summary>
    /// 拖动的旧位置
    /// </summary>
    private Vector2 m_OldDragPos;
    /// <summary>
    /// 拖动的方向
    /// </summary>
    private Vector2 m_DragDir;
    /// <summary>
    /// 交互参数
    /// </summary>
    private int m_PreFinger = -1;

    void Awake()
    {
        Instance = this;
    }

    void OnEnable()
    {
    	//启动时调用，这里开始注册手势操作的事件。
    	
    	//按下事件： OnFingerDown就是按下事件监听的方法，这个名子可以由你来自定义。方法只能在本类中监听。下面所有的事件都一样！！！
        FingerGestures.OnFingerDown += OnFingerDown;
        //抬起事件
		FingerGestures.OnFingerUp += OnFingerUp;
	    //开始拖动事件
	    FingerGestures.OnFingerDragBegin += OnFingerDragBegin;
        //拖动中事件...
        FingerGestures.OnFingerDragMove += OnFingerDragMove;
        //拖动结束事件
        FingerGestures.OnFingerDragEnd += OnFingerDragEnd; 
		//上、下、左、右、四个方向的手势滑动
		FingerGestures.OnFingerSwipe += OnFingerSwipe;
		//连击事件 连续点击事件
		FingerGestures.OnFingerTap += OnFingerTap;
		//按下事件后调用一下三个方法
		FingerGestures.OnFingerStationaryBegin += OnFingerStationaryBegin;
		FingerGestures.OnFingerStationary += OnFingerStationary;
		FingerGestures.OnFingerStationaryEnd += OnFingerStationaryEnd;
		//长按事件
		FingerGestures.OnFingerLongPress += OnFingerLongPress;
		
    }

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(OnZoom != null)
            {
                OnZoom(ZoomType.Out);
            }
        }else if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (OnZoom != null)
            {
                OnZoom(ZoomType.In);
            }
        }
#elif UNITY_ANDROID || UNITY_IPHONE
         //两根手指触摸屏幕
        if(Input.touchCount > 1)
        {
            //其中一根手指在移动
            if(Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                m_TempFinger1Pos = Input.GetTouch(0).position;
                m_TempFinger2Pos = Input.GetTouch(1).position;

                if(Vector2.Distance(m_OldFinger1Pos,m_OldFinger2Pos) < Vector2.Distance(m_TempFinger1Pos,m_TempFinger2Pos))
                {
                    //表示放大
                    OnZoom(ZoomType.In);
                }
                else
                {
                    //缩小
                    OnZoom(ZoomType.Out);
                }

                m_OldFinger1Pos = m_TempFinger1Pos;
                m_OldFinger2Pos = m_TempFinger2Pos;
            }
        }
#endif

    }

    void OnDisable()
    {
    	//关闭时调用，这里销毁手势操作的事件
    	//和上面一样
        FingerGestures.OnFingerDown -= OnFingerDown;
		FingerGestures.OnFingerUp -= OnFingerUp;
		FingerGestures.OnFingerDragBegin -= OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= OnFingerDragEnd; 
		FingerGestures.OnFingerSwipe -= OnFingerSwipe;
		FingerGestures.OnFingerTap -= OnFingerTap;
		FingerGestures.OnFingerStationaryBegin -= OnFingerStationaryBegin;
		FingerGestures.OnFingerStationary -= OnFingerStationary;
		FingerGestures.OnFingerStationaryEnd -= OnFingerStationaryEnd;
		FingerGestures.OnFingerLongPress -= OnFingerLongPress;
    }

    //按下时调用
    void OnFingerDown( int fingerIndex, Vector2 fingerPos )
    {
        m_PreFinger = 1;
    }
	
	//抬起时调用
	void OnFingerUp( int fingerIndex, Vector2 fingerPos, float timeHeldDown )
	{
		if(m_PreFinger == 1)
        {
            m_PreFinger = -1;
            if(OnPlayerClickGround != null)
            {
                OnPlayerClickGround();
            }
        }
	}
	
	//开始滑动
	void OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
        m_PreFinger = 2;
        m_OldDragPos = fingerPos;
    }
	//滑动结束
	void OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
	{
        m_PreFinger = 4;
    }
	//滑动中
    void OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
        m_PreFinger = 3;
        m_DragDir = fingerPos - m_OldDragPos;
        if(m_DragDir.y < m_DragDir.x && m_DragDir.y > -m_DragDir.x)
        {
            //向右
            if(OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Right);
            }
        }else if(m_DragDir.y > m_DragDir.x && m_DragDir.y < -m_DragDir.x)
        {
            //向左
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Left);
            }
        }
        else if(m_DragDir.y > m_DragDir.x && m_DragDir.y > -m_DragDir.x)
        {
            //向上
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Up);
            }
        }
        else
        {
            //向下
            if (OnFingerDrag != null)
            {
                OnFingerDrag(FingerDir.Down);
            }
        }
    }
	//上下左右四方方向滑动手势操作
	void OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {

    }
	
	//连续按下事件， tapCount就是当前连续按下几次
	void OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
			
    }
	
	//按下事件开始后调用，包括 开始 结束 持续中状态只到下次事件开始！
	void OnFingerStationaryBegin( int fingerIndex, Vector2 fingerPos )
	{
		
	}
	
	
	void OnFingerStationary( int fingerIndex, Vector2 fingerPos, float elapsedTime )
	{
		
	}
	
	void OnFingerStationaryEnd( int fingerIndex, Vector2 fingerPos, float elapsedTime )
	{
		
	}
	
	
	//长按事件
	void OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
	{
		
	}
	
	//把Unity屏幕坐标换算成3D坐标
    Vector3 GetWorldPos( Vector2 screenPos )
    {
        Camera mainCamera = Camera.main;
        return mainCamera.ScreenToWorldPoint( new Vector3( screenPos.x, screenPos.y, Mathf.Abs( transform.position.z - mainCamera.transform.position.z ) ) ); 
    }
}
