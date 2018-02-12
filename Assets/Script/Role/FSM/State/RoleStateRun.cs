// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 14:25:59
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

public class RoleStateRun : RoleStateAbstract
{

    //转身速度
    private float m_RosSpeed = 10f;
    /// <summary>
    /// 转身的目标方向
    /// </summary>
    private Quaternion m_TargetQuaternion;

    private Vector3 tempTargetPos = Vector3.zero;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateRun(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        m_RosSpeed = 0;
        //播放待机的动画 从任意状态进入Idle状态并设置当前状态为Idle
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToRun.ToString(), true);
    }

    /// <summary>
    /// 实现基类执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorName.Run.ToString()))
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Run);
        }else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }

        //优化转身
        if(tempTargetPos != CurrRoleFSMMgr.CurrRoleCtrl.TargetPos)
        {
            m_RosSpeed = 0;
        }

        if (Vector3.Distance(CurrRoleFSMMgr.CurrRoleCtrl.transform.position, CurrRoleFSMMgr.CurrRoleCtrl.TargetPos) > 0.1f)
        {
            //记录上次鼠标点击地面的坐标
            tempTargetPos = CurrRoleFSMMgr.CurrRoleCtrl.TargetPos;
            //移动的方向
            Vector3 direction = CurrRoleFSMMgr.CurrRoleCtrl.TargetPos - CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
            direction = direction.normalized;
            m_TargetQuaternion = Quaternion.identity;

            if (m_RosSpeed <= 1f)
            {
                m_RosSpeed += 10f * Time.deltaTime;
                m_TargetQuaternion = Quaternion.LookRotation(direction);
                //当m_RosSpeed==1的时候根据lerp函数此时已经将transform转身完毕了
                CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation = Quaternion.Lerp(CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation, m_TargetQuaternion, m_RosSpeed);
            }

            CurrRoleFSMMgr.CurrRoleCtrl.CharacterController.Move(direction * CurrRoleFSMMgr.CurrRoleCtrl.m_Speed);
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.ToIdle();
        }
    }

    /// <summary>
    /// 实现基类离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToRun.ToString(), false);
    }
}
