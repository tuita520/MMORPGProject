// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-11 16:55:44
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色管理器
/// </summary>
public class RoleMgr : Singleton<RoleMgr> {

    private Dictionary<string, GameObject> m_Role = new Dictionary<string, GameObject>();

    #region LoadPlayer 根据角色预设名称 克隆角色
    /// <summary>
    /// 根据角色预设名称 克隆角色
    /// </summary>
    /// <param name="name">角色名称</param>
    /// <returns></returns>
    public GameObject LoadPlayer(string name)
    {
        GameObject obj = null;
        if (m_Role.ContainsKey(name))
        {
            obj = m_Role[name];
        }
        else
        {
            obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.Role, string.Format("Player/{0}", name), cache: true);
            m_Role.Add(name, obj);
        }
        
        return obj;
    }
    #endregion

    /// <summary>
    /// 卸载清空资源
    /// </summary>
    public override void Dispose()
    {
        m_Role.Clear();

    }
}
