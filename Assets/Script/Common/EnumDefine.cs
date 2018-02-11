// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 14:38:30
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;


#region RoleType 角色类型
public enum RoleType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 主角
    /// </summary>
    MainPlayer = 1,
    /// <summary>
    /// 怪
    /// </summary>
    Monster = 2
}
#endregion

#region RoleState 角色状态
/// <summary>
/// 角色状态
/// </summary>
public enum RoleState
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 待机状态
    /// </summary>
    Idle = 1,
    /// <summary>
    /// 跑状态
    /// </summary>
    Run = 2,
    /// <summary>
    /// 攻击状态
    /// </summary>
    Attack = 3,
    /// <summary>
    /// 受伤状态
    /// </summary>
    Hurt = 4,
    /// <summary>
    /// 死亡状态
    /// </summary>
    Die = 5
}
#endregion


/// <summary>
/// 角色动画状态名称
/// </summary>
public enum RoleAnimatorName
{
    Idle_Normal,
    Idle_Fight,
    Run,
    Hurt,
    Die,
    PhyAttack1,
    PhyAttack2,
    PhyAttack3
}

/// <summary>
/// 角色动画条件
/// </summary>
public enum ToAnimatorCondition
{
    ToIdleNormal,
    ToIdleFight,
    ToRun,
    ToPhyAttack,
    ToHurt,
    ToDie,
    CurrState
}

