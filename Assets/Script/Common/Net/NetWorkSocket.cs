// ========================================================
// 描 述：
// 作 者：牛水鱼 
// 创建时间：2018-02-24 15:00:24
// 版 本：v 1.0
// ========================================================
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;

public class NetWorkSocket : MonoBehaviour {

    #region 单例
    private static NetWorkSocket instance;

    public static NetWorkSocket Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("NetWorkSocket");
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<NetWorkSocket>();
            }
            return instance;
        }
    }
    #endregion

    /// <summary>
    /// 客户端Socket
    /// </summary>
    private Socket client;

    /// <summary>
    /// 连接到Socket服务器
    /// </summary>
    /// <param name="ip">ip</param>
    /// <param name="port">端口号</param>
    public void Connect(string ip,int port)
    {
        //如果Socket已经存在 并且处于连接状态 直接返回
        if (client != null && client.Connected) return;

        client = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);

        try
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(ip),port));
            Debug.Log("连接成功");
        }
        catch (System.Exception ex)
        {
            Debug.Log("连接失败 "+ ex.Message);
        }
    }

    void OnDestroy()
    {
        if(client != null && client.Connected)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}
