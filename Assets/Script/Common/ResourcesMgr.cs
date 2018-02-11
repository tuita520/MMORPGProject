// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-06 15:48:10
// 版 本：v 1.0
// ========================================================
using System.Collections;
using System.Text;
using UnityEngine;

public class ResourcesMgr : Singleton<ResourcesMgr>
{
    public enum ResourceType
    {
        //场景UI
        UIScene,
        //窗口
        UIWindow,
        //角色
        Role,
        //特效
        Effect
    }

    //预设列表
    private Hashtable m_PrefabTable;

    public ResourcesMgr()
    {
        m_PrefabTable = new Hashtable();
    }

    public GameObject Load(ResourceType type, string path, bool cache = false)
    {
        GameObject obj = null;

        if (m_PrefabTable.Contains(path))
        {
            Debug.Log("资源从缓存中加载");
            obj = m_PrefabTable[path] as GameObject;

        }
        else
        {
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case ResourceType.UIScene:
                    sb.Append("UIPrefab/UIScene/");
                     break;
                case ResourceType.UIWindow:
                    sb.Append("UIPrefab/UIWindows/");
                     break;
                case ResourceType.Role:
                    sb.Append("RolePrefab/");
                    break;
                case ResourceType.Effect:
                    sb.Append("EffectPrefab/");
                    break;
            }
            sb.Append(path);

            obj = Resources.Load(sb.ToString()) as GameObject;
            //判断当前资源是否需要缓存
            if (cache)
            {
                m_PrefabTable.Add(path, obj);
            }
        }
        return GameObject.Instantiate(obj);
    }

    //卸载不需要的资源（切换场景的时候）
    public override void Dispose()
    {
        base.Dispose();
        m_PrefabTable.Clear();
        Resources.UnloadUnusedAssets();
    }
}