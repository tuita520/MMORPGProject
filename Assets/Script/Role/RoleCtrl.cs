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
    [HideInInspector]
    public float m_Speed = 0.1f;
    //移动的目标位置
    [HideInInspector]
    public Vector3 TargetPos;
    //角色控制组件
    [HideInInspector]
    public CharacterController CharacterController;

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
    public void Init(RoleType roleType, RoleInfoBase roleInfo, IRoleAI ai)
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
        CharacterController = GetComponent<CharacterController>();
        if (CurrRoleType == RoleType.MainPlayer)
        {
            if (CameraCtr.Instance != null)
            {
                Debug.Log("摄像机初始化");
                CameraCtr.Instance.Init();
            }
        }

        currRoleFSMMgr = new RoleFSMMgr(this);
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
        if (currRoleFSMMgr != null)
        {
            currRoleFSMMgr.OnUpdate();
        }

        //执行AI方法
        //CurrRoleAI.DoAI();

        if (CharacterController == null) return;

        //判断当前CharacterController组件是否接地
        if (!CharacterController.isGrounded)
        {
            //让角色贴着地面
            CharacterController.Move((transform.position + new Vector3(0, -1000, 0)) - transform.position);
        }


        if (CurrRoleType == RoleType.MainPlayer)
            //摄像机自动跟随
            CameraAutoFollow();

        //if (Input.GetKeyUp(KeyCode.R))
        //{
        //    ToRun();
        //}else if (Input.GetKeyUp(KeyCode.N))
        //{
        //    ToIdle();
        //}else if (Input.GetKeyUp(KeyCode.A))
        //{
        //    ToAttack();
        //}
        //if (Input.GetKeyUp(KeyCode.H))
        //{
        //    ToHurt();
        //}
        //if (Input.GetKeyUp(KeyCode.D))
        //{
        //    ToDie();
        //}
    }
    #endregion

    #region 角色控制方法

    public void ToIdle()
    {
        currRoleFSMMgr.ChangeState(RoleState.Idle);
    }

    public void MoveTo(Vector3 targetPos)
    {
        Debug.Log("进行移动");

        if (targetPos == Vector3.zero) return;

        TargetPos = targetPos;
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





    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    private void OnDestroy()
    {

    }
    #endregion
}
