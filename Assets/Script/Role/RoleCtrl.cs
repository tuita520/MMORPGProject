// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-01-12 10:21:52
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 角色控制器
/// </summary>
public class RoleCtrl : MonoBehaviour
{
    #region 成员变量或属性
    /// <summary>
    /// 动画
    /// </summary>
    [SerializeField]
    public Animator Animator;

    //移动速度
    private float m_Speed = 0.1f;
    //移动的目标位置
    private Vector3 m_TargetPos;
    //角色控制组件
    private CharacterController m_CharacterController;
    //转身速度
    private float m_RosSpeed = 10f;


    /// <summary>
    /// 当前角色类型
    /// </summary>
    public RoleType CurrRoleType = RoleType.None;
    /// <summary>
    /// 当前角色信息
    /// </summary>
    public RoleInfoBase CurrRoleInfo = null;
    /// <summary>
    /// 当前角色AI
    /// </summary>
    public IRoleAI CurrRoleAI = null;
    /// <summary>
    /// 当前角色有限状态机管理器
    /// </summary>
    public RoleFSMMgr currRoleFSMMgr = null;
    #endregion

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="roleType">角色类型</param>
    /// <param name="roleInfo">角色信息</param>
    /// <param name="ai">AI</param>
    public void Init(RoleType roleType,RoleInfoBase roleInfo,IRoleAI ai)
    {
        CurrRoleType = roleType;
        CurrRoleInfo = roleInfo;
        CurrRoleAI = ai;
    }


    #region Start 组件初始化
    /// <summary>
    /// 组件初始化
    /// </summary>
    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        if(CameraCtr.Instance != null)
        {
            CameraCtr.Instance.Init();
        }

        currRoleFSMMgr = new RoleFSMMgr(this);

        if(FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
            FingerEvent.Instance.OnPlayerClickGround += OnPlayerClickGround;
            FingerEvent.Instance.OnZoom += OnZoom;
        }
        
    }
    #endregion

    #region OnZoom 摄像机缩放
    /// <summary>
    /// 摄像机缩放
    /// </summary>
    /// <param name="obj"></param>
    private void OnZoom(FingerEvent.ZoomType obj)
    {
        switch (obj)
        {
            case FingerEvent.ZoomType.In:
                CameraCtr.Instance.SetCameraZoom(0);
                break;
            case FingerEvent.ZoomType.Out:
                CameraCtr.Instance.SetCameraZoom(1);
                break;
            default:
                break;
        }
    }
    #endregion

    #region OnPlayerClickGround 鼠标点击地面
    /// <summary>
    /// 鼠标点击地面
    /// </summary>
    private void OnPlayerClickGround()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name.Equals("Ground"))
            {
                m_TargetPos = hit.point;
                m_RosSpeed = 0;
            }
        }
    }
    #endregion

    #region OnFingerDrag 手指滑动
    /// <summary>
    /// 手指滑动
    /// </summary>
    /// <param name="obj"></param>
    private void OnFingerDrag(FingerEvent.FingerDir obj)
    {
        switch (obj)
        {
            case FingerEvent.FingerDir.Up:
                CameraCtr.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Down:
                CameraCtr.Instance.SetCameraUpAndDown(0);
                break;
            case FingerEvent.FingerDir.Left:
                CameraCtr.Instance.SetCameraRotate(1);
                break;
            case FingerEvent.FingerDir.Right:
                CameraCtr.Instance.SetCameraRotate(0);
                break;
            default:
                break;
        }
    }
    #endregion

    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {
        FingerEvent.Instance.OnFingerDrag -= OnFingerDrag;
        FingerEvent.Instance.OnPlayerClickGround -= OnPlayerClickGround;
        FingerEvent.Instance.OnZoom -= OnZoom;
    }
    #endregion

    #region Update 角色移动
    /// <summary>
    /// 角色移动
    /// </summary>
    void Update()
    {
        //如果角色没有AI 直接返回
        //if (CurrRoleAI == null) return;

        //每帧执行
        if(currRoleFSMMgr != null)
        {
            currRoleFSMMgr.OnUpdate();
        }

        //执行AI方法
        //CurrRoleAI.DoAI();

        if (m_CharacterController == null) return;

        //判断当前CharacterController组件是否接地
        if (!m_CharacterController.isGrounded)
        {
            //让角色贴着地面
            m_CharacterController.Move((transform.position + new Vector3(0,-1000,0))- transform.position);
        }

        if(m_TargetPos != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, m_TargetPos) > 0.5f)
            {
                //移动的方向
                Vector3 direction = m_TargetPos - transform.position;
                direction = direction.normalized;
                Quaternion m_TargetRos = Quaternion.identity;

                if(m_RosSpeed <= 1f)
                {
                    m_RosSpeed += 4f * Time.deltaTime;
                    m_TargetRos = Quaternion.LookRotation(direction);
                    //当m_RosSpeed==1的时候根据lerp函数此时已经将transform转身完毕了
                    transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetRos,m_RosSpeed);
                }

                //transform.LookAt(new Vector3(m_TargetPos.x, transform.position.y, m_TargetPos.z));
                m_CharacterController.Move(direction * m_Speed);
            }
        }

        //摄像机自动跟随
        CameraAutoFollow();

        if (Input.GetKeyUp(KeyCode.R))
        {
            ToRun();
        }else if (Input.GetKeyUp(KeyCode.N))
        {
            ToIdle();
        }else if (Input.GetKeyUp(KeyCode.A))
        {
            ToAttack();
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            ToHurt();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            ToDie();
        }
    }
    #endregion

    #region 角色控制方法

    public void ToIdle()
    {
        currRoleFSMMgr.ChangeState(RoleState.Idle);
    }

    public void ToRun()
    {
        currRoleFSMMgr.ChangeState(RoleState.Run);
    }

    public void ToAttack()
    {
        currRoleFSMMgr.ChangeState(RoleState.Attack);
    }

    public void ToHurt()
    {
        currRoleFSMMgr.ChangeState(RoleState.Hurt);
    }
    public void ToDie()
    {
        currRoleFSMMgr.ChangeState(RoleState.Die);
    }

    #endregion

    #region CameraAutoFollow 摄像机自动跟随
    /// <summary>
    /// 摄像机自动跟随
    /// </summary>
    private void CameraAutoFollow()
    {
        if (CameraCtr.Instance == null) return;

        CameraCtr.Instance.transform.position = transform.position;
        CameraCtr.Instance.AutoLookAt(gameObject.transform.position);
    }
    #endregion
}
