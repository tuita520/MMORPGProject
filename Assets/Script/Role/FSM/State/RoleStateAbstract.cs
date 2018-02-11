// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 14:25:05
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 角色状态的抽象基类
/// </summary>
public abstract class RoleStateAbstract {

    /// <summary>
    /// 当前角色有限状态机管理器
    /// </summary>
    public RoleFSMMgr CurrRoleFSMMgr { get; private set; }
    
    /// <summary>
    /// 当前动画状态信息
    /// </summary>
    public AnimatorStateInfo CurrRoleAnimatorStateInfo { get;  set; }


    public RoleStateAbstract(RoleFSMMgr roleFSMMgr)
    {
        CurrRoleFSMMgr = roleFSMMgr;
    }

    /// <summary>
    /// 进入状态
    /// </summary>
    public virtual void OnEnter() { }

    /// <summary>
    /// 执行状态
    /// </summary>
    public virtual void OnUpdate() { }

    /// <summary>
    /// 离开状态
    /// </summary>
    public virtual void OnLeave() { }
}
