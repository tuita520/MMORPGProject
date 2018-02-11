// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 14:29:56
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 角色信息基类
/// </summary>
public class RoleInfoBase
{
    /// <summary>
    /// 服务器编号
    /// </summary>
    public int RoleServerID;
    /// <summary>
    /// 角色编号
    /// </summary>
    public int RoleID;
    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName;
    /// <summary>
    /// 最大血量
    /// </summary>
    public int MaxHP;
    /// <summary>
    /// 当前血量
    /// </summary>
    public int currHP;
}
