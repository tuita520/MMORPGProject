// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-23 16:55:41
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Http 通讯管理
/// </summary>
public class NetWorkHttp : MonoBehaviour
{

    #region 单例
    private static NetWorkHttp instance;

    public static NetWorkHttp Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("NetWorkHttp");
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<NetWorkHttp>();
            }
            return instance;
        }
    }
    #endregion
    /// <summary>
    /// Web请求回调
    /// </summary>
    private Action<CallBackArgs> m_CallBack;
    private CallBackArgs m_CallBackArgs;

    void Start()
    {
        m_CallBackArgs = new CallBackArgs();
    }

    public void SendData(string url, Action<CallBackArgs> callBack, bool isPost = false, string json = "")
    {
        m_CallBack = callBack;

        if (!isPost)
        {
            GetUrl(url);
        }
        else
        {
            PostUrl(url, json);
        }
    }

    #region GetUrl Get请求
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url"></param>
    private void GetUrl(string url)
    {
        WWW data = new WWW(url);
        StartCoroutine(Get(data));
    }

    private IEnumerator Get(WWW data)
    {
        yield return data;
        if (string.IsNullOrEmpty(data.error))
        {
            if (data.text == "null")
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.IsError = true;
                    m_CallBackArgs.Error = "未请求到数据";
                    m_CallBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.IsError = false;
                    m_CallBackArgs.Json = data.text;
                    m_CallBack(m_CallBackArgs);
                }
            }
            
        }
        else
        {
            
            if (m_CallBack != null)
            {
                m_CallBackArgs.IsError = true;
                m_CallBackArgs.Error = data.error;
                m_CallBack(m_CallBackArgs);
            }
        }
    }
    #endregion


    #region PostUrl post请求
    /// <summary>
    /// post 请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json"></param>
    private void PostUrl(string url, string json)
    {

    }
    #endregion

    /// <summary>
    /// 数据回调
    /// </summary>
    public class CallBackArgs
    {
        /// <summary>
        /// 是否报错
        /// </summary>
        public bool IsError;
        /// <summary>
        /// 错误原因
        /// </summary>
        public string Error;
        /// <summary>
        /// json数据
        /// </summary>
        public string Json;
    }

}
