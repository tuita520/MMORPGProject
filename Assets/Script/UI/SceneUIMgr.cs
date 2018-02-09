// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-06 16:10:40
// 版 本：v 1.0
// ========================================================
using UnityEngine;
/// <summary>
/// 场景UI管理器
/// </summary>
public class SceneUIMgr : Singleton<SceneUIMgr>
{
    //场景UI类型
    public enum SceneUIType
    {
        //登录
        LogOn,
        //加载
        Loading,
        //主城
        MainCity
    }

    //加载场景UI
    public GameObject LoadSceneUI(SceneUIType type)
    {
        GameObject obj = null;
        switch (type)
        {
            case SceneUIType.LogOn:
                obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIScene, "UI Root_LogOnScene");
                break;
            case SceneUIType.Loading:

                break;
            case SceneUIType.MainCity:

                break;
        }

        return obj;
    }
}
