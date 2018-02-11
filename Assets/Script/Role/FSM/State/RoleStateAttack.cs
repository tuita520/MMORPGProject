// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 14:27:07
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

public class RoleStateAttack : RoleStateAbstract {

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateAttack(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        //播放待机的动画 从任意状态进入Idle状态并设置当前状态为Idle
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.ToPhyAttack.ToString(), 1);


    }

    /// <summary>
    /// 实现基类执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorName.PhyAttack1.ToString()))
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Attack);
            //如果动画执行一遍之后自动切换待机
            if (CurrRoleAnimatorStateInfo.normalizedTime > 1)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle();
            }
        }
    }

    /// <summary>
    /// 实现基类离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.ToPhyAttack.ToString(), 0);
    }
}
